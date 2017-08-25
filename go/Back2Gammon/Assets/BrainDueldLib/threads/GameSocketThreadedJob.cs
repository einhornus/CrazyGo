using System.Collections;
using BrainDuelsLib.web;
using System.Collections.Generic;
using BrainDuelsLib.model;
using BrainDuelsLib.model.entities;

//using Newtonsoft.Json;
using BrainDuelsLib.delegates;

namespace BrainDuelsLib.threads
{
    public class GameSocketThreadedJob : SocketThreadedJob
    {
        private BrainDuelsLib.delegates.Action<string, int> gameStateCallback = delegate { };
		private BrainDuelsLib.delegates.Action<string> gameEndedCallback = delegate { };
		private BrainDuelsLib.delegates.Action<int, string> opponentMovedCallback = delegate { };
		private BrainDuelsLib.delegates.Action<Game> initGameCallback = delegate { };
		private BrainDuelsLib.delegates.Action movePermittedCallback = delegate { };
		private BrainDuelsLib.delegates.Action illegalMoveCallback = delegate { };
		private BrainDuelsLib.delegates.Action<string> eventCallback = delegate { };


        private Server.TokenAndId tai;
        private int[] timers;
        private bool[] permissions;
        private Game game;
        private string currentState = "";

        private void MessageCallback(string s)
        {
            if (s.Contains("!MOVE_IS_ILLEGAL")){
                illegalMoveCallback();
                return;
            }

            Message message = new Message(s);

            if (message.header.Equals("end"))
            {
               this.gameEndedCallback(message.content[0]);
            }

            if(message.header.Equals("event")){
                this.eventCallback(message.content[0]);
            }

            if (message.header.Equals("+permission"))
            {
                int index = int.Parse(message.content[0]);
                permissions[index] = true;
                if(index == GetIndex()){
                    movePermittedCallback();
                }
            }

            if (message.header.Equals("-permission"))
            {
                int index = int.Parse(message.content[0]);
                permissions[index] = false;
            }

            if (message.header.Equals("game_state"))
            {
                string state = message.content[0];
                int index = int.Parse(message.content[1]);
                this.currentState = state;
                this.gameStateCallback(state, index);
            }

            if (message.header.Equals("time"))
            {
                int index = int.Parse(message.content[0]);
                int time = int.Parse(message.content[1]);
                timers[index] = time;
            }
        }

        public void Move(string move)
        {
            if(game.users.Contains(tai.id)){
                Write("move move=" + move + ";game_db_id=" + game.id);
            }
            else
            {
            }
        }

        public string GetGameState()
        {
            return currentState;
        }

        private static long GetCurrentTimeMillis()
        {
			long milliseconds = System.DateTime.Now.Ticks / System.TimeSpan.TicksPerMillisecond;
            return milliseconds;
        }

        public int GetIndex()
        {
            int index = -1;
            for (int i = 0; i < game.users.Count; i++)
            {
                if (game.users[i] == tai.id)
                {
                    index = i;
                }
            }
            return index;
        }

        public GameSocketThreadedJob(Server.TokenAndId tai, Game game)
            : base(SocketManager.GAME_SERVERS[game.server], null)
        {
            this.game = game;
            this.tai = tai;
            this.timers = new int[game.max];
            this.permissions = new bool[game.max];
            this.callback = MessageCallback;
            int index = GetIndex();

            if (index != -1)
            {
                string sss = "authorize id=" + tai.id + ";token=" + tai.token + ";game_id=" + game.id + ";index=" + index;
                this.Write(sss);
            }
            else
            {
                string sss = "observe id=" + tai.id + ";token=" + tai.token + ";game_id=" + game.id;
                this.Write(sss);
            }

            if(index == 0){
                string ssss = "initialize id=" + tai.id + ";token=" + tai.token + ";game_id="+game.id+";title="+game.title+";settings="+game.settings+";users=";
                for (int i = 0; i < game.users.Count; i++ )
                {
                    ssss += game.users[i];
                    if(i != game.users.Count - 1){
                        ssss += "?";
                    }
                }
                this.Write(ssss);
            }
        }

        public void SetGameStateCallback(Action<string, int> _callback)
        {
            this.gameStateCallback = _callback;
        }

        public void SetGameEndedCallback(Action<string> _callback)
        {
            this.gameEndedCallback = _callback;
        }

        public void SetOpponentMovedCallback(Action<int, string> _callback)
        {
            this.opponentMovedCallback = _callback;
        }

        public void SetInitGameCallback(Action<Game> _callback)
        {
            this.initGameCallback = _callback;
        }

        public void SetMovePermittedCallback(Action _callback)
        {
            this.movePermittedCallback = _callback;
        }


        public void SetIllegalMoveCallback(Action _callback)
        {
            this.illegalMoveCallback = _callback;
        }

        public void SetEventCallback(Action<string> _callback)
        {
            this.eventCallback = _callback;
        }


        public int[] getTimers(){
            return timers;
        }
    }
}
