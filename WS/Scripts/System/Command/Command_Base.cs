using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WS
{
    public abstract class Command_Base
    {
        public static int ShowLog =1;

        public abstract IEnumerator DoCommand(Hashtable param);
    }
}
