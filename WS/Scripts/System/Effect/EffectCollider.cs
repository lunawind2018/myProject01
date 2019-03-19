using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WS
{
    public class EffectCollider : MonoBehaviour
    {
        private List<FieldObject> hitlist;
        public void Init(List<FieldObject> fList)
        {
            hitlist = fList;
        }


        void OnTriggerEnter2D(Collider2D collider)
        {
//            Debug.Log("hit " + collider.gameObject.name);
            var f = collider.gameObject.GetComponent<FieldObject>();
            hitlist.Add(f);
        }

        void OnTriggerExit2D(Collider2D collider)
        {
            var f = collider.gameObject.GetComponent<FieldObject>();
            hitlist.Remove(f);
        }
    }
}