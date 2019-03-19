using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WS
{
    public class UIWindow_System : UIWindow
    {
        public void OnClickSave()
        {
            SaveDataManager.Instance.SaveSaveData(OnSaveComplete);
        }

        private void OnSaveComplete()
        {
            UIManager.CloseUI(this);
        }
    }
}