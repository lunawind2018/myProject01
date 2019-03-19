using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ArrowAnimation : MonoBehaviour
{

    private Text animText;

    public bool isPlaying;

    private float timeCount;
    private float interval = 0.1f;
    private int step = 0;
    void Awake()
    {
        animText = this.GetComponent<Text>();
        isPlaying = false;
    }

    private string[] strArr = new string[] { "➪", "➫", "➬" };

    public void Play()
    {
        this.isPlaying = true;
    }

    public void Stop()
    {
        this.isPlaying = false;
    }
	
	// Update is called once per frame
	void Update () {
	    if (isPlaying)
	    {
	        timeCount += Time.deltaTime;
	        if (timeCount > interval)
	        {
	            timeCount -= interval;
	            this.step++;
	            if (step >= strArr.Length) step = 0;
	            this.animText.text = strArr[step];
	        }
	    }
	}
}
