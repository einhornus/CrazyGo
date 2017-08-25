
using System.Collections.Generic; using System;

using System.Text;

using BrainDuelsLib.view;
using BrainDuelsLib.view.forms;
using BrainDuelsLib.threads;
using BrainDuelsLib.model.entities;
using BrainDuelsLib.web;

namespace BrainDuelsLib.widgets
{
    public class LobbyBackendWidget : Widget, IStoppable
    {
        private LobbySocketThreadedJob backgroudJob;
        private Server.TokenAndId tai;

        private LobbyBackendWidgetCallbackStore store = new LobbyBackendWidgetCallbackStore();
        public LobbyBackendWidgetCallbackStore Callbacks
        {
            get
            {
                return store;
            }
        }

        public class LobbyBackendWidgetCallbackStore : CallbackStore
        {
            public Action<List<int>> usersCallback = delegate { };
            public Action<List<Game>> gamesCallback = delegate { 
                //NGUIDebug.Log("lbw games"); 
            };
            public Action<Game> goToGameCallback = delegate { };
            public BrainDuelsLib.delegates.Action<int, string, string, int> rejectedChallengeCallback = delegate { };
            public BrainDuelsLib.delegates.Action<int> expireChallengeCallback = delegate { };
            public BrainDuelsLib.delegates.Action<int, string, string, int> newChallengeCallback = delegate { };
            public BrainDuelsLib.delegates.Action<int> createdChallengeCallback = delegate { };
            public BrainDuelsLib.delegates.Action repeatedCallback = delegate { };


        }

        public class LobbyBackendWidgetControlsStore : CallbackStore
        {
            public TimerControl timer;
        }

        private LobbyBackendWidgetControlsStore controlsStore = new LobbyBackendWidgetControlsStore();
        public LobbyBackendWidgetControlsStore Controls
        {
            get
            {
                return controlsStore;
            }
        }

        public LobbyBackendWidget(Server.TokenAndId tai)
            : base()
        {
            this.tai = tai;
        }

        public override void Go()
        {
            base.Go();
            try
            {
                backgroudJob = new LobbySocketThreadedJob(tai);
                backgroudJob.Start();
                backgroudJob.SetUsersCallback(OnUsersCallback);
                backgroudJob.SetToGameCallback(OnGoToGameCallback);
                backgroudJob.SetGamesCallback(OnGamesCallback);
                backgroudJob.SetRejectedChallengeCallback(OnRejectedChallenge);
                backgroudJob.SetNewChallengeCallback(OnNewChallenge);
                backgroudJob.SetCreatedChallengeCallback(OnCreateChallenge);
                backgroudJob.SetExpiredChallengeCallback(OnExpireChallenge);
                backgroudJob.SetConnectionRepeatedCallbacl(OnRepeat);


                //backgroudJob.SetNewChallengeCallback(OnNewChallenge);

                rejectedChallengeTimerQueue.callback = delegate(ChallengeResponse r)
                {
                    this.Callbacks.rejectedChallengeCallback(r.index, r.title, r.settings, r.id);
                };

                newChallengeTimerQueue.callback = delegate (ChallengeResponse r)
                {
                    this.Callbacks.newChallengeCallback(r.index, r.title, r.settings, r.id);
                };

                expireChallengeCallbaclTimerQueue.callback = delegate (int r)
                {
                    this.Callbacks.expireChallengeCallback(r);
                };

                repeatQueue.callback = delegate(int r)
                {
                    this.Callbacks.repeatedCallback();
                };

                Controls.timer.Start(OnTimer);
            }
            catch(Exception e){
                store.errorCallback(e);
            }
        }

        private bool newGoToGame = false;
        public void OnTimer()
        {
            store.usersCallback(users);
            store.gamesCallback(games);

            if (newGoToGame)
            {
                newGoToGame = false;
                store.goToGameCallback(game);
            }

            rejectedChallengeTimerQueue.OnTimer();
            newChallengeTimerQueue.OnTimer();
            expireChallengeCallbaclTimerQueue.OnTimer();
            this.repeatQueue.OnTimer();

        }

