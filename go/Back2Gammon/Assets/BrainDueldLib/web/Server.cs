
using System.Collections.Generic; using System;

using System.Text;
using BrainDuelsLib.model.entities;
using BrainDuelsLib.web.requests;
using BrainDuelsLib.web.exceptions;
//using Newtonsoft.Json;

using BrainDuelsLib.utils.json;
using BrainDuelsLib.utils.pic;
using BrainDuelsLib.web.socket_requests;
using BrainDuelsLib.delegates;



namespace BrainDuelsLib.web
{
    public class Server
    {
        public class TokenAndId
        {
            public String token;
            public int id;
        }

        public class RegistractionServerResult
        {
        }

        public class AuthorizationServerResult
        {
            public int id;
            public String token;
        }

        public class GameServerResult
        {
            public int result_code;
            public string game_state;
            public int game_result;
        }

        public class GameStateServerResult
        {
            public int result_code;
            public string game_state;
        }

        public class IAmHereServerResult
        {
            public int result_code;
        }

        public class GetUsersServerResult
        {
            public class ActiveUser
            {
                public int user_id;
            }
            public List<ActiveUser> active_users;
            public int result_code;
        }

        public class GetPictureServerResult
        {
            public int result_code;
            public String picture;
        }

        public class AddPictureServerResult
        {
            public int result_code;
            public int avatar_id;
        }

        public class ResultCodeServerResult
        {
            public int result_code;
        }

        public class UserCoinsServerResult
        {
            public int user_id;
            public int xp;
            public int coins;
        }

        public class SubmitServerResult
        {
            public int result_code;
            public int submit_id;
        }

        public static void Register(String login, String password){
            string str = "register "+"login="+login+";password="+password;
            Response response = DBSocketRequest.Send(str);
            if (response is ErrorResponse)
            {
                Exception exception = ((ErrorResponse)response).ThrowException();
                throw exception;
            }
            else
            {
                RegistractionServerResult result = JsonConvert.DeserializeObject<RegistractionServerResult>(response.ToString());
            }
        }

        public static void RequestCoins(Server.TokenAndId tai, int amount)
        {
            string str = "add_coins " + "id=" + tai.id + ";token=" + tai.token+";amount="+amount;
            Response response = DBSocketRequest.Send(str);
            if (response is ErrorResponse)
            {
                Exception exception = ((ErrorResponse)response).ThrowException();
                throw exception;
            }
            else
            {
                RegistractionServerResult result = JsonConvert.DeserializeObject<RegistractionServerResult>(response.ToString());
            }
        }

        public static TokenAndId Authorize(String login, String password){
            string str = "authorize " + "login=" + login + ";password=" + password;
            Response response = DBSocketRequest.Send(str);
            if (response is ErrorResponse)
            {
                Exception exception = ((ErrorResponse)response).ThrowException();
                throw exception;
            }
            else
            {
                AuthorizationServerResult result = JsonConvert.DeserializeObject<AuthorizationServerResult>(response.ToString());
                TokenAndId res = new TokenAndId();
                res.token = result.token;
                res.id = result.id;
                return res;
            }
        }


        public class LeaderboardResult{
            public int id;
            public int games;
            public double roi;
            public double rating;
        }

        public static List<LeaderboardResult> GetLeaderboard(Server.TokenAndId tai, bool backgammon, bool narde, int size)
        {
            string backgammonString = backgammon ? "1" : "0";
            string nardeString = narde ? "1" : "0";
            string str = "leaderboard " + "id=" + tai.id + ";token=" + tai.token + ";size=" + size+";"+"backgammon="+backgammonString+";narde="+nardeString;
            Response response = DBSocketRequest.Send(str);
            if (response is ErrorResponse)
            {
                Exception exception = ((ErrorResponse)response).ThrowException();
                throw exception;
            }
            else
            {
                double[][] result = JsonConvert.DeserializeObject<double[][]>(response.ToString());
                List<LeaderboardResult> res = new List<LeaderboardResult>();
                for (int i = 0; i<result.Length; i++)
                {
                    int id = (int)(result[i][0]);
                    int games = (int)(result[i][1]);

                    double roi = (int)(result[i][2]);
                    double rating = (int)(result[i][3]);

                    LeaderboardResult lr = new LeaderboardResult();
                    lr.id = id;
                    lr.games = games;
                    lr.roi = roi;
                    lr.rating = rating;
                    res.Add(lr);
                }
                return res;
            }
        }

