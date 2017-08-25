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
using BrainDuelsLib.web.socket_requests;
using System.Diagnostics;
using System.Runtime.InteropServices;


public partial class LobbyFormControl : ControlBase
{
    public LobbyWidget lobbyWidget;
    public CrazyGoTimerScript timerScriptLobby;

    public Server.TokenAndId tai;
    public Camera camera;
    public LobbyFormControl moi;
    public UIButton logout;

    void Set(Server.TokenAndId tai)
    {
        


        moi = this;
        lobbyWidget = new LobbyWidget(tai);
        LobbyBackendWidget backgroud = new LobbyBackendWidget(tai);
        backgroud.Controls.timer = new CrazyGoTimerControl(1000, timerScriptLobby);
        lobbyWidget.Controls.backgroud = backgroud;
        lobbyWidget.Callbacks.goToGameAsPlayerCallback = delegate (Game g)
        {
            lobbyWidget.Discard();
            newGameSound.Play();
            UnityEngine.Debug.Log("Go to game");


            DataPasser.Get().Set("lobby", lobbyWidget);
            DataPasser.Get().Set("tai", tai);
            DataPasser.Get().Set("is_observer", false);
            DataPasser.Get().Set("game", g);
            CloseWidget();
            Application.LoadLevel(3);

        };

        lobbyWidget.Callbacks.repeatedCallback = delegate ()
        {
            lobbyWidget.Discard();
            UnityEngine.Debug.Log("Already connected");
            CloseWidget();
            DataPasser.Get().Set("repeat", "true");
            Application.LoadLevel(0);
        };


        SetUserInfo();
        SetPlayBarPart(lobbyWidget);
        lobbyWidget.Go();

    }

    public void Deliver()
    {
    }


    void Start()
    {
        DataPasser.Get().Set("repeat", "false");
        logout.onClick.Add(new EventDelegate(new EventDelegate.Callback(
        delegate
        {
            buttonSound.Play();
            CloseWidget();
            Application.LoadLevel(0);
        }
        )));



        Deliver();
        if (DataPasser.Get().dic.ContainsKey("tai"))
        {
            tai = (Server.TokenAndId)DataPasser.Get().Get("tai");
        }
        else
        {
            tai = Server.Authorize("einhorn", "795132486");
        }

        SetSetingsPart();

        UnityEngine.Debug.Log("Start lobby");
        Set(tai);
    }

    public void CloseWidget()
    {
        lobbyWidget.Stop();
        UnityEngine.Debug.Log("Lobby quit");
    }

    public void OnApplicationQuit()
    {
        DBSocketRequest.Close();
        CloseWidget();
    }
}
