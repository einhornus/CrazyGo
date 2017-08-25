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
using Assets.Go;

public partial class LobbyFormControl : ControlBase
{
    public GameScrollableList gamesList;
    public PlayerScrollableList playersList;

    public CreateNewGamePopup createGamePopup;
    public UIButton createGameButton;
    public UISprite gamesListSprite;
    public UISprite playerListSprite;
    public UILabel createGamePopupLabel;
    public UIButton createGamePopupCancelButton;

    public UISprite newChallengePopup;
    public UILabel newChallengeFromLabel;
    public UILabel newChallengeGameString;
    public UILabel newChallengeTimeString;
    public UILabel newChallengeSizeString;
    public UILabel newChallengeHandiString;
    public UIButton acceptChallengeButton;
    public UIButton rejectChallengeButton;

    public class IncomingChallenge
    {
        public int ch;
        public string title;
        public string settings;
        public int id;
    }

    public List<IncomingChallenge> incomingChallenges = new List<IncomingChallenge>();

    List<Game> prev = new List<Game>();

    public void callCreateGamePopup(bool isPrivate, Action action)
    {
        if (isPrivate)
        {
            createGamePopupLabel.text = "Create private challenge";
        }
        else
        {
            createGamePopupLabel.text = "Create public challenge";
        }

        if (Utils.IsUnvisible(createGamePopup.transform))
        {
            Utils.MakeVisible(createGamePopup.transform);
        }
        else
        {
            Utils.MakeUnvisible(createGamePopup.transform);
        }


        createGamePopup.create.isEnabled = true;
        createGamePopup.create.onClick.Clear();
        createGamePopup.create.onClick.Add(new EventDelegate(new EventDelegate.Callback(
        delegate
        {
            action();
        }
        )));

        createGamePopupCancelButton.onClick.Clear();
        createGamePopupCancelButton.onClick.Add(new EventDelegate(new EventDelegate.Callback(
        delegate
        {
            if (Utils.IsUnvisible(createGamePopup.transform))
            {
                Utils.MakeVisible(createGamePopup.transform);
            }
            else
            {
                Utils.MakeUnvisible(createGamePopup.transform);
            }
        }
        )));

    }



    public void SetPlayBarPart(LobbyWidget lobbyWidget)
    {
        this.gamesListSprite.spriteName = "nicht";
        this.playerListSprite.spriteName = "nicht";

        GameScrollableList.me = tai.id;
        this.gamesList.transform.localPosition = new Vector3(0, 0, 0);
        this.playersList.transform.localPosition = new Vector3(0, -10, 0);
        this.createGamePopup.transform.localPosition = new Vector3(this.createGamePopup.transform.localPosition.x, 0, 0);
        this.newChallengePopup.transform.localPosition = new Vector3(this.newChallengePopup.transform.localPosition.x, 0, 0);

        lobbyWidget.Callbacks.gamesCallback = UpdateGames;
        lobbyWidget.Callbacks.getPlayersCallback = UpdatePlayers;
        //setMock();


        Utils.MakeUnvisible(createGamePopup.transform);
        Utils.MakeUnvisible(this.newChallengePopup.transform);


        createGameButton.onClick.Add(new EventDelegate(new EventDelegate.Callback(
        delegate
        {
            callCreateGamePopup(false, delegate
            {
                Settings settings = createGamePopup.GetSettings();
                String title = createGamePopup.GetGame();
                if (title.Equals("hidden-move-go"))
                {
                    settings.handi = false;
                }
                this.lobbyWidget.CreateNewGame(2, title, Settings.Save(settings));

                
                if (!Utils.IsUnvisible(createGamePopup.transform))
                {
                    Utils.MakeUnvisible(createGamePopup.transform);
                }
                
            });
        }
        )));

        lobbyWidget.Callbacks.rejectedChallengeCallback = delegate (int a, string aa, string aaa, int aaaa)
        {
            if (Utils.IsUnvisible(createGamePopup.transform))
            {
                //Utils.MakeVisible(createGamePopup.transform);
            }
            else
            {
                Utils.MakeUnvisible(createGamePopup.transform);
            }
        };



        lobbyWidget.Callbacks.newChallengeCallback = delegate (int a, string aa, string aaa, int aaaa)
        {
            for (int i = 0; i < this.incomingChallenges.Count(); i++)
            {
                lobbyWidget.RejectChallenge(incomingChallenges[i].ch, incomingChallenges[i].title, incomingChallenges[i].settings, incomingChallenges[i].id);
            }
            incomingChallenges.Clear();

            IncomingChallenge me = new IncomingChallenge();
            int id = a;
            string title = aa;
            string settings = aaa;
            int chId = aaaa;
            me.ch = id;
            me.title = title;
            me.settings = settings;
            me.id = chId;
            incomingChallenges.Add(me);

            if (Utils.IsUnvisible(newChallengePopup.transform))
            {
                Utils.MakeVisible(newChallengePopup.transform);
            }

            User opponent = Server.GetUser(tai, id);
            Settings set = Settings.Load(settings);
            newChallengeFromLabel.text = opponent.login + "(" + opponent.GetRankString() + ")";
            if (title.Equals("go"))
            {
                this.newChallengeGameString.text = "Go";
            }

            if (title.Equals("one-color-go"))
            {
                this.newChallengeGameString.text = "One color Go";
            }

            if (title.Equals("blind-go"))
            {
                this.newChallengeGameString.text = "Blind Go";
            }

            if (title.Equals("hidden-move-go"))
            {
                this.newChallengeGameString.text = "Hidden move Go";
            }

            this.newChallengeTimeString.text = set.GetTimeControl();
            this.newChallengeHandiString.text = set.GetHandi() ? "Yes" : "No";
            this.newChallengeSizeString.text = set.size + "*" + set.size;

            acceptChallengeButton.onClick.Clear();
            acceptChallengeButton.onClick.Add(new EventDelegate(new EventDelegate.Callback(
            delegate
            {
                lobbyWidget.AcceptChallenge(a, aa, aaa, aaaa);
            }
            )));

            rejectChallengeButton.onClick.Clear();
            this.rejectChallengeButton.onClick.Add(new EventDelegate(new EventDelegate.Callback(
            delegate
            {
                lobbyWidget.RejectChallenge(a, aa, aaa, aaaa);
                incomingChallenges.Clear();

                if (Utils.IsUnvisible(newChallengePopup.transform))
                {
                    Utils.MakeVisible(newChallengePopup.transform);
                }
                else
                {
                    Utils.MakeUnvisible(newChallengePopup.transform);
                }
            }
            )));
        };


        lobbyWidget.Callbacks.expiredChallengeCallback = delegate (int chId)
        {
            Debug.Log("Expired " + chId);
            if (incomingChallenges.Count > 0)
            {
                int last = incomingChallenges[incomingChallenges.Count - 1].id;
                Debug.Log("Last " + last);
                if (last == chId)
                {
                    Debug.Log("Delete " + chId);
                    if (!Utils.IsUnvisible(newChallengePopup.transform))
                    {
                        Debug.Log("deleted");
                        Utils.MakeUnvisible(newChallengePopup.transform);
                    }
                }
            }
        };

    }



