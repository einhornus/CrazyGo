using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrainDuelsLib.delegates;
using UnityEngine;

public class CrazyGoTimerScript : ControlBase
{
    private BrainDuelsLib.delegates.Action action = delegate { };
    public bool isWorking = false;
    private long lastMilliseconds = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
    private int interval;

    public void SetAction(BrainDuelsLib.delegates.Action action, int interval)
    {
        this.action = action;
        this.interval = interval;
    }

    public void Start()
    {

    }

    public void Update()
    {
        if (isWorking)
        {
            long milliseconds = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            long diff = milliseconds - lastMilliseconds;

            if (diff >= interval)
            {
                lastMilliseconds = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
                action();
            }
        }
    }

    public void StartTimer()
    {
        isWorking = true;
    }

    public void StopTimer()
    {
        isWorking = false;
    }
}

