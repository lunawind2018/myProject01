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
        protected Text circleText;

        protected CircleState currState;

        protected override void Awake()
        {
            base.Awake();
            circleObject = transform.Find("Circle").gameObject;
            if (circleObject == null) Debug.LogError("===error:no UICircle: " + this.gameObject.name);
            circleText = circleObject.GetComponent<Text>();
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
                    circleText.color = Color.gray;
                    break;
                case CircleState.Selected:
                    circleText.color = Color.yellow;
                    break;
                case CircleState.Disabled:
                    break;
            }
        }
    }
}
