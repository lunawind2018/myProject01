using System.Collections;
using System.Collections.Generic;
using MyEvent;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

namespace WS
{
    public class FieldMap : MonoBehaviour,IPointerDownHandler
    {
        public static FieldMap Instance { get; private set; }

        public static int Map_Size = 100;

        public static float GridSize = 50;

        public Transform playerLayer { get; private set; }
        private Transform mapLayer;
        //private Transform mapLayerUp;
        private Transform monsterLayer;

        private FieldMapDataManager mapDataManager;
        private int[][] currMapData;

        private List<FieldObjectNpc> monsterList = new List<FieldObjectNpc>();

        void Awake()
        {
            playerLayer = transform.Find("PlayerLayer").transform;
            mapLayer = transform.Find("MapLayer").transform;
            monsterLayer = transform.Find("MonsterLayer").transform;
            //mapLayerUp = transform.Find("MapLayerUp").transform;
            mapDataManager = FieldMapDataManager.Instance;
            Instance = this;
        }

        // Use this for initialization
        void Start()
        {
            StartCoroutine(LoadMap());
        }

        private IEnumerator LoadMap()
        {
            var mapdata = "mapdata.dat";
            var url = Application.persistentDataPath + "/" + mapdata;
            var webrequest = UnityWebRequest.Get(url);
            yield return webrequest.Send();
            if (webrequest.isNetworkError)
            {
                mapDataManager.GenerateFieldMapData();
                mapDataManager.SaveMapData(url);
            }
            else
            {
                byte[] data = webrequest.downloadHandler.data;
                mapDataManager.LoadFromData(data);
            }
            StartCoroutine(DrawMap(0, 0, OnMapGenerated));
        }

        private void OnMapGenerated()
        {
            Debug.Log("===MAP OK!!!");
            MyEventSystem.SendEvent(new MyGameEvent(MyGameEvent.MAP_OK));
        }

        public IEnumerator DrawMap(int x, int y, System.Action callback)
        {
            currMapData = mapDataManager.GetMapData(x, y);
            if (currMapData == null)
            {
                Debug.LogError("...");
                yield break;
            }
            var watch = new System.Diagnostics.Stopwatch();
            var obj = Resources.Load("Prefab/FieldObj") as GameObject;
            watch.Start();
            var interval = Time.fixedDeltaTime*0.99f*1000000;
            for (int i = 0; i < Map_Size; i++)
            {
                for (int j = 0; j < Map_Size; j++)
                {
                    if (currMapData[i] == null) continue;
                    var d = currMapData[i][j];
                    if (d == 0) continue;
                    //Debug.Log("obj " + i + "," + j + "  " + currMapData[i][j]);
                    var newObj = Instantiate(obj);
                    var sc = newObj.AddComponent<FieldObjResource>();
                    sc.Init(d);
                    sc.world_x = i;
                    sc.world_y = j;
                    Utils.SetParent(sc.transform, this.mapLayer);
                    sc.transform.localPosition = new Vector3(i*GridSize, j*GridSize);
                    if (watch.ElapsedMilliseconds >= interval)
                    {
                        watch.Reset();
                        watch.Start();
                        yield return 0;
                    }
                }
            }
            callback.Invoke();
        }


        private float monsterGenTime = 0f;
        private int maxMonster = 1;
        // Update is called once per frame
        void Update()
        {
            if (monsterList.Count < maxMonster)
            {
                monsterGenTime+=Time.deltaTime;
                if (monsterGenTime > 1)
                {
                    GenerateMonster(1);
                }
            }
        }

        private void GenerateMonster(int id)
        {
            var rand = new System.Random();
            var playerPos = PlayerManager.Instance.playerData.Position;
            var r1 = rand.Next(100) / 100f + 0.5f;
            var r2 = rand.Next(100) / 100f + 0.5f;
            var r3 = rand.Next(100) / 2f + 200f;
            var r4 = rand.Next(100) / 2f + 200f;
            var newpos = new Vector2(playerPos.x + r1 * r3, playerPos.y + r2 * r4);
            //Debug.Log(playerPos + " " + r1 + " " + r2 + " " + r3 + " " + newpos);
            var prefab = Resources.Load<GameObject>("Prefab/FieldMonster");
            var monster = Instantiate(prefab);
            monster.name = "monster_" + this.monsterList.Count;
            Utils.SetParent(monster.transform, this.monsterLayer);
            var sc = monster.AddComponent<FieldMonster>();
            sc.transform.localPosition = newpos;
            sc.Init(id);
            this.monsterList.Add(sc);
        }

        public void RemoveObject(FieldObject obj)
        {
            FieldPlayer.Instance.UnSelectObj(obj);
            if (obj is FieldObjResource)
            {
                RemoveObject(obj as FieldObjResource);
            }
            else if(obj is FieldMonster)
            {
                RemoveObject(obj as FieldMonster);
            }
        }

        private void RemoveObject(FieldMonster obj)
        {
            if (monsterList.Contains(obj))
            {
                monsterList.Remove(obj);
                DestroyImmediate(obj.gameObject);
            }
            else
            {
                Debug.LogError("obj error");
                return;
            }
        }
        private void RemoveObject(FieldObjResource obj)
        {
            var x = obj.world_x;
            var y = obj.world_y;
            if (currMapData[x][y] != obj.data.intid)
            {
                Debug.LogError("obj error");
                return;
            }
            currMapData[x][y] = 0;
            DestroyImmediate(obj.gameObject);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            var id = eventData.pointerId;
            if (id == -1)
            {
//                Debug.Log("mouse left");
                MyEventSystem.SendEvent(new MyKeyEvent(MyKeyEvent.KEY_DOWN, KeyControl.M_LEFT));
            }
            else if (id == -2)
            {
//                Debug.Log("mouse right");
                MyEventSystem.SendEvent(new MyKeyEvent(MyKeyEvent.KEY_DOWN, KeyControl.M_RIGHT));
            }
        }
    }
}