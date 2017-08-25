using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrainDuelsLib.widgets;

using BrainDuelsLib.web;
using UnityEngine;
using Assets;

public class DialogBoxYesNo : ControlBase
{
    public UIButton yes;
    public UIButton no;
    public Action yesAction = delegate () { Debug.Log("YES"); };
    public Action noAction = delegate () { Debug.Log("NO"); };

    public void Start()
    {
        yes.onClick.Add(new EventDelegate(new EventDelegate.Callback(Yes)));
        no.onClick.Add(new EventDelegate(new EventDelegate.Callback(No)));
    }

    void Yes()
    {
        yesAction();
    }

    void No()
    {
        noAction();
    }
}
