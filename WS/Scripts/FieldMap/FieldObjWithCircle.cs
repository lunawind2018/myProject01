using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WS
{
    //field object
    public class FieldObjWithCircle : FieldObject
    {
        public enum CircleState
        {
            None,
            Selected,
            Touched,
            Disabled,
        }

        protected GameObject circleObject;
        protected Image circleImg;

        protected CircleState currState;

        protected override void Awake()
        {
            base.Awake();
            circleObject = transform.Find("Circle").gameObject;
            if (circleObject == null)
            {
                Debug.LogError("===error:no UICircle: " + this.gameObject.name);
                return;
            }
            circleImg = circleObject.GetComponent<Image>();
            circleObject.SetActive(false);
            SetCircleState(CircleState.None);
        }

        public void SetCircleState(CircleState s)
        {
            if (this.currState == s) return;
            this.currState = s;
            this.circleObject.SetActive(currState != CircleState.None);
            switch (s)
            {
                case CircleState.None:
                    break;
                case CircleState.Touched:
                    circleImg.color = Color.gray;
                    break;
                case CircleState.Selected:
                    circleImg.color = Color.yellow;
                    break;
                case CircleState.Disabled:
                    break;
            }
        }

        public virtual string GetDesc()
        {
            return string.Empty;
        }

        public virtual string GetHintName()
        {
            return string.Empty;
        }
    }
}
