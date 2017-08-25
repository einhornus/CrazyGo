using System.Collections;
using BrainDuelsLib.web;
using System.Collections.Generic; using System;
using BrainDuelsLib.model;
using BrainDuelsLib.model.entities;

//using Newtonsoft.Json;
using BrainDuelsLib.delegates;



namespace BrainDuelsLib.threads
{
    public class LobbySocketThreadedJob : SocketThreadedJob
    {
		private BrainDuelsLib.delegates.Action<List<int>> usersCallback = delegate { };
        private BrainDuelsLib.delegates.Action<int, string, string, int> newChallengeCallback = delegate { };
        private BrainDuelsLib.delegates.Action<int, string, string, int> rejectedChallengeCallback = delegate { };
        private BrainDuelsLib.delegates.Action<int> createdChallengeCallback = delegate { };
        private BrainDuelsLib.delegates.Action<int> expiredChallengeCallback = delegate { };
        private BrainDuelsLib.delegates.Action<List<Game>> gamesCallback = delegate { };
		private BrainDuelsLib.delegates.Action<Game> toGameCallback = delegate { };
		private BrainDuelsLib.delegates.Action gameNotFoundCallback = delegate { };
		private BrainDuelsLib.delegates.Action didNotEnteredGameCallback = delegate { };
        private BrainDuelsLib.delegates.Action connectionRepeatCallback = delegate { };

        private Server.TokenAndId tai;

        private List<int> users = new List<int>();
        private List<Game> games = new List<Game>(); 

        private void MessageCallback(string s){
            if (s.Contains("CONNECTION_REPEAT"))
            {
                connectionRepeatCallback();
            }

            Message message = new Message(s);
            if (message.header.Equals("all_users"))
            {
                for (int i = 0; i < message.content.Length-1; i++ )
                {
                    int id = int.Parse(message.content[i]);
                    users.Add(id);
                }
            }

            if (message.header.Equals("access_denied"))
            {
                throw new BrainDuelsLib.web.exceptions.SecurityError();
            }

            if (message.header.Equals("+"))
            {
                int id = int.Parse(message.content[0]);
                users.Add(id);
            }

            if (message.header.Equals("-"))
            {
                int id = int.Parse(message.content[0]);
                users.Remove(id);
            }

            if(message.header.Equals("all_games")){
                for (int i = 0; i < message.content.Length; i++ )
                {
                    if (!message.content[i].Equals(""))
                    {
                        Game game = JsonConvert.DeserializeObject<Game>(message.content[i].ToString());
                        games.Add(game);
                    }
                }
            }

            if (message.header.Equals("game+"))
            {
                int gameId = int.Parse(message.content[0]);
                int userId = int.Parse(message.content[1]);
                Game game = FindGame(gameId);
                game.users.Add(userId);
            }

            if (message.header.Equals("game-"))
            {
                int gameId = int.Parse(message.content[0]);
                int userId = int.Parse(message.content[1]);
                Game game = FindGame(gameId);
                game.users.Remove(userId);
            }

            if (message.header.Equals("game_closed"))
            {
                int gameId = int.Parse(message.content[0]);
                Game game = FindGame(gameId);
                game.status = "closed";
            }

            if (message.header.Equals("game_killed"))
            {
                int gameId = int.Parse(message.content[0]);
                Game game = FindGame(gameId);
                games.Remove(game);
            }

            if (message.header.Equals("game_removed"))
            {
                int gameId = int.Parse(message.content[0]);
                int index = -1;
                for (int i = 0; i < games.Count; i++ )
                {
                    if(games[i].id == gameId){
                        index = i;
                    }
                }
                //NGUIDebug.Log(index);
                games.RemoveAt(index); //-V3057
            }

            if (message.header.Equals("new_game"))
            {
                Game game = JsonConvert.DeserializeObject<Game>(message.content[0].ToString());
                games.Add(game);
            }

            if (message.header.Equals("go_to_game"))
            {
                Game game = JsonConvert.DeserializeObject<Game>(message.content[0].ToString());
                this.toGameCallback(game);
            }

            if (message.header.Equals("game_not_found"))
            {
                gameNotFoundCallback();
            }

            if (message.header.Equals("you_did_not_entered_the_game"))
            {
                didNotEnteredGameCallback();
            }



            if (message.header.Equals("new_challenge"))
            {
                int from = int.Parse(message.content[0]);
                string game = message.content[1];
                string settings = message.content[2];
                int id = int.Parse(message.content[3]);
                newChallengeCallback(from, game, settings, id);
            }


            if (message.header.Equals("reject_challenge"))
            {
                int from = int.Parse(message.content[0]);
                string game = message.content[1];
                string settings = message.content[2];
                int id = int.Parse(message.content[3]);
                rejectedChallengeCallback(from, game, settings, id);
            }

            if (message.header.Equals("created_challenge"))
            {
                int id = int.Parse(message.content[0]);
                createdChallengeCallback(id);
            }

            if (message.header.Equals("expired_challenge"))
            {
                int id = int.Parse(message.content[0]);
                expiredChallengeCallback(id);
            }

            usersCallback(users);
            gamesCallback(games);
        }

