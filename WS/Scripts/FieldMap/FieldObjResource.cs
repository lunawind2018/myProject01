using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WS
{
    public class FieldObjResource : FieldObjWithCircle
    {
        public bool test = false;
        public int testid = 1;

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
        }
    }
}
