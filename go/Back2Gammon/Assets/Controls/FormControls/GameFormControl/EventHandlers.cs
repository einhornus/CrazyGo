using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using BrainDuelsLib.widgets;
using BrainDuelsLib.web;
using BrainDuelsLib.threads;
using BrainDuelsLib.model.entities;
using UnityEngine;
using Assets.Go;


public partial class GameFormControl : ControlBase
{
    public UIButton resignButton;

    public UILabel firstName;
    public UILabel secondName;
    public UISprite currentPlayerHightlight;

    public UILabel handicapLabel;
    public Settings gameSettings;

    public UILabel phaseLabel;

    public UISprite[] firstPlayerTries;
    public UISprite[] secondPlayerTries;
    public UISprite tryPopup;
    public UIButton yesTry;
    public UIButton noTry;
    public UISprite firstPlayerColor;
    public UISprite secondPlayerColor;
    public UILabel firstPlayerKomi;
    public UILabel secondPlayerKomi;


    public void SetInitialData()
    {
        User user1 = Server.GetUser(tai, game.users[0]);
        User user2 = Server.GetUser(tai, game.users[1]);
        firstName.text = user1.login + "(" + user1.GetRankString() + ")";
        secondName.text = user2.login + "(" + user2.GetRankString() + ")";
        gameSettings = Settings.Load(game.settings);
    }

    public void SetAvas()
    {

    }

    public void UpdateGameCallback(string s, int index)
    {
        ResponseBase response = BoardStateParser.Parse(s, index);
        UpdateAfterState(response, index);
    }

    public void OnEvent(string s)
    {
        string[] parts = s.Split('|');
        string signal = parts[0];
        string what = parts[1];

        if (signal.Equals("phase"))
        {
            //phaseLabel.text = what;
        }

        if (signal.Equals("pass"))
        {
            this.passSound.Play();
        }

        if (signal.Equals("move"))
        {
            this.stoneSound.Play();
        }

        if (signal.Equals("reveal"))
        {
            this.askButton.isEnabled = false;
        }

        if (signal.Equals("time"))
        {
            if (what[0] == 'b')
            {
                string[] ps = what.Split('#');
                int firstMainTime = int.Parse(ps[0].Substring(1));
                int firstOvertime = int.Parse(ps[1]);
                int firstPeriods = int.Parse(ps[2]);
                int secondMainTime = int.Parse(ps[3].Substring(1));
                int secondOvertime = int.Parse(ps[4]);
                int secondPeriods = int.Parse(ps[5]);

                string firstString = GetByoyomiString(firstMainTime, firstOvertime, gameSettings.getOvertime(), firstPeriods);
                string secondString = GetByoyomiString(secondMainTime, secondOvertime, gameSettings.getOvertime(), secondPeriods);
                this.firstPlayerTime.text = firstString;
                this.secondPlayerTime.text = secondString;
            }

            if (what[0] == 'a')
            {
                string[] ps = what.Split('#');
                int firstMainTime = int.Parse(ps[0].Substring(1));
                int secondMainTime = int.Parse(ps[1].Substring(1));
                string firstString = GetAbsoluteString(firstMainTime);
                string secondString = GetAbsoluteString(secondMainTime);
                this.firstPlayerTime.text = firstString;
                this.secondPlayerTime.text = secondString;
            }
        }

        if (signal.Equals("ask"))
        {
            int to = int.Parse(what[1] + "");
            if (game.users[to] == tai.id)
            {
                if (Utils.IsUnvisible(tryPopup.transform))
                {
                    Utils.MakeVisible(tryPopup.transform);
                }
            }
        }
    }
}