        private Game FindGame(int id)
        {
            for (int i = 0; i < games.Count; i++ )
            {
                if(games[i].id == id){
                    return games[i];
                }
            }
            return null;
        }

        public void ExpireChallenge(int id)
        {
            string str = "expire id=" + tai.id + ";challenge_id=" + id;
            this.Write(str);
        }


        public void SendNewChallenge(int challenger, string game, string settings)
        {
            string str = "challenge id=" + tai.id +";challenger_id="+challenger+ ";title=" + game + ";settings=" + settings;
            this.Write(str);
        }

        public void AcceptChallenge(int challenger, string game, string settings, int id)
        {
            string str = "accept_challenge id=" + tai.id + ";from_id=" + challenger + ";title=" + game + ";settings=" + settings+";challenge_id="+id;
            this.Write(str);
        }

        public void RejectChallenge(int challenger, string game, string settings, int id)
        {
            string str = "reject_challenge id=" + tai.id + ";from_id=" + challenger + ";title=" + game + ";settings=" + settings + ";challenge_id=" + id;
            this.Write(str);
        }

        public void CreateNewGame(int max, string game, string settings)
        {
            string str = "create_game max=" + max + ";game=" + game + ";settings=" + settings;
            this.Write(str);
        }

        public void OpenForRandomChallenge(string game, string settings)
        {
            string str = "open_for_random_challenge " + "game=" + game + ";settings=" + settings+";id="+this.tai.id;
            this.Write(str);
        }

        public void CloseForRandomChallenge(string game, string settings)
        {
            string str = "close_for_random_challenge " + "game=" + game + ";settings=" + settings + ";id=" + this.tai.id;
            this.Write(str);
        }

        public void EnterGame(int id)
        {
            string str = "enter game_id=" + id;
            this.Write(str);
        }

        public void LeaveGame(int id)
        {
            string str = "leave game_id=" + id;
            this.Write(str);
        }

        public LobbySocketThreadedJob(Server.TokenAndId tai):base(SocketManager.LOBBY_SERVER, null)
        {
            this.tai = tai;
            this.callback = MessageCallback;
            this.Write("authorize id="+tai.id+";token="+tai.token);
        }



        public void SetRejectedChallengeCallback(BrainDuelsLib.delegates.Action<int, string, string, int> _callback)
        {
            rejectedChallengeCallback = _callback;
        }


        public void SetNewChallengeCallback(BrainDuelsLib.delegates.Action<int, string, string, int> _callback)
        {
            newChallengeCallback = _callback;
        }

        public void SetToGameCallback(BrainDuelsLib.delegates.Action<Game> _callback)
        {
            toGameCallback = _callback;
        }

        public void SetCreatedChallengeCallback(BrainDuelsLib.delegates.Action<int> _callback)
        {
            createdChallengeCallback = _callback;
        }


        public void SetExpiredChallengeCallback(BrainDuelsLib.delegates.Action<int> _callback)
        {
            expiredChallengeCallback = _callback;
        }


        public void SetUsersCallback(BrainDuelsLib.delegates.Action<List<int>> callback)
        {
            usersCallback = callback;
        }

		public void SetGamesCallback(BrainDuelsLib.delegates.Action<List<Game>> callback)
        {
            gamesCallback = callback;
        }

		public void SetGameNotFoundCallback(BrainDuelsLib.delegates.Action callback)
        {
            gameNotFoundCallback = callback;
        }

		public void SetDidNotEnteredGameCallback(BrainDuelsLib.delegates.Action callback)
        {
            this.didNotEnteredGameCallback = callback;
        }

        public void SetConnectionRepeatedCallbacl(BrainDuelsLib.delegates.Action callback)
        {
            this.connectionRepeatCallback = callback;
        }
    }
}
