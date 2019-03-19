using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WS
{
    public class FieldObjResource : FieldObjWithCircle
    {
        public MasterDataFieldObject data { get; private set; }

        public void Init(int id)
        {
            this.data = MasterDataManager.FieldObject.GetData(id);
            this.SetName(data.name,data.color);
        }

        public override string GetHintName()
        {
            if (string.IsNullOrEmpty(data.color)) return data.name;
            var str = "<color=#{0}>{1}</color>";
            return string.Format(str, data.color, data.name);
        }

        public override string GetDesc()
        {
            return this.data.desc;
        }
    }
}
