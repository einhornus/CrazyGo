using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class CrazyGoButton : UnityEngine.MonoBehaviour, BrainDuelsLib.view.ButtonControl
{
    public UIButton button;
    public EventHandler act;
    public void SetOnClick(EventHandler action)
    {
        act = action;
        button.onClick.Add(new EventDelegate(new EventDelegate.Callback(f)));
    }

    public void f()
    {
        act(null, null);
    }
}

