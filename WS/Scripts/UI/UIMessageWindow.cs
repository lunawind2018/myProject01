using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace WS
{
    public class UIMessageWindow : MonoBehaviour
    {
        [SerializeField] 
        private ScrollRect scrollRect;

        [SerializeField] 
        private CanvasGroup canvasGroup;

        [SerializeField]
        private SingleMessage messagePrefab;



        private List<SingleMessage> messageList = new List<SingleMessage>();

        void Awake()
        {
            this.state = 0;
            this.timeCount = 0;
            this.canvasGroup.alpha = 0f;
        }

        void Start()
        {
        }

        public void AddText(string ttt)
        {
            var message = Instantiate(messagePrefab);
            message.SetText(ttt);
            messageList.Add(message);
            Utils.SetParent(message.transform, scrollRect.content);

            if (messageList.Count > 5)
            {
                Destroy(messageList[0].gameObject);
                messageList.RemoveAt(0);
            }

            if (this.canvasGroup.alpha < 0.99999999)
            {
                if (this.doTween != null) this.doTween.Kill(false);
                this.canvasGroup.alpha = 1;
            }
            this.timeCount = 0;
            this.state = 1;
            this.added = true;
        }

        private float timeCount = 0f;
        private int state = 0;
        private const float FadeTime = 10f;
        private Tweener doTween;
        private bool added;

        void Update()
        {

            if (this.state!=0)
            {
                this.timeCount += Time.deltaTime;
                if (timeCount > FadeTime)
                {
                    this.state = 0;
                    this.timeCount = 0;
                    this.doTween = this.canvasGroup.DOFade(0, 2);
                }
            }
        }

        void LateUpdate()
        {
            if (added)
            {
                scrollRect.normalizedPosition = new Vector2(0, 0);
                added = false;
            }
        }
    }
}
