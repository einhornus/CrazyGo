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
    public UILabel nameLabel;
    public UIButton changeRankButton;
    public UISlider changeRankSlider;
    public UIButton confirmButtom;
    public UILabel rankLabel;

    public static int MAX_RANK = 9;
    public static int MIN_RANK = -19;
    private int selRank;

    public double GetValueFromRank(int rank)
    {
        double all = MAX_RANK - MIN_RANK;
        double past = rank - MIN_RANK;
        double res = past / all;
        return res;
    }

    public int GetRankFromValue(double value)
    {
        double res = value * (MAX_RANK - MIN_RANK) + MIN_RANK;
        res = Math.Round(res);
        return (int)res;
    }

    public void SetUserInfo()
    {
        Utils.MakeUnvisible(rankLabel.transform);
        Utils.MakeUnvisible(changeRankSlider.transform);
        Utils.MakeUnvisible(confirmButtom.transform);
        changeRankSlider.numberOfSteps = 29;
        User me = Server.UpdateUser(tai, tai.id);
        nameLabel.text = me.login + "(" + me.GetRankString() + ")";

        changeRankButton.onClick.Add(new EventDelegate(delegate
        {
            buttonSound.Play();
            Utils.MakeVisible(rankLabel.transform);
            Utils.MakeVisible(changeRankSlider.transform);
            Utils.MakeVisible(confirmButtom.transform);

            me = Server.UpdateUser(tai, tai.id);
            int rank = me.rank;

            changeRankSlider.onChange = new List<EventDelegate>();
            changeRankSlider.onChange.Add(new EventDelegate(delegate
            {
                int selectedRank = GetRankFromValue(changeRankSlider.value);
                String rankString = User.GetRankString(selectedRank);
                rankLabel.text = rankString;
                selRank = selectedRank;
            }));

            changeRankSlider.value = (float)GetValueFromRank(rank);

            confirmButtom.onClick = new List<EventDelegate>();

            confirmButtom.onClick.Add(new EventDelegate(delegate
            {
                Server.SetUser(tai, selRank);
                User meNow = Server.UpdateUser(tai, tai.id);
                nameLabel.text = meNow.login + "(" + meNow.GetRankString() + ")";
                Utils.MakeUnvisible(rankLabel.transform);
                Utils.MakeUnvisible(changeRankSlider.transform);
                Utils.MakeUnvisible(confirmButtom.transform);

            }
            ));
        }));
    }
}
