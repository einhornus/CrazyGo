using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrainDuelsLib.widgets;
using BrainDuelsLib.web;
using UnityEngine;
using Assets;

public class DialogBoxOk : ControlBase
{
    public UIButton ok;
    public Action okAction;

    public void Start()
    {
        ok.onClick.Add(new EventDelegate(new EventDelegate.Callback(Ok)));
    }

    void Ok()
    {
        okAction();
    }

}
