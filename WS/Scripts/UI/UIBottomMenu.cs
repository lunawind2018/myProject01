using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class UIBottomMenu : MonoBehaviour
{
    [SerializeField]
    private float starty = 0;
    [SerializeField]
    private float endy = 70;
    [SerializeField]
    private float movetime = 1;

    private int state;
    void Awake()
    {
        
    }

    void Start()
    {
        starty += this.transform.localPosition.y;
        endy += this.transform.localPosition.y;
        state = 0;
        Show();
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
