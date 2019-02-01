using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SingleMessage : MonoBehaviour
{
    [SerializeField]
    private Text uiText;

    public void SetText(string txt)
    {
        uiText.text = txt;
    }
}
