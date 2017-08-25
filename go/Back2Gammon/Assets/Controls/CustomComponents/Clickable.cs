using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrainDuelsLib.widgets;
using BrainDuelsLib.web;
using UnityEngine;
using Assets;

public class Clickable : ControlBase
{
    public Action<int> press = new Action<int>(delegate { });
    public int index;
    public long lastPress = 0;

    public static long timeDelta = 500;

    void OnPress(bool isPressed)
    {
        long currentTime = Utils.CurrentTimeMillis();
        long diff = currentTime - lastPress;
        if (diff > timeDelta)
        {
            press(index);
            lastPress = currentTime;
        }
    }
}
