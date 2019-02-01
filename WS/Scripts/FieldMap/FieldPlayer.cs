using System.Collections;
using System.Collections.Generic;
using MyEvent;
using UnityEngine;
using UnityEngine.UI;

namespace WS
{
    public class FieldPlayer : FieldObject
    {
        public static FieldPlayer Instance { get; private set; }

        public const string PLAYER_BODY = "PlayerBody";

        public float speed = 1;
        private Vector2 moveSpeed = new Vector2(10, 10);

        private Vector2 InputDir;

        private List<FieldObjWithCircle> touchObjList = new List<FieldObjWithCircle>();
        private FieldObjWithCircle currSelectObject;

        public bool canCtrl = true;

        [SerializeField] private Slider castBar;

        protected override void Awake()
        {
            if (Instance != null)
            {
                Debug.Log("multi player!!");
                Instance.DestroySelf();
                Instance = this;
            }
            bodyName = PLAYER_BODY;
            base.Awake();
            RegisterEvents();
            castBar.gameObject.SetActive(false);
        }

        private void DestroySelf()
        {
            DestroyImmediate(this.gameObject);
        }

        void Destroy()
        {
            UnRegisterEvents();
        }

        private void RegisterEvents()
        {
            MyEventSystem.RegistEvent(MyKeyEvent.KEY_DOWN, OnKeyDownHandler);
            MyEventSystem.RegistEvent(MyKeyEvent.KEY_UP, OnKeyUpHandler);
            MyEventSystem.RegistEvent(MyTriggerEvent.ON_PLAYER_ENTER, OnTriggerEnterHandler);
            MyEventSystem.RegistEvent(MyTriggerEvent.ON_PLAYER_EXIT, OnTriggerExitHandler);
        }


        private void UnRegisterEvents()
        {
            MyEventSystem.UnRegistEvent(MyTriggerEvent.ON_PLAYER_ENTER, OnTriggerEnterHandler);
            MyEventSystem.UnRegistEvent(MyTriggerEvent.ON_PLAYER_EXIT, OnTriggerExitHandler);

        }


        // Use this for initialization
        void Start()
        {
            PlayerCamera.Instance.InitPlayer(this.transform);
        }

        private float timeCount = 0f;
        private int timeIndex = 0;
        private float[] timeArr = new[] {0.1f, 0.2f, 0.3f, 0.4f, 0.5f, 0.6f, 0.7f, 0.8f, 0.9f, 1f};
        // Update is called once per frame
        void Update()
        {
            if (this.body.transform.localPosition != Vector3.zero)
            {
                this.transform.localPosition += this.body.transform.localPosition;
                this.body.transform.localPosition = Vector3.zero;
            }
            timeCount += Time.deltaTime;
            if (timeCount >= timeArr[timeIndex])
            {
                UpdateTen();
                timeIndex++;
                if (timeIndex == 5)
                {
                    UpdateTwo();
                }
                if (timeIndex >= 10)
                {
                    timeIndex -= 10;
                    timeCount -= 1;
                    UpdateTwo();
                    UpdateOne();
                }
            }
        }

        private void UpdateTen()
        {
            
        }

        private void UpdateOne()
        {
            
        }

        private void UpdateTwo()
        {
            
        }

        void FixedUpdate()
        {
            if (canCtrl && (InputDir.x != 0 || InputDir.y != 0))
            {
                //move
                this.transform.Translate(InputDir.x * moveSpeed.x * Time.fixedDeltaTime * speed,
                    InputDir.y * moveSpeed.y * Time.fixedDeltaTime * speed, 0);
                //stop channeling
                StopCanneling();
                
            }

            if (isCanneling)
            {
                currCannelTime += Time.deltaTime;
                this.castBar.value = currCannelTime/cannelTime;
                if (currCannelTime > cannelTime)
                {
                    StopCanneling();
                    if (tempCom != CommandType.None)
                    {
                        GameManager.Instance.SendCommand(tempCom, tempParam, OnCommandComplete);
                    }
                }
            }
        }

        private void OnCommandComplete()
        {
            
        }

        private bool isCanneling = false;
        private float cannelTime;
        private float currCannelTime;
        private CommandType tempCom;
        private Hashtable tempParam;

        public void StartCanneling(float t)
        {
            this.isCanneling = true;
            this.cannelTime = t;
            this.currCannelTime = 0;
            this.castBar.gameObject.SetActive(true);
            this.castBar.value = 0;
        }

