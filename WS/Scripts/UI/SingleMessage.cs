using UnityEngine;
using UnityEngine.UI;

namespace WS
{
    public class SingleMessage : MonoBehaviour
    {
        [SerializeField]
        private Text uiText;

        public void SetText(string txt)
        {
            uiText.text = txt;
        }
    }
}
