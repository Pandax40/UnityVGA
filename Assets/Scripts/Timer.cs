using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public float timer;
    [SerializeField] private  TMP_Text timertext;
    private int seconds, cents;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;

            seconds = (int)(timer);
            cents = (int)((timer - (int)(timer))*100f);

           timertext.text = string.Format("{0:00}:{1:00}", seconds, cents);
        }
        else
        {
            timertext.text = "00:00";
        }
    }
}
