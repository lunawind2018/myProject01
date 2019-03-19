using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_ListIconBase : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,IPointerClickHandler
{

    [SerializeField] private GameObject highLight;

    [SerializeField] private GameObject selectObj;

    private bool isHover = false;

    void Awake()
    {
        SetHighLight(false);
        SetSelect(false);
    }
    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        isHover = true;
        SetHighLight(isHover);
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        isHover = false;
        SetHighLight(isHover);
    }

    public void SetHighLight(bool v)
    {
        if (highLight != null)
        {
            highLight.SetActive(v);
        }
    }

    public void SetSelect(bool v)
    {
        if (v)
        {
            SetHighLight(false);
        }
        else
        {
            if (isHover) SetHighLight(true);
        }
        if (selectObj != null)
        {
            selectObj.SetActive(v);
        }
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }
}
