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
        public Transform BodyTransform { get { return body.transform; } }

        public const string PLAYER_BODY = "PlayerBody";

        private float speed = 100;
        private float fixSpeed;

        private int[] InputDir = new int[2];

        private List<FieldObjWithCircle> touchObjList = new List<FieldObjWithCircle>();
        private int currSelectObject;

        public bool canCtrl = true;

        private bool isMoving;
        private bool changeDir;
        private float faceRot;

        private int canRotate = 0;

        [SerializeField] private Slider castBar;
        private CanvasGroup barGroup;

        protected override void Awake()
        {
            if (Instance != null)
            {
                Debug.Log("multi player!!");
                Instance.DestroySelf();
            }
            Instance = this;
            bodyName = PLAYER_BODY;
            base.Awake();
            RegisterEvents();
            barGroup = castBar.gameObject.GetComponent<CanvasGroup>();
            StopCanneling();
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
            MyEventSystem.UnRegistEvent(MyKeyEvent.KEY_DOWN, OnKeyDownHandler);
            MyEventSystem.UnRegistEvent(MyKeyEvent.KEY_UP, OnKeyUpHandler);
            MyEventSystem.UnRegistEvent(MyTriggerEvent.ON_PLAYER_ENTER, OnTriggerEnterHandler);
            MyEventSystem.UnRegistEvent(MyTriggerEvent.ON_PLAYER_EXIT, OnTriggerExitHandler);

        }

        // Use this for initialization
        void Start()
        {
            PlayerCamera.Instance.InitPlayer(this.transform);
            this.fixSpeed = Time.fixedDeltaTime * speed;
        }

        private float timeCount = 0f;
        private int timeIndex = 0;
        private float[] timeArr = new[] {0.1f, 0.2f, 0.3f, 0.4f, 0.5f, 0.6f, 0.7f, 0.8f, 0.9f, 1f};

        private int[] dirArr = new[] { 45, 0, 315, 90, 0, 270, 135, 180, 225 };

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
            if (canCtrl)
            {
                if (InputDir[0] != 0 || InputDir[1] != 0)
                {
                    var nor = new Vector2(InputDir[0],InputDir[1]).normalized;
                    //move
                    this.transform.Translate(nor.x * fixSpeed, nor.y * fixSpeed, 0);
                    PlayerManager.Instance.playerData.Position = GetPosition();

                    if (!this.isMoving)
                    {
                        this.isMoving = true;
                    }
                    //rotate
                    //dest rotation
                    if (this.changeDir)
                    {
                        var dd = InputDir[0] - InputDir[1] * 3 + 4;
                        if (dd != 4)
                        {
                            this.faceRot = dirArr[dd];
//                            Debug.Log(InputDir[0] + "," + InputDir[1] + " dd " + dd + " " + faceRot);
                            this.changeDir = false;
                        }
                    }
                    if (this.canRotate <= 0)
                    {
                        var zz = this.body.transform.eulerAngles.z;
                        if (zz != this.faceRot)
                        {
                            if (this.faceRot - zz > 180)
                            {
                                this.faceRot -= 360;
                            }
                            else if (zz - this.faceRot > 180)
                            {
                                this.faceRot += 360;
                            }
                            var newz = Mathf.Lerp(zz, this.faceRot, 0.2f);
//                        Debug.Log("z "+zz+" -> "+newz);
                            var d = newz - faceRot;
                            if (d > -1 && d < 1) newz = faceRot;
                            this.SetRotation( new Vector3(0, 0, newz));
                        }
                    }
                    //stop channeling
                    StopCanneling();

                }
                else
                {
                    if (isMoving)
                    {
                        isMoving = false;
                    }
                }
            }

            if (isCanneling)
            {
                //canneling
                currCannelTime += Time.deltaTime;
                this.castBar.value = currCannelTime/cannelTime;
                if (currCannelTime > cannelTime)
                {
                    StopCanneling();
                    GameManager.Instance.SendCommand(tempCom, tempParam, OnCommandComplete);
                }
            }
        }

        private void OnCommandComplete(bool success)
        {
            
        }

        private bool isCanneling = false;
        private float cannelTime;
        private float currCannelTime;
        private CommandType tempCom;
        private object tempParam;

        public void StartCanneling(float t)
        {
            this.isCanneling = true;
            this.cannelTime = t;
            this.currCannelTime = 0;
            this.castBar.value = 0;
            //this.castBar.gameObject.SetActive(true);
            this.barGroup.alpha = 1f;
        }

        private void StopCanneling()
        {
            this.isCanneling = false;
            this.castBar.value = 0;
            //this.castBar.gameObject.SetActive(false);
            this.barGroup.alpha = 0;
        }

        private void StartInteract()
        {
            var obj = touchObjList[currSelectObject];
            
            var res = obj as FieldObjResource;
            if (res != null)
            {
                var toolid = GetCurrTool();
                var res_data = res.data;
                var collect = MasterDataManager.Collect.GetData(res_data.intid, toolid);
                this.tempCom = CommandType.Collect;
                this.tempParam = new Command_Collect.Param
                {
                    obj = res,
                    collect = collect
                };
                StartCanneling(collect.time);
                return;
            }
            var corpse = obj as FieldMonster;
            if (corpse != null)
            {
                var collect = MasterDataManager.Collect.GetData(corpse.GetCorpseId(), 0);
                this.tempCom = CommandType.Collect;
                this.tempParam = new Command_Collect.Param
                {
                    obj = corpse,
                    collect = collect
                };
                StartCanneling(collect.time);
                return;
            }
        }

        private int GetCurrTool()
        {
            return 0;
        }

        private bool CanInteract()
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
                        InputDir[1] = KeyControl.keyCountDic[KeyControl.DOWN] > 0 ? -1 : 0;
                    }
                    this.changeDir = true;
                    break;
                case KeyControl.DOWN:
                    if (KeyControl.keyCountDic[KeyControl.DOWN] == 0)
                    {
                        InputDir[1] = KeyControl.keyCountDic[KeyControl.UP] > 0 ? 1 : 0;
                    }
                    this.changeDir = true;
                    break;
                case KeyControl.LEFT:
                    if (KeyControl.keyCountDic[KeyControl.LEFT] == 0)
                    {
                        InputDir[0] = KeyControl.keyCountDic[KeyControl.RIGHT] > 0 ? 1 : 0;
                    }
                    this.changeDir = true;
                    break;
                case KeyControl.RIGHT:
                    if (KeyControl.keyCountDic[KeyControl.RIGHT] == 0)
                    {
                        InputDir[0] = KeyControl.keyCountDic[KeyControl.LEFT] > 0 ? -1 : 0;
                    }
                    this.changeDir = true;
                    break;
            }
        }
        private void OnKeyDownHandler(MyEvent.MyEvent obj)
        {
            var key = obj.data.ToString();
//            Debug.Log("key down " + key);
            switch (key)
            {
                case KeyControl.UP:
                    InputDir[1] = 1;
                    this.changeDir = true;
                    break;
                case KeyControl.DOWN:
                    InputDir[1] = -1;
                    this.changeDir = true;
                    break;
                case KeyControl.LEFT:
                    InputDir[0] = -1;
                    this.changeDir = true;
                    break;
                case KeyControl.RIGHT:
                    InputDir[0] = 1;
                    this.changeDir = true;
                    break;
                case KeyControl.INTERACT:
                    if (currSelectObject >= 0)
                    {
                        if (CanInteract())
                        {
                            StartInteract();
                        }
                    }
                    break;
                case KeyControl.TAB:
                    if (touchObjList.Count > 1)
                    {
                        SwitchObject();
                    }
                    break;
                case KeyControl.M_LEFT:
                    //Debug.Log("111");
                    if (isCanneling)
                    {
                        StopCanneling();
                    }
                    UseSkill(1);
                    break;
            }
            //Debug.Log(InputDir.x + "," + InputDir.y);
        }

        private void SwitchObject()
        {
            if (this.currSelectObject >= 0)
            {
                this.touchObjList[currSelectObject].SetCircleState(FieldObjWithCircle.CircleState.Touched);
            }
            SelectObject(this.currSelectObject + 1);
        }

        private void SelectObject(int index)
        {
            var l = touchObjList.Count;
            if (l == 0)
            {
                this.currSelectObject = -1;
                UIManager.Instance.HideHint();
                return;
            }
            if (index >= l)
            {
                index = 0;
            }
            var obj = this.touchObjList[index];
            obj.SetCircleState(FieldObjWithCircle.CircleState.Selected);
            this.currSelectObject = index;
            UIManager.Instance.SetHint(obj.GetHintName(), obj.GetDesc());
        }

        private void UseSkill(int id)
        {
            ForceFace();
            SkillManager.Instance.UseSkill(id, this, NotForceFace);
        }

        public void ForceFace()
        {
            this.canRotate++;
            var t = PlayerCamera.Instance.camera.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            Debug.Log(t);
            var ang = -Mathf.Atan2(t.x, t.y) * Mathf.Rad2Deg;
            this.SetRotation(new Vector3(0, 0, ang));
        }

        public void NotForceFace()
        {
            this.canRotate--;
            if (this.canRotate <= 0 && this.isMoving) this.SetRotation(new Vector3(0, 0, faceRot));
        }

        private void OnTriggerEnterHandler(MyEvent.MyEvent myevent)
        {
            var obj = (myevent as MyTriggerEvent).data as GameObject;
//            Debug.Log("enter " + obj.name);
            var sc = obj.GetComponent<FieldObjWithCircle>();
            if (IgnoreTrigger(sc))
            {
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
                    SelectObject(0);
                }
                else
                {
                    sc.SetCircleState(FieldObjWithCircle.CircleState.Touched);
                }
            }

        }

        private bool IgnoreTrigger(FieldObjWithCircle sc)
        {
            if (sc == null)
            {
                Debug.LogError("no script");
                return true;
            }
            if (sc is FieldMonster && !(sc as FieldMonster).IsDead) return true;
            return false;
        }

        private void OnTriggerExitHandler(MyEvent.MyEvent myevent)
        {
            var obj = (myevent as MyTriggerEvent).data as GameObject;
//            Debug.Log("enter " + obj.name);
            var sc = obj.GetComponent<FieldObjWithCircle>();
            if (IgnoreTrigger(sc)) return;
            var index = this.touchObjList.IndexOf(sc);
            if (index>=0)
            {
                sc.SetCircleState(FieldObjWithCircle.CircleState.None);
                this.touchObjList.RemoveAt(index);
                if (index == this.currSelectObject)
                {
                    SelectObject(index);
                }
            }
            else
            {
                Debug.LogError("???");
            }

        }

        public void UnSelectObj(FieldObject obj)
        {
            var sc = obj as FieldObjWithCircle;
            if (IgnoreTrigger(sc)) return;
            var index = this.touchObjList.IndexOf(sc);
            if (index >= 0)
            {
                sc.SetCircleState(FieldObjWithCircle.CircleState.None);
                this.touchObjList.RemoveAt(index);
                if (index == this.currSelectObject)
                {
                    SelectObject(index);
                }
            }
        }

        public void SetPosition(Vector3 pos)
        {
            this.transform.localPosition = pos;
        }

        public Vector3 GetPosition()
        {
            return this.transform.localPosition;
        }

        public void SetRotation(Vector3 a)
        {
            this.body.transform.eulerAngles = a;
            PlayerManager.Instance.playerData.Rotation = a;
        }

        public Vector3 GetRotation()
        {
            return this.body.transform.eulerAngles;
        }
    }
}