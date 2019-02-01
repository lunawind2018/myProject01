using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace WS
{
    public class UIHeader : MonoBehaviour
    {
        [SerializeField]
        private float starty = 0;
        [SerializeField]
        private float endy = -55;
        [SerializeField]
        private float movetime = 0.5f;


        private int state = 0;

        void Awake()
        {
        }

        void Start()
        {
            starty += this.transform.localPosition.y;
            endy += this.transform.localPosition.y;
            state = 0;
        }


        public void Show()
        {
            if (this.state != 0) return;
            this.transform.DOLocalMoveY(endy, movetime).OnComplete(OnShowComplete);
        }

        private void OnShowComplete()
        {
//            Utils.SetPositionY(this, endy);
        }

        public void Hide()
        {
            if (this.state == 0) return;
            this.transform.DOLocalMoveY(starty, movetime).OnComplete(OnHideComplete);
            this.state = 0;
        }

        private void OnHideComplete()
        {
//            Utils.SetPositionY(this, starty);
        }
    }
}
