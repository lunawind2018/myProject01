using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WS
{
    public class MySlider : Slider
    {
        [SerializeField] private float min = 0.001f;

        public override float value {
            set
            {
                base.value = value;
                var b = value > min;
                if (this.fillRect.gameObject.activeSelf != b)
                {
                    this.fillRect.gameObject.SetActive(b);
                }
            }
        }
    }
}