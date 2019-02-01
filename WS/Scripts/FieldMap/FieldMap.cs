using System.Collections;
using System.Collections.Generic;
using MyEvent;
using UnityEngine;
using UnityEngine.Networking;

namespace WS
{
    public class FieldMap : MonoBehaviour
    {
        public static FieldMap Instance { get; private set; }

        public static int Map_Size = 100;

        public static float GridSize = 50;

        public Transform playerLayer { get; private set; }
        private Transform mapLayer;
        private Transform mapLayerUp;

        private FieldMapDataManager mapDataManager;
        private int[][] currMapData;

        void Awake()
        {
            playerLayer = GameObject.Find("PlayerLayer").transform;
            mapLayer = GameObject.Find("MapLayer").transform;
            mapLayerUp = GameObject.Find("MapLayerUp").transform;
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



        // Update is called once per frame
        void Update()
        {

        }
    }
}