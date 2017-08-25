
using System.Collections.Generic; using System;

using System.Text;

using BrainDuelsLib.web;
using BrainDuelsLib.threads;
using BrainDuelsLib.view.forms;
using BrainDuelsLib.view;
using BrainDuelsLib.delegates;
using UnityEngine;

namespace BrainDuelsLib.widgets
{
    public class GameWidget : Widget, IStoppable
    {
        private Server.TokenAndId tai;
        private Game game;
        private GameSocketThreadedJob job;
        private static System.Random random = new System.Random();
        private int id;

        public GameWidget(Server.TokenAndId tai, Game game):base()
        {
            this.game = game;
            this.tai = tai;
            id = random.Next();
        }

        private string gameResultString = null;
        int index = 0;

        Queue<KeyValuePair<int, string>> gameStates = new Queue<KeyValuePair<int, string>>();
        TimerQueue<string> events = new TimerQueue<string>();

        public override void Go()
        {
            base.Go();
            job = new GameSocketThreadedJob(tai, game);
            job.SetGameEndedCallback(delegate(string a) { gameResultString = a; });
            job.SetGameStateCallback(delegate(string x, int index) {
                gameStates.Enqueue(new KeyValuePair<int, string>(index, x));
            });
            job.SetIllegalMoveCallback(Callbacks.illegalMoveCallback);
            job.SetEventCallback(delegate(string s)
            {
                events.Push(s);
            });

            events.callback = delegate (string s)
            {
                Callbacks.eventCallback(s);
            };
            job.Start();
            Controls.timer.Start(OnTimer);
        }

        private GameWidgetWidgetCallbackStore store = new GameWidgetWidgetCallbackStore();
        public GameWidgetWidgetCallbackStore Callbacks
        {
            get
            {
                return store;
            }
        }

        public class GameWidgetWidgetCallbackStore : CallbackStore
        {
			public BrainDuelsLib.delegates.Action<string> gameEndedCallback = delegate(string s) { };
			public BrainDuelsLib.delegates.Action<string, int> updateGameCallback = delegate(string s, int a)  { };
			public BrainDuelsLib.delegates.Action<string> eventCallback = delegate { };
			public BrainDuelsLib.delegates.Action illegalMoveCallback = delegate { };
			public BrainDuelsLib.delegates.Action timerCallback = delegate { };
        }

        public class GameWidgetWidgetControlsStore : CallbackStore
        {
            public TimerControl timer;
        }

        private GameWidgetWidgetControlsStore controlsStore = new GameWidgetWidgetControlsStore();
        public GameWidgetWidgetControlsStore Controls
        {
            get
            {
                return controlsStore;
            }
        }

        public long?[] GetTimers(){
            return null;
        }

        public int getIndex()
        {
            if(game == null){
                return -1;
            }
            for (int i = 0; i < game.users.Count; i++ )
            {
                if(game.users[i] == tai.id){
                    return i;
                }
            }
            return -1;
        }

        public int GetPeopleCount()
        {
            if (game == null)
            {
                return -1;
            }
            else
            {
                return game.max;
            }
        }

        public string GetGame()
        {
            if (game == null)
            {
                return null;
            }
            else
            {
                return game.title;
            }
        }

        public List<int> GetUsers()
        {
            if (game == null)
            {
                return null;
            }
            else
            {
                return game.users;
            }
        }

        public void MakeAMove(string move)
        {
            job.Move(move);
        }


        public void OnTimer()
        {
            while (gameStates.Count > 0)
            {
                KeyValuePair<int, string> pair = gameStates.Dequeue();
                Callbacks.updateGameCallback(pair.Value, pair.Key);
            }

            if (gameResultString != null)
            {
                Callbacks.gameEndedCallback(gameResultString);
                gameResultString = null;
            }

            Callbacks.timerCallback();

            events.OnTimer();
        }

        public void Stop()
        {
            Controls.timer.Stop();
            //NGUIDebug.Log("STAAAAAAAAAAAP");
            job.Stop();
            job.IsDone = true;
            job.Update();
        }
    }
}
