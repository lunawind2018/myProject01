using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MJ
{
    public class CardButtonManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject ButtonChi;
        [SerializeField]
        private GameObject ButtonPeng;
        [SerializeField]
        private GameObject ButtonGang;
        [SerializeField]
        private GameObject ButtonReach;
        [SerializeField]
        private GameObject ButtonRong;
        [SerializeField]
        private GameObject ButtonCancel;

        private Transform startPos;

        private List<GameObject> ButtonList;

        // Use this for initialization
        void Start()
        {
            ButtonList = new List<GameObject>()
            {
                ButtonChi,
                ButtonPeng,
                ButtonGang,
                ButtonReach,
                ButtonRong,
                ButtonCancel
            };
            Reset();
        }

        public void Reset()
        {
            foreach (var btn in ButtonList)
            {
                if (btn.activeSelf) btn.SetActive(false);
            }
        }

        public void UpdateButtonState(int statecode)
        {
            ButtonChi.SetActive((statecode & 1) > 0);
            ButtonPeng.SetActive((statecode & 2) > 0);
            ButtonGang.SetActive((statecode & 4) > 0);
            ButtonReach.SetActive((statecode & 8) > 0);
            ButtonRong.SetActive((statecode & 16) > 0);
            ButtonCancel.SetActive((statecode & 32) > 0);

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}