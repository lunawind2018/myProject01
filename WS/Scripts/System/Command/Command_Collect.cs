using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WS
{
    public class Command_Collect : Command_Base
    {
        public override IEnumerator DoCommand(Hashtable param)
        {
            var data = param["data"] as MasterDataFieldObject;
            if (data == null)
            {
                Debug.LogError("data error");
                yield break;
            }

            yield return 0;
        }
    }
}