        private void StopCanneling()
        {
            this.isCanneling = false;
            this.castBar.gameObject.SetActive(false);
        }

        private void StartInterAct(FieldObjWithCircle fieldObjWithCircle)
        {
            var obj = fieldObjWithCircle as FieldObjResource;
            if (obj != null)
            {
                var data = obj.data;
                this.tempCom = CommandType.Collect;
                this.tempParam = new Hashtable();
                this.tempParam.Add("data", data);

                StartCanneling(int.Parse(data.param2));

            }
        }

        private bool CanInterAct(FieldObjWithCircle fieldObjWithCircle)
        {
            if (!canCtrl) return false;
            return true;
        }

        private void OnKeyUpHandler(MyEvent.MyEvent obj)
        {
            var key = obj.data.ToString();
            switch (key)
            {
                case KeyControl.UP:
                    if (KeyControl.keyCountDic[KeyControl.UP] == 0)
                    {
                        InputDir.y = KeyControl.keyCountDic[KeyControl.DOWN] > 0 ? -1 : 0;
                    }
                    break;
                case KeyControl.DOWN:
                    if (KeyControl.keyCountDic[KeyControl.DOWN] == 0)
                    {
                        InputDir.y = KeyControl.keyCountDic[KeyControl.UP] > 0 ? 1 : 0;
                    }
                    break;
                case KeyControl.LEFT:
                    if (KeyControl.keyCountDic[KeyControl.LEFT] == 0)
                    {
                        InputDir.x = KeyControl.keyCountDic[KeyControl.RIGHT] > 0 ? 1 : 0;
                    }
                    break;
                case KeyControl.RIGHT:
                    if (KeyControl.keyCountDic[KeyControl.RIGHT] == 0)
                    {
                        InputDir.x = KeyControl.keyCountDic[KeyControl.LEFT] > 0 ? -1 : 0;
                    }
                    break;
            }
        }
        private void OnKeyDownHandler(MyEvent.MyEvent obj)
        {
            var key = obj.data.ToString();
            Debug.Log("key down " + key);
            switch (key)
            {
                case KeyControl.UP:
                    InputDir.y = 1;
                    break;
                case KeyControl.DOWN:
                    InputDir.y = -1;
                    break;
                case KeyControl.LEFT:
                    InputDir.x = -1;
                    break;
                case KeyControl.RIGHT:
                    InputDir.x = 1;
                    break;
                case KeyControl.INTERACT:
                    if (currSelectObject != null)
                    {
                        if (CanInterAct(currSelectObject))
                        {
                            StartInterAct(currSelectObject);
                        }
                    }
                    break;
            }
            //Debug.Log(InputDir.x + "," + InputDir.y);
        }

        private void OnTriggerEnterHandler(MyEvent.MyEvent myevent)
        {
            var obj = (myevent as MyTriggerEvent).data as GameObject;
//            Debug.Log("enter " + obj.name);
            var sc = obj.GetComponent<FieldObjWithCircle>();
            if (sc == null)
            {
                Debug.LogError("no script");
                return;
            }
            if (this.touchObjList.Contains(sc))
            {
                Debug.LogError("???");
            }
            else
            {
                this.touchObjList.Add(sc);
                if (this.touchObjList.Count == 1)
                {
                    sc.SetCircleState(FieldObjWithCircle.CircleState.Selected);
                    currSelectObject = sc;
                }
                else
                {
                    sc.SetCircleState(FieldObjWithCircle.CircleState.Touched);
                }
            }

        }

        private void OnTriggerExitHandler(MyEvent.MyEvent myevent)
        {
            var obj = (myevent as MyTriggerEvent).data as GameObject;
//            Debug.Log("enter " + obj.name);
            var sc = obj.GetComponent<FieldObjWithCircle>();
            if (sc == null)
            {
                Debug.LogError("no script");
                return;
            }
            var index = this.touchObjList.IndexOf(sc);
            if (index>=0)
            {
                sc.SetCircleState(FieldObjWithCircle.CircleState.None);
                this.touchObjList.RemoveAt(index);
                if (this.touchObjList.Count > 0)
                {
                    this.touchObjList[0].SetCircleState(FieldObjWithCircle.CircleState.Selected);
                    currSelectObject = this.touchObjList[0];
                }
                else
                {
                    currSelectObject = null;
                }
            }
            else
            {
                Debug.LogError("???");
            }

        }

        public void SetPosition(Vector3 pos)
        {
            this.transform.localPosition = pos;
        }
    }
}