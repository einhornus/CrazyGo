using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrainDuelsLib.widgets;
using BrainDuelsLib.web;
using BrainDuelsLib.threads;
using UnityEngine;
using Assets;
using BrainDuelsLib.model.entities;

public partial class LobbyFormControl : ControlBase
{
    public SettingsComponentScript settings;
    public AudioSource musicAudioSource;
    public UIButton settingsButton;

    public AudioSource popupSound;
    public AudioSource buttonSound;
    public AudioSource newGameSound;

    public void SetSetingsPart()
    {
        settingsButton.onClick.Add(new EventDelegate(new EventDelegate.Callback(
            delegate
            {
                buttonSound.Play();
            }
            )));


        settings.changeSoundsVolumeCallback = delegate (double level)
        {
            popupSound.volume = (float)level;
            buttonSound.volume = (float)level;
            newGameSound.volume = (float)level;
        };

        settings.changeMusicVolumeCallback = delegate (double level)
        {
            musicAudioSource.volume = (float)level;
        };


        Utils.MakeUnvisible(settings.transform);

        settingsButton.onClick.Add(new EventDelegate(new EventDelegate.Callback(delegate
        {
            if (Utils.IsUnvisible(settings.transform))
            {
                Utils.MakeVisible(settings.transform);
            }
            else
            {
                Utils.MakeUnvisible(settings.transform);
            }
        })));
    }
}
