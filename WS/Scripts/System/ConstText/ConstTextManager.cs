using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WS
{
    public class ConstTextManager : MonoBehaviour
    {
        public static string Get(TextId id, params object[] texts)
        {
            var str = MasterDataManager.ConstText.GetData(id.ToString()).text;
            return string.Format(str, texts);
        }
    }

    public enum TextId
    {
        None,
        Msg_GetItem,//"你获得了 {0}"
        Craft_Btn_Craft,//合成
        Craft_Btn_Unlock,//尝试（解锁配方）
        Craft_Btn_Melting,//熔炼
        Cancel,//取消
        UI_Char_Name,//姓名
        Player_Name_Default,
    }
}
