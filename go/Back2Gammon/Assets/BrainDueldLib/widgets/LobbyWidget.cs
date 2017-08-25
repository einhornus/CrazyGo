
using System.Collections.Generic;
using System;

using System.Text;

using BrainDuelsLib.view;
using BrainDuelsLib.view.forms;

using BrainDuelsLib.web;
using BrainDuelsLib.web.exceptions;

using BrainDuelsLib.model.entities;

namespace BrainDuelsLib.widgets
{
    public class LobbyWidget : Widget, IStoppable
    {
        private Server.TokenAndId tai;
        private bool isChallenging = false;


        private LobbyWidgetCallbackStore store = new LobbyWidgetCallbackStore();
        public LobbyWidgetCallbackStore Callbacks
        {
            get
            {
                return store;
            }
        }

        public class LobbyWidgetCallbackStore : CallbackStore
        {
            public Action<BrainDuelsLib.threads.Game> goToGameAsPlayerCallback = delegate(BrainDuelsLib.threads.Game game) { };
            public Action<BrainDuelsLib.threads.Game> goToGameAsObserverCallback = delegate(BrainDuelsLib.threads.Game game) { };
            public Action<List<int>> getPlayersCallback = delegate(List<int> players) { };
            public BrainDuelsLib.delegates.Action<int, string, string, int> rejectedChallengeCallback = delegate(int a, string aa, string aaa, int id) { };
            public BrainDuelsLib.delegates.Action<int, string, string, int> newChallengeCallback = delegate(int a, string aa, string aaa, int id) { };
            public BrainDuelsLib.delegates.Action unexpiredChallengeCallback = delegate() { };
            public BrainDuelsLib.delegates.Action<int> expiredChallengeCallback = delegate(int a) { };
            public BrainDuelsLib.delegates.Action repeatedCallback = delegate() { };
            public Action<List<threads.Game>> gamesCallback = delegate(List<threads.Game> players) { };
        }



        public class LobbyWidgetControlsStore : CallbackStore
        {
            public UserInfoWidget meInfoWidget;
            public GamesChallengesLobbyControl challengesControl;
            public LobbyBackendWidget backgroud;
        }

        private LobbyWidgetControlsStore controlsStore = new LobbyWidgetControlsStore();
        public LobbyWidgetControlsStore Controls
        {
            get
            {
                return controlsStore;
            }
        }


        public LobbyWidget(Server.TokenAndId tai)
            : base()
        {
            this.tai = tai;
        }


        public override void Go()
        {
            base.Go();
            //User me = Server.GetUser(tai, tai.id);
            //if(Controls.meInfoWidget != null){
            //Controls.meInfoWidget.Update(me);
            //}


            Controls.backgroud.Callbacks.gamesCallback = OnGamesCallback;
            if (Controls.challengesControl != null)
            {
                Controls.challengesControl.SetOnEnterGameAsPlayerCallback(OnEnterGameAsPlayer);
                Controls.challengesControl.SetOnEnterGameAsObserverCallback(OnEnterGameAsObserver);
                Controls.challengesControl.SetOnLeaveGameCallback(OnLeaveGame);
            }
            Controls.backgroud.Callbacks.goToGameCallback = OnGoToGameCallback;
            Controls.backgroud.Callbacks.errorCallback = delegate(Exception e)
            {
                store.errorCallback(e);
                Controls.backgroud.Stop();
            };
            Controls.backgroud.Callbacks.usersCallback = delegate(List<int> l)
            {
                Callbacks.getPlayersCallback(l);
            };

            Controls.backgroud.Callbacks.rejectedChallengeCallback = delegate(int a, string aa, string aaa, int id)
            {
                this.Controls.backgroud.Expire(id);
                this.Callbacks.rejectedChallengeCallback(a, aa, aaa, id);
            };


            Controls.backgroud.Callbacks.newChallengeCallback = delegate(int a, string aa, string aaa, int id)
            {
                this.Callbacks.newChallengeCallback(a, aa, aaa, id);
            };


            Controls.backgroud.Callbacks.expireChallengeCallback = delegate(int id)
            {
                this.Callbacks.expiredChallengeCallback(id);
            };


            Controls.backgroud.Callbacks.repeatedCallback = delegate()
            {
                this.Callbacks.repeatedCallback();
            };


            Controls.backgroud.Go();

        }

        public void CreateNewChallenge(int ch, string title, string settings)
        {
            if (Controls.backgroud.CanChallenge())
            {
                Controls.backgroud.ChallengeUser(ch, title, settings);
            }
            else
            {
                this.Callbacks.unexpiredChallengeCallback();
            }
        }


        public void AcceptChallenge(int ch, string title, string settings, int id)
        {
            Controls.backgroud.AcceptChallenge(ch, title, settings, id);
        }

        public void RejectChallenge(int ch, string title, string settings, int id)
        {
            Controls.backgroud.RejectChallenge(ch, title, settings, id);
        }

        public void CreateNewGame(int max, string title, string settings)
        {
            Controls.backgroud.CreateNewGame(max, title, settings);
            isChallenging = true;
        }

        public void OnGamesCallback(List<threads.Game> _games)
        {
            List<threads.Game> gms = new List<BrainDuelsLib.threads.Game>();
            foreach (BrainDuelsLib.threads.Game game in _games)
            {
                BrainDuelsLib.threads.Game n = game.Copy();
                gms.Add(n);
            }
            Callbacks.gamesCallback(gms);
        }

        public void OnGoToGameCallback(BrainDuelsLib.threads.Game game)
        {
            Callbacks.goToGameAsPlayerCallback(game);
        }

        public void OnEnterGameAsObserver(BrainDuelsLib.threads.Game game)
        {
            Callbacks.goToGameAsObserverCallback(game);
        }

        public void OnEnterGameAsPlayer(BrainDuelsLib.threads.Game game)
        {
            Controls.backgroud.EnterGame(game.id);
        }

        public void OnLeaveGame(BrainDuelsLib.threads.Game game)
        {
            Controls.backgroud.LeaveGame(game.id);
        }

        public void Stop()
        {
            Controls.backgroud.Stop();
        }

        public void Discard()
        {
            this.Controls.backgroud.ExpireAll();
        }
    }
}
