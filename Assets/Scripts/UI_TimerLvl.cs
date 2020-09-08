using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_TimerLvl : MonoBehaviour
{

    public float timerStart;
    public Text textBox;

    void Start()
    {
        System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
        textBox.text = timerStart.ToString("F2");
    }

    void Update()
    {
        timerStart += Time.deltaTime;
        textBox.text = timerStart.ToString("F2");
    }
}
