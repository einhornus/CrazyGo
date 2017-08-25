using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BrainDuelsLib.widgets;
using BrainDuelsLib.web;
using BrainDuelsLib.web.socket_requests;

using BrainDuelsLib.threads;
using BrainDuelsLib.model.entities;
using BrainDuelsLib;
using UnityEngine;


public partial class GameFormControl : ControlBase
{
    private GameWidget gameWidget;
    public CrazyGoTimerScript timerScript;
    private Game game;
    private LobbyWidget lw;
    public CrazyGoTimerScript lobbyTimer;
    public Server.TokenAndId tai;

    private IBoardScript board;//TODO
    public BoardScript b;//TODO


    public UIButton yesResignButton;
    public UIButton noResignButton;
    public UISprite resignPopup;
    public UIButton exitButton;

    public UIButton moveButton;
    public UIButton chatButton;

    public UIButton passButton;
    public UIButton agreeButton;
    public UIButton goButton;

    public UIButton askButton;

    public void InitGameWidgetPart()
    {
        board = b;
        if (!isTest)
        {
            Server.TokenAndId tai = (Server.TokenAndId)DataPasser.Get().Get("tai");
            Game game = (Game)DataPasser.Get().Get("game");
            Boolean isObserver = (Boolean)DataPasser.Get().Get("is_observer");
            SetGameWidgetPart(tai, game);
        }
        else
        {
            Server.TokenAndId tai = Server.Authorize("!!FB!!103018196855905", "uskjlrvkovscfzzwbtsr");
            lw = new LobbyWidget(tai);
            LobbyBackendWidget backgroud = new LobbyBackendWidget(tai);
            backgroud.Controls.timer = new CrazyGoTimerControl(1000, lobbyTimer);
            lw.Controls.backgroud = backgroud;
            lw.Callbacks.goToGameAsPlayerCallback = delegate (Game g)
            {
                lw.Discard();
                Debug.Log("Go to game as player");
                SetGameWidgetPart(tai, g);
            };
            lw.Go();
            lw.Controls.backgroud.OpenForRandomChallenge("hidden-move-go", "board_size-19|komi-7.5|first_player-0|randomize-1|hm_count-7|time-b3600#60#5");
        }
    }


    public void SetGameWidgetPart(Server.TokenAndId tai, Game game)
    {
        this.tai = tai;
        this.game = game;
        gameWidget = new GameWidget(tai, game);
        CrazyGoTimerControl timerControl = new CrazyGoTimerControl(250, timerScript);
        gameWidget.Controls.timer = timerControl;

        gameWidget.Callbacks.updateGameCallback = delegate (string s, int a)
        {
            UpdateGameCallback(s, a);
        };


        gameWidget.Callbacks.eventCallback = delegate (string message)
        {
            OnEvent(message);
        };


        gameWidget.Callbacks.illegalMoveCallback = delegate
        {
            UnityEngine.Debug.Log("Illegal move");
        };


        gameWidget.Callbacks.gameEndedCallback = delegate (string res)
        {
            this.OnGameEnd(res);

        };

        gameWidget.Callbacks.gameEndedCallback = delegate (string s)
        {
            OnGameEnd(s);
            UnityEngine.Debug.Log("Game ended: result is " + s);
        };

        gameWidget.Callbacks.eventCallback = delegate (string s)
        {
            OnEvent(s);
            //UnityEngine.Debug.Log("Event: " + s);
        };


        moveButton.onClick.Add(new EventDelegate(new EventDelegate.Callback(
        delegate
        {
            //pressButtonSound.Play();
            Point a = board.GetCurrentMoveIndicator();
            if (a != null)
            {
                if (a.x >= 0 && a.x < board.GetN())
                {
                    if (a.y >= 0 && a.y < board.GetN())
                    {
                        gameWidget.MakeAMove(a.x + "-" + a.y);
                    }
                }
            }
        }
        )));

        resignButton.onClick.Add(new EventDelegate(new EventDelegate.Callback(
            delegate
            {
                pressButtonSound.Play();
                Utils.MakeVisible(resignPopup.transform);
            }
        )));

        yesResignButton.onClick.Add(new EventDelegate(new EventDelegate.Callback(
            delegate
            {
                pressButtonSound.Play();
                Utils.MakeUnvisible(resignPopup.transform);
                gameWidget.MakeAMove("resign");
            }
        )));


        this.yesTry.onClick.Add(new EventDelegate(new EventDelegate.Callback(
            delegate
            {
                pressButtonSound.Play();
                Utils.MakeUnvisible(this.tryPopup.transform);
                gameWidget.MakeAMove("yes");
            }
        )));

        this.noTry.onClick.Add(new EventDelegate(new EventDelegate.Callback(
        delegate
        {
            pressButtonSound.Play();
            Utils.MakeUnvisible(this.tryPopup.transform);
            gameWidget.MakeAMove("no");
        }
        )));


        this.passButton.onClick.Add(new EventDelegate(new EventDelegate.Callback(
            delegate
            {
                pressButtonSound.Play();
                Utils.MakeUnvisible(resignPopup.transform);
                gameWidget.MakeAMove("pass");
            }
        )));

        this.agreeButton.onClick.Add(new EventDelegate(new EventDelegate.Callback(
            delegate
            {
                pressButtonSound.Play();
                Utils.MakeUnvisible(resignPopup.transform);
                gameWidget.MakeAMove("agree");
            }
        )));

        this.askButton.onClick.Add(new EventDelegate(new EventDelegate.Callback(
            delegate
            {
                pressButtonSound.Play();
                Utils.MakeUnvisible(resignPopup.transform);
                gameWidget.MakeAMove("ask");
            }
        )));

        this.goButton.onClick.Add(new EventDelegate(new EventDelegate.Callback(
            delegate
            {
                gameWidget.MakeAMove("go");
            }
        )));

        noResignButton.onClick.Add(new EventDelegate(new EventDelegate.Callback(
            delegate
            {
                pressButtonSound.Play();
                Utils.MakeUnvisible(resignPopup.transform);
                //gameWidget.MakeAMove("resign");
            }
        )));


        exitButton.onClick.Add(new EventDelegate(new EventDelegate.Callback(
            delegate
            {
                GoToLobby();
            }
        )));

        this.board.SetUserTogglePoint(delegate (Point a)
        {
            if (a.x >= 0 && a.x < this.board.GetN())
            {
                if (a.y >= 0 && a.y < this.board.GetN())
                {
                    gameWidget.MakeAMove(a.x + "-" + a.y);
                }
            }
        });

        gameWidget.Go();
        SetInitialData();
        StartChat();
    }

    void OnApplicationQuit()
    {
        DBSocketRequest.Close();

        UnityEngine.Debug.Log("Game quit");
        StopChat();

        if (lw != null)
        {
            UnityEngine.Debug.Log("stop toy lobby");
            lw.Stop();
        }

        if (gameWidget != null)
        {
            UnityEngine.Debug.Log("stop game");
            gameWidget.Stop();
        }

        if (!isTest)
        {
            LobbyWidget lobby = (LobbyWidget)DataPasser.Get().Get("lobby");
            if (lobby != null)
            {
                UnityEngine.Debug.Log("stop actual lobby");
                lobby.Stop();
            }
        }
    }
}