    public List<int> prevPlayers = new List<int>();
    public void UpdatePlayers(List<int> players)
    {
        if (compareLists(players, prevPlayers))
        {
        }
        else
        {
            List<User> fuck = new List<User>();
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i] != tai.id)
                {
                    fuck.Add(Server.GetUser(tai, players[i]));
                }
            }
            playersList.Set(fuck, -30, -150, 82, OnChallenge, tai);
            prevPlayers = new List<int>();

            for (int i = 0; i < players.Count; i++)
            {
                prevPlayers.Add(players[i]);
            }
        }
    }

    public void UpdateGames(List<Game> games)
    {

        if (compareLists(games, prev))
        {

        }
        else
        {
            gamesList.Set(games, 50, -150, 82, OnGame, tai);
        }

        prev = games;
    }

    public void setMock()
    {
        List<Game> list = new List<Game>();
        Game game1 = new Game();
        game1.settings = "board_size-19|komi-7.5|first_player-0|time-b10#15#3";
        game1.status = "open";
        game1.title = "one-color-go";
        game1.users = new List<int>();
        game1.users.Add(2501);


        Game game2 = new Game();
        game2.settings = "board_size-9|komi-0.5|first_player-1|time-b100#15#3";
        game2.status = "open";
        game2.title = "go";
        game2.users = new List<int>();
        game2.users.Add(2501);

        Game game3 = new Game();
        game3.settings = "board_size-13|komi-7.5|first_player-1|time-b10#45#3";
        game3.status = "open";
        game3.title = "blind-go";
        game3.users = new List<int>();
        game3.users.Add(2501);


        Game game4 = new Game();
        game4.settings = "board_size-19|komi-7.5|first_player-9|time-b1800#30#5";
        game4.status = "open";
        game4.title = "hidden-move-go";
        game4.users = new List<int>();
        game4.users.Add(2501);

        for (int i = 0; i < 10; i++)
        {
            list.Add(game1);
            list.Add(game2);
            list.Add(game3);
            list.Add(game4);
        }

        gamesList.Set(list, 0, -150, 66, OnGame, tai);
    }

    public bool compareLists<U>(List<U> a, List<U> b)
    {
        if (a.Count != b.Count)
        {
            return false;
        }
        for (int i = 0; i < a.Count; i++)
        {
            if (!a[i].Equals(b[i]))
            {
                return false;
            }
        }
        return true;
    }

    public bool compareLists(List<Game> a, List<Game> b)
    {
        if (a.Count != b.Count)
        {
            return false;
        }
        for (int i = 0; i < a.Count; i++)
        {
            if (a[i].id != b[i].id)
            {
                return false;
            }
            else
            {
                if (a[i].status != b[i].status)
                {
                    return false;
                }
                if (a[i].users.Count != b[i].users.Count)
                {
                    return false;
                }
                else
                {
                    for (int j = 0; j < a[i].users.Count; j++)
                    {
                        if (a[i].users[j] != b[i].users[j])
                        {
                            return false;
                        }
                    }
                }
            }
        }
        return true;
    }

    public void OnGame(Game g, int index)
    {
        if (index == 0)
        {
            lobbyWidget.Controls.backgroud.EnterGame(g.id);
        }
        if (index == 1)
        {
            lobbyWidget.Controls.backgroud.LeaveGame(g.id);
        }
    }

    public void OnChallenge(User g, int index)
    {
        callCreateGamePopup(true, delegate
        {
            Settings settings = createGamePopup.GetSettings();
            String title = createGamePopup.GetGame();
            lobbyWidget.CreateNewChallenge(index, title, Settings.Save(settings));
            createGamePopup.create.isEnabled = false;
            createGamePopup.create.onClick.Clear();
            Debug.Log("create");



            this.createGamePopupCancelButton.onClick.Clear();
            this.createGamePopupCancelButton.onClick.Add(new EventDelegate(new EventDelegate.Callback(
            delegate
            {
                Debug.Log("discard");
                lobbyWidget.Discard();
                if (Utils.IsUnvisible(createGamePopup.transform))
                {
                    Utils.MakeVisible(createGamePopup.transform);
                }
                else
                {
                    Utils.MakeUnvisible(createGamePopup.transform);
                }
            }
            )));
        });
    }
}
