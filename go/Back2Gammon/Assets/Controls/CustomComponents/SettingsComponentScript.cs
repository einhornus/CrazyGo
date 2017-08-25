using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrainDuelsLib.widgets;
using BrainDuelsLib.web;
using UnityEngine;
using Assets;

public class SettingsComponentScript : ControlBase
{
    public UISlider musicVolumeSlider;
    public UISlider soundsVolumeSlider;

    public Action<double> changeMusicVolumeCallback = delegate (double a) { };
    public Action<double> changeSoundsVolumeCallback = delegate (double a) { };


    public static double musicVolume = 1.0;
    public static double soundsVolume = 1.0;

    void Start()
    {
        musicVolumeSlider.value = 0f;
        soundsVolumeSlider.value = 1.0f;
        musicVolumeSlider.onChange.Add(new EventDelegate(new EventDelegate.Callback(MusicChange)));
        soundsVolumeSlider.onChange.Add(new EventDelegate(new EventDelegate.Callback(SoundChange)));
    }

    public void MusicChange()
    {
        double newVolume = musicVolumeSlider.value;
        changeMusicVolumeCallback(newVolume);
    }

    public void SoundChange()
    {
        double newVolume = soundsVolumeSlider.value;
        changeSoundsVolumeCallback(newVolume);
    }
}