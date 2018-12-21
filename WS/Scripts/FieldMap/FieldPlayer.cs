using System.Collections;
using System.Collections.Generic;
using MyEvent;
using UnityEngine;
using UnityEngine.UI;

namespace WS
{
    public class FieldPlayer : FieldObject
    {
        public const string PLAYER_BODY = "PlayerBody";

        public float speed = 1;
        private Vector2 moveSpeed = new Vector2(10, 10);
        private Vector2 InputSpeed;

        private List<FieldObjWithCircle> touchObjList = new List<FieldObjWithCircle>();
        private FieldObjWithCircle currSelectObject;

        protected override void Awake()
        {
            bodyName = PLAYER_BODY;
            base.Awake();
            RegisterEvents();
        }

        void Destroy()
        {
            UnRegisterEvents();
        }

        private void RegisterEvents()
        {
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

        }

        // Update is called once per frame
        void Update()
        {
            if (this.body.transform.localPosition != Vector3.zero)
            {
                this.transform.localPosition += this.body.transform.localPosition;
                this.body.transform.localPosition = Vector3.zero;
            }
        }

        void FixedUpdate()
        {
            if (InputSpeed.x != 0 || InputSpeed.y != 0)
            {
                //move
                this.transform.Translate(InputSpeed.x*moveSpeed.x*Time.fixedDeltaTime*speed,
                    InputSpeed.y*moveSpeed.y*Time.fixedDeltaTime*speed, 0);
                //stop channeling
                
            }
        }

        void OnTriggerEnterHandler(MyEvent.MyEvent myevent)
        {
            var obj = (myevent as MyTriggerEvent).data as GameObject;
            Debug.Log("enter " + obj.name);
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

        void OnTriggerExitHandler(MyEvent.MyEvent myevent)
        {
            var evt = myevent as MyTriggerEvent;
            var obj = (myevent as MyTriggerEvent).data as GameObject;
            Debug.Log("enter " + obj.name);
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

        void OnGUI()
        {
            InputSpeed.x = GetDir(Input.GetAxis("Horizontal"));
            InputSpeed.y = GetDir(Input.GetAxis("Vertical"));

        }

        private int GetDir(float a)
        {
            if (a == 0) return 0;
            if (a > 0) return 1;
            if (a < 0) return -1;
            return 0;
        }

    }
}