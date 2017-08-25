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

public class PlayerScrollableList : ScrollableList<User>
{
    public static int me;
    public override void SetItem(GameObject widget, User item, Action<User, int> action, Server.TokenAndId tai)
    {
        PlayerListItem litem = widget.GetComponent<PlayerListItem>();
        litem.login.text = item.login + "(" + item.GetRankString() + ")";
        litem.challengeButton.onClick.Add(new EventDelegate(new EventDelegate.Callback(
            delegate
            {
                action(item, item.id);
            }
            )));
    }
}

