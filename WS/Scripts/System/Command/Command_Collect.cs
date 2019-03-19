using System.Collections;
using System.Collections.Generic;
using MyEvent;
using UnityEngine;

namespace WS
{
    public class Command_Collect : Command_Base
    {
        public override IEnumerator DoCommand(object param)
        {
            var pp = param as Param;
            if (pp == null)
            {
                Debug.LogError("collect param error");
                yield break;
            }
            var items = GameManager.Instance.playerManager.Reward(pp.collect.dropItemData);
            //yield return FieldMap.Instance.CollectResource(obj);
            var msg = ConstTextManager.Get(TextId.Msg_GetItem, Utils.GetItemString(items));
            Utils.GlobalMessage(msg);
            if (pp.collect.destroy)
            {
                FieldMap.Instance.RemoveObject(pp.obj);
            }
            yield return 0;
        }

        public class Param
        {
            public FieldObject obj;
            public MasterDataCollect collect;
        }

    }
}
