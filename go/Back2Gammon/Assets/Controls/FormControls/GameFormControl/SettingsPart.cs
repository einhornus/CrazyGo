using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BrainDuelsLib.widgets;
using BrainDuelsLib.web;
using BrainDuelsLib.threads;
using BrainDuelsLib.model.entities;
using UnityEngine;
public partial class GameFormControl : ControlBase
{

    public SettingsComponentScript settings;
    public UIButton settingsButton;

    //public AudioSource popupSound;
    //public AudioSource tricTracSound;
    //public AudioSource moveSound;
    //public AudioSource buttonSound;
    //public AudioSource chatSound;
    //public AudioSource turnSound;


    public AudioSource pressButtonSound;
    public AudioSource chatSound;
    public AudioSource passSound;
    public AudioSource stoneSound;
    public AudioSource illegalMoveSound;

    public void SetSettingsPart()
    {
    }
}
