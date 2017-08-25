using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrainDuelsLib.view;
using BrainDuelsLib.delegates;
using System.Timers;


public class CrazyGoTimerControl : TimerControl
{
    public CrazyGoTimerScript script;
    public CrazyGoTimerControl(int interval, CrazyGoTimerScript script) : base(interval)
    {
        this.script = script;
    }

    public override void Start(BrainDuelsLib.delegates.Action onTime)
    {
        script.SetAction(onTime, interval);
        script.isWorking = true;
    }

    public override void Stop()
    {
        script.isWorking = false;
    }
}