        public static Dictionary<int, User> usersDictionary = new Dictionary<int, User>();

        public static User GetUser(TokenAndId tai, int id)
        {
            if (usersDictionary.ContainsKey(id))
            {
                return usersDictionary[id];
            }
            else
            {
                User res = _GetUser(tai, id);
                usersDictionary.Add(id, res);
                return res;
            }
        }

        public static User UpdateUser(TokenAndId tai, int id)
        {
            User res = _GetUser(tai, id);
            if (usersDictionary.ContainsKey(id))
            {
                usersDictionary[id] = res;
            }
            else
            {
                usersDictionary.Add(id, res);
            }
            return res;
        }


        public static User _GetUser(TokenAndId tai, int userId)
        {
            string str = "get_user " + "id=" + tai.id + ";token=" + tai.token+";user_id="+userId;
            Response response = DBSocketRequest.Send(str);
            if (response is ErrorResponse)
            {
                Exception exception = ((ErrorResponse)response).ThrowException();
                throw exception;
            }
            else
            {
                UserJsonAvatar result = JsonConvert.DeserializeObject<UserJsonAvatar>(response.ToString());
                User user = new User();
                result.CopyData(user);
                user.id = userId;
                return user;
            }
        }



        public static Dictionary<KeyValuePair<int, int>, LightImage> picturesCache = new Dictionary<KeyValuePair<int, int>, LightImage>();

		public static void GetPicture(Server.TokenAndId tai, int serverIndex, int pictureId, BrainDuelsLib.delegates.Action<LightImage> imageCallback){
            KeyValuePair<int, int> address = new KeyValuePair<int, int>(serverIndex, pictureId);
            if(picturesCache.ContainsKey(address)){
                imageCallback(picturesCache[address]);
                return;
            }

			BrainDuelsLib.delegates.Action<string> resultCallback = delegate(string s){
                LightImage li = ImageSerizlizer.Deserialize(s);
                picturesCache[address] = li;
                imageCallback(li);
            };

            PictureRequest.GetPicture(tai, serverIndex, pictureId, resultCallback);
            return;
        }


		public static void SetAva(Server.TokenAndId tai, LightImage image, BrainDuelsLib.delegates.Action callback)
        {
            LightImage largeImage = image.CropToSize(SocketManager.Images.avatarSize.width, SocketManager.Images.avatarSize.height);
            LightImage miniImage = image.CropToSize(SocketManager.Images.miniAvatarSize.width, SocketManager.Images.miniAvatarSize.height);
            string content1 = ImageSerizlizer.Serialize(largeImage);
            string content2 = ImageSerizlizer.Serialize(miniImage);
            PictureRequest.SetPicture(tai, content1, callback, true);
            PictureRequest.SetPicture(tai, content2, callback, false);
        }

        public static Dictionary<int, String> loginsDic = new Dictionary<int, string>();
        public static String GetUserLogin(int id, TokenAndId tai)
        {
            if(loginsDic.ContainsKey(id)){
                return loginsDic[id];
            }
            User user = GetUser(tai, id);
            loginsDic.Add(id, user.login);
            return user.login;
        }

        public static void SetUser(TokenAndId tai, int newRank)
        {
            string str = "set_user " + "id=" + tai.id + ";token=" + tai.token+";new_rank="+newRank;
            Response response = DBSocketRequest.Send(str);
            if (response is ErrorResponse)
            {
                Exception exception = ((ErrorResponse)response).ThrowException();
                throw exception;
            }
            else
            {
                return;
            }
        }
    }
}
