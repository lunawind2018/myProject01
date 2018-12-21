using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WS
{
    public class PlayerDataManager : MonoBehaviour
    {

        private static PlayerDataManager mInstance;

        public static PlayerDataManager Instance
        {
            get { return mInstance ?? (mInstance = new PlayerDataManager()); }
        }
    }
}