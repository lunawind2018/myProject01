using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WS
{
    public class EffectBase : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private EffectCollider effectCollider;

        protected virtual void Awake()
        {
            this.animator.enabled = false;
            effectCollider.Init(hitlist);
        }

        public void Play(float speed = 1f)
        {
            if (speed != 1)
            {
                this.animator.speed = speed;
            }
            this.animator.Play(this.name);
            this.animator.enabled = true;
        }

        public void Play(string anim, float speed = 1f)
        {
            if (speed != 1)
            {
                this.animator.speed = speed;
            }
            this.animator.Play(anim);
            this.animator.enabled = true;
        }

        public void Stop()
        {
            this.animator.enabled = false;
        }

        public void DestroySelf()
        {
            Destroy(this.gameObject);
        }


        private List<FieldObject> hitlist = new List<FieldObject>();
        public List<FieldObject> HitList { get { return hitlist;} }

    }
}