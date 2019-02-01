using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WS
{
    public class FieldObjResource : FieldObjWithCircle
    {
        public bool test = false;
        public int testid = 1;

        public int world_x = 0;
        public int world_y = 0;

        public MasterDataFieldObject data { get; private set; }

        protected override void Awake()
        {
            base.Awake();
        }

        void Start()
        {
            if (test)
            {
                Init(testid);
            }
        }

        public void Init(int id)
        {
            this.data = MasterDataManager.Instance.masterDataFieldObjectDic[id];
            this.nameTxt.text = data.name;
            if (!string.IsNullOrEmpty(data.param3))
            {
                this.nameTxt.color = Utils.GetColor(data.param3);
            }
        }
    }
}
