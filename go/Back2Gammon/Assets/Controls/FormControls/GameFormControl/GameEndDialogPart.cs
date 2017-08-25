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
    public UILabel blackPoints;
    public UILabel whitePoints;
    public UILabel resultString;
    public DialogBoxOk gameEndDialog;

    public void OnGameEnd(string result)
    {
        char cause = result[0];

        string[] parts = result.Substring(1).Split('#');
        double firstPoints = double.Parse(parts[1]);
        double secondPoints = double.Parse(parts[2]);

        double white = firstPoints;
        double black = secondPoints;

        //TODO

        if (this.black == 1)
        {
            white = firstPoints;
            black = secondPoints;
        }
        else
        {
            white = secondPoints;
            black = firstPoints;
        }

        string str = "";
        if (black > white)
        {
            if (cause == 'p')
            {
                str = "Black won by " + (black - white);
            }
            if (cause == 't')
            {
                str = "Black won by timeout";
            }
            if (cause == 'r')
            {
                str = "Black won by resignation";
            }
        }
        else
        {
            if (cause == 'p')
            {
                str = "White won by " + (white - black);
            }
            if (cause == 't')
            {
                str = "White won by timeout";
            }
            if (cause == 'r')
            {
                str = "White won by resignation";
            }
        }

        blackPoints.text = black + "";
        whitePoints.text = white + "";
        resultString.text = str;

        Utils.MakeVisible(this.gameEndDialog.transform);
        gameEndDialog.okAction = delegate
        {
            pressButtonSound.Play();
            GoToLobby();
        };
    }

    public void SetOnGameEndDialog()
    {
        Utils.MakeUnvisible(this.gameEndDialog.transform);
    }

    public void GoToLobby()
    {
        DataPasser.Get().Set("tai", tai);

        LobbyWidget lobby = (LobbyWidget)DataPasser.Get().Get("lobby");
        if (lobby != null)
        {
            UnityEngine.Debug.Log("stop lobby");
            lobby.Stop();
        }

        if (gameWidget != null)
        {
            gameWidget.Stop();
            UnityEngine.Debug.Log("stop game widget");
        }

        StopChat();

        Application.LoadLevel(2);
    }
}
