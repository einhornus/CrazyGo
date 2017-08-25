using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrainDuelsLib.model.entities;
using UnityEngine;
using BrainDuelsLib.threads;
using Assets.Go;
using Assets;
using BrainDuelsLib.web;

public class GameScrollableList : ScrollableList<Game>
{
    public static int me;
    public override void SetItem(GameObject widget, Game item, Action<Game, int> action, Server.TokenAndId tai)
    {
        GameListItem litem = widget.GetComponent<GameListItem>();
        if (item.users.Count >= 1)
        {
            User u = Server.GetUser(tai, item.users[0]);
            litem.firstPlayerName.text = u.login + "(" + u.GetRankString() + ")";
        }
        else
        {
            litem.firstPlayerName.text = "Available";
        }

        if (item.users.Count >= 2)
        {
            User u = Server.GetUser(tai, item.users[1]);
            litem.secondPlayerName.text = u.login + "(" + u.GetRankString() + ")";
        }
        else
        {
            litem.secondPlayerName.text = "Available";
        }

        Settings settings = Settings.Load(item.settings);


        litem.timeControlLabel.text = settings.GetTimeControl();
        /*
        litem.komiLabel.text = settings.komi+"";

        Stone stone1 = Stone.BLACK;
        Stone stone2 = Stone.WHITE;

        if(settings.firstPlayer == 0){
            stone1 = Stone.BLACK;
            stone2 = Stone.WHITE;
        }

        if (settings.firstPlayer == 1)
        {
            stone1 = Stone.WHITE;
            stone2 = Stone.BLACK;
        }

        if (settings.firstPlayer == 9)
        {
            stone1 = Stone.UNKNOWN;
            stone2 = Stone.UNKNOWN;
        }
         * */

        //litem.firstPlayerStoneSprite.spriteName = BoardStateParser.GetSprite(stone1);
        //litem.secondPlayerStoneSprite.spriteName = BoardStateParser.GetSprite(stone2);

        litem.joinButton.onClick.Add(new EventDelegate(new EventDelegate.Callback(
            delegate
            {
                action(item, 0);
            }
            )));

        litem.leaveButton.onClick.Add(new EventDelegate(new EventDelegate.Callback(
            delegate
            {
                action(item, 1);
            }
            )));

        litem.backgroundSprite.spriteName = getGameBackground(item.status);

        litem.joinButton.isEnabled = false;
        litem.leaveButton.isEnabled = false;

        if (item.status.Equals("open"))
        {
            if (!item.users.Contains(me))
            {
                litem.joinButton.isEnabled = true;
            }
            else
            {
                litem.leaveButton.isEnabled = true;
            }
        }
        else
        {
            litem.joinButton.isEnabled = true;
            if (!item.users.Contains(me))
            {
                litem.joinLabel.text = "Observe";
            }
            else
            {
                litem.joinLabel.text = "Join";
            }
        }

        if (item.title.Equals("go"))
        {
            litem.gameTypeLabel.text = "Go";
        }

        if (item.title.Equals("one-color-go"))
        {
            litem.gameTypeLabel.text = "One color Go";
        }

        if (item.title.Equals("blind-go"))
        {
            litem.gameTypeLabel.text = "Blind Go";
        }

        if (item.title.Equals("hidden-move-go"))
        {
            litem.gameTypeLabel.text = "Hidden move Go";
        }

        int size = settings.size;
        litem.gameTypeLabel.text += "\n" + size + "*" + size;

        if (settings.GetHandi())
        {
            litem.handicapLabel.text = "Yes";
        }
        else
        {
            litem.handicapLabel.text = "No";
        }
    }

    public string getGameBackground(string status)
    {
        if (status.Equals("open"))
        {
            return "green";
        }

        if (status.Equals("closed"))
        {
            return "dark_green";
        }
        throw new ArgumentException();
    }
}