        private List<int> users = new List<int>();
        private List<Game> games = new List<Game>();
        public void OnUsersCallback(List<int> users)
        {
            this.users = users;
        }

        public void OnGamesCallback(List<Game> games)
        {
            this.games = games;
        }

        private Game game;
        public void OnGoToGameCallback(Game game)
        {
            this.game = game;
            newGoToGame = true;
        }

        public class ChallengeResponse
        {
            public ChallengeResponse(int a, string aa, string aaa, int id)
            {
                index = a;
                title = aa;
                settings = aaa;
                this.id = id;
            }
            public int index;
            public string title;
            public string settings;
            public int id;
        }

        TimerQueue<ChallengeResponse> rejectedChallengeTimerQueue = new TimerQueue<ChallengeResponse>();
        TimerQueue<ChallengeResponse> newChallengeTimerQueue = new TimerQueue<ChallengeResponse>();
        TimerQueue<int> expireChallengeCallbaclTimerQueue= new TimerQueue<int>();
        TimerQueue<int> repeatQueue = new TimerQueue<int>();

        public void OnRepeat()
        {
            repeatQueue.Push(0);
        }


        public void OnRejectedChallenge(int id, string title, string settings, int chId)
        {
            ChallengeResponse resp = new ChallengeResponse(id, title, settings, chId);
            rejectedChallengeTimerQueue.Push(resp);
        }


        public void OnNewChallenge(int id, string title, string settings, int challengeId)
        {
            ChallengeResponse resp = new ChallengeResponse(id, title, settings, challengeId);
            newChallengeTimerQueue.Push(resp);
        }

        List<int> myChallenges = new List<int>();

        public void OnCreateChallenge(int id)
        {
            myChallenges.Add(id);
            //TODOexpireChallengeCallbaclTimerQueue.Push(id);
        }

        public void ExpireAll()
        {
            for (int i = 0; i<myChallenges.Count; i++)
            {
                backgroudJob.ExpireChallenge(myChallenges[i]);
            }
            myChallenges = new List<int>();
        }

        public void Expire(int id)
        {
            backgroudJob.ExpireChallenge(id);
            List<int> newChallenges = new List<int>();
            for (int i = 0; i<myChallenges.Count; i++)
            {
                if (myChallenges[i] != id)
                {
                    newChallenges.Add(newChallenges[i]);
                }
            }
            myChallenges = newChallenges;
        }

        public bool CanChallenge()
        {
            return myChallenges.Count == 0;
        }

        public void OnExpireChallenge(int id)
        {
            expireChallengeCallbaclTimerQueue.Push(id);
        }

        public void CreateNewGame(int max, string title, string settings)
        {
            backgroudJob.CreateNewGame(max, title, settings);
        }

        public void ChallengeUser(int user, string title, string settings)
        {
            backgroudJob.SendNewChallenge(user, title, settings);
        }



        public void OpenForRandomChallenge(string title, string settings)
        {
            backgroudJob.OpenForRandomChallenge(title, settings);
        }

        public void CloseForRandomChallenge(string title, string settings)
        {
            backgroudJob.CloseForRandomChallenge(title, settings);
        }


        public void AcceptChallenge(int user, string title, string settings, int id)
        {
            backgroudJob.AcceptChallenge(user, title, settings, id);
        }

        public void RejectChallenge(int user, string title, string settings, int id)
        {
            backgroudJob.RejectChallenge(user, title, settings, id);
        }

        public void EnterGame(int gameId)
        {
            backgroudJob.EnterGame(gameId);
        }

        public void LeaveGame(int gameId)
        {
            backgroudJob.LeaveGame(gameId);
        }

        public void Stop()
        {
            //NGUIDebug.Log("STAAAAAAAAAAAP");
            backgroudJob.Stop();
            backgroudJob.IsDone = true;
            backgroudJob.Update();
            this.Controls.timer.Stop();
        }
    }
}
