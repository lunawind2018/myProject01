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


        protected virtual void Awake()
        {
            body = transform.Find(bodyName).gameObject;
            nameTxt = body.GetComponent<Text>();
            coll = body.GetComponent<CircleCollider2D>();
        }

        public void SetRotation(Quaternion r)
        {
            body.transform.localRotation = r;
        }

        public void SetRotation(Vector3 a)
        {
            body.transform.eulerAngles = a;
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