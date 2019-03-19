using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MyEvent;

namespace WS
{
    public class FieldObject : MonoBehaviour
    {
        protected string bodyName = "Body";
        protected Text nameTxt;
        protected CircleCollider2D coll;
        protected GameObject body;

        public int world_x = 0;
        public int world_y = 0;

        protected virtual void Awake()
        {
            body = transform.Find(bodyName).gameObject;
            nameTxt = body.GetComponent<Text>();
            coll = body.GetComponent<CircleCollider2D>();
        }

        public void SetName(string t, string color = null)
        {
            this.nameTxt.text = t;
            if (!string.IsNullOrEmpty(color))
            {
                this.nameTxt.color = Utils.GetColor(color);
            }

        }
        public void SetScale(float s = 1)
        {
            body.transform.localScale = new Vector3(s, s, s);
        }

        void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.gameObject.name == FieldPlayer.PLAYER_BODY) MyEventSystem.SendEvent(new MyTriggerEvent(MyTriggerEvent.ON_PLAYER_ENTER, this.gameObject));
        }

        void OnTriggerExit2D(Collider2D collider)
        {
            if (collider.gameObject.name == FieldPlayer.PLAYER_BODY) MyEventSystem.SendEvent(new MyTriggerEvent(MyTriggerEvent.ON_PLAYER_EXIT, this.gameObject));
        }
    }
}