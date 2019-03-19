using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace WS
{
    public class UIBottomMenu : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
    {
        [SerializeField] private float starty = 0;
        [SerializeField] private float endy = 70;
        [SerializeField] private float movetime = 0.3f;

        private int state;
        private float showtime;
        private bool isHover;

        private Tweener currTween;
        void Start()
        {
            starty += this.transform.localPosition.y;
            endy += this.transform.localPosition.y;
            state = 0;
            //Show();
        }

        void Update()
        {
            if (this.isHover) return;
            if (this.state > 0)
            {
                this.showtime += Time.deltaTime;
                if (this.showtime > 5) Hide();
            }
        }

        public bool IsShow()
        {
            return this.state > 0;
        }

        public void Switch()
        {
            if (this.state == 0) { Show(); }
            else { Hide(); }
        }

        public void Show()
        {
            if (this.state != 0) return;
            if (currTween != null)
            {
                currTween.Kill();
            }
            currTween = this.transform.DOLocalMoveY(endy, movetime).OnComplete(OnShowComplete);
            this.showtime = 0;
            this.state = 1;
        }

        private void OnShowComplete()
        {
            //            Utils.SetPositionY(this, endy);
            currTween = null;
        }

        public void Hide()
        {
            if (this.state == 0) return;
            if (currTween != null)
            {
                currTween.Kill();
            }
            currTween = this.transform.DOLocalMoveY(starty, movetime).OnComplete(OnHideComplete);
            this.state = 0;
        }

        private void OnHideComplete()
        {
            //            Utils.SetPositionY(this, starty);
            currTween = null;
        }

        public void OnClickButton(string btnName)
        {
            var type = (UIType) Enum.Parse(typeof (UIType), btnName);
            UIManager.OpenUI(type);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            isHover = true;
            showtime = 0;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            isHover = false;
            showtime = 0;
        }
    }
}