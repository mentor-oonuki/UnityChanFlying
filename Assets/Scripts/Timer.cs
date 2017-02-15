using UnityEngine;
using System.Collections;
using System.Diagnostics;
using UnityEngine.UI;
using System;

public class Timer : SingletonMonoBehaviour<Timer>
{
    [SerializeField]
	private Text TimerText;

	private Stopwatch stopwatch;


    private void Start()
    {
        stopwatch = new Stopwatch();
    }

    void Update()
	{
		TimeSpan deltaTime = GetTimer();
        string timerString = deltaTime.Hours.ToString("D2") + ":"
            + deltaTime.Minutes.ToString("D2") + ":"
            + deltaTime.Seconds.ToString("D2");
        TimerText.text = timerString;
	}

	public void TimerStart()
	{
        stopwatch = new Stopwatch();
        stopwatch.Reset();
		stopwatch.Start();
	}

    public void TimerStop()
    {
        if(stopwatch != null)
        {
            stopwatch.Stop();
        }
    }

    public void TimerRestart()
    {

        if(stopwatch != null)
        {
            stopwatch.Start();
        }
    }

	public TimeSpan GetTimer()
	{
        return stopwatch.Elapsed;
    }

}
