using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrainDuelsLib.widgets;
using BrainDuelsLib.web;
using UnityEngine;
using Assets;
using BrainDuelsLib.model.entities;

using Assets.Go;

public class CreateNewGamePopup : ControlBase
{
    public UIPopupList gameTypePopup;
    public UIPopupList handiPopup;
    public UIPopupList timeControlPopup;
    public UIPopupList boardSizePopup;
    public UIButton create;
    public UILabel handicapLabel;

    public void Start()
    {
        if (gameTypePopup != null)
        {
            gameTypePopup.onChange.Add(new EventDelegate(delegate
            {
                if (GetSelection(gameTypePopup) == 3)
                {
                    Utils.MakeUnvisible(handiPopup.transform);
                    Utils.MakeUnvisible(handicapLabel.transform);
                }
                else
                {
                    if (Utils.IsUnvisible(handiPopup.transform))
                    {
                        Utils.MakeVisible(handiPopup.transform);
                        Utils.MakeVisible(handicapLabel.transform);

                    }
                }
            }));
        }
    }

    public String GetGame()
    {
        String game = "go";
        if (GetSelection(gameTypePopup) == 0)
        {
            game = "go";
        }

        if (GetSelection(gameTypePopup) == 1)
        {
            game = "one-color-go";
        }

        if (GetSelection(gameTypePopup) == 2)
        {
            game = "blind-go";
        }

        if (GetSelection(gameTypePopup) == 3)
        {
            game = "hidden-move-go";
        }
        return game;
    }

    public Settings GetSettings()
    {
        Settings settings = new Settings();

        settings.handi = GetSelection(handiPopup) == 1;

        if (GetSelection(timeControlPopup) == 0)
        {
            settings.time = "b300#10#2";
        }

        if (GetSelection(timeControlPopup) == 1)
        {
            settings.time = "b900#20#4";
        }

        if (GetSelection(timeControlPopup) == 2)
        {
            settings.time = "b1800#30#5";
        }

        if (GetSelection(timeControlPopup) == 3)
        {
            settings.time = "b3600#60#5";
        }


        if (GetSelection(boardSizePopup) == 0)
        {
            settings.size = 19;
        }

        if (GetSelection(boardSizePopup) == 1)
        {
            settings.size = 13;
        }

        if (GetSelection(boardSizePopup) == 2)
        {
            settings.size = 9;
        }

        return settings;
    }

    public static int GetSelection(UIPopupList popup)
    {
        for (int i = 0; i < popup.items.Count; i++)
        {
            if (popup.items[i].Equals(popup.selection))
            {
                return i;
            }
        }
        return -1;
    }
}

