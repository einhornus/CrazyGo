from Server import *
import lobby_server_handlers;
import db_server_handlers;
from db_server_handlers.EchoHandler import *
from db_server_handlers.RegistrationHandler import *
from db_server_handlers.AuthorizationHandler import *
from db_server_handlers.GetUserInfoHandler import *
from db_server_handlers.SetUserInfoHandler import *
from db_server_handlers.SetAvatarHandler import *
from db_server_handlers.ReportGameHandler import *
from db_server_handlers.GetUserCoinsInfoHandler import *
from db_server_handlers.LeaderboardHandler import *
from lobby_server_handlers.CreateGameHandler import *
from lobby_server_handlers.EnterHandler import *
from lobby_server_handlers.LeaveHandler import *
from lobby_server_handlers.AuthorizationHandler import *
from lobby_server_handlers.OpenForRandomChallengeHandler import *
from lobby_server_handlers.CloseForRandomChallengeHandler import *
from lobby_server_handlers.SendChallengeHandler import *
from lobby_server_handlers.AcceptChallengeHandler import *
from lobby_server_handlers.RejectChallengeHandler import *
from lobby_server_handlers.ExpireChallengesHandler import *
from game_server_handlers.PlayerAuthorizationHandler import *
from game_server_handlers.MoveHandler import *
from game_server_handlers.InitializationHandler import *
from game_server_handlers.RegisterSlaveHandler import *
from game_server_handlers.TimeHandler import *
from game_server_handlers.ObserverAuthorizationHandler import *
from settings import *

server = Server(db_server[1])
server.add_handler(RegistrationHandler())
server.add_handler(lobby_server_handlers.AuthorizationHandler.AuthorizationHandler())
server.add_handler(GetUserInfoHandler())
server.add_handler(SetUserInfoHandler())
server.add_handler(SetAvatarHandler())
server.add_handler(ReportGameHandler())
server.add_handler(GetUserCoinsInfoHandler())
server.add_handler(LeaderboardHandler())
server.add_handler(db_server_handlers.AuthorizationHandler.AuthorizationHandler())
server.add_handler(EnterGameHandler())
server.add_handler(LeaveHandler())
server.add_handler(CreateGameHandler())
server.add_handler(OpenForRandomChallengeHandler())
server.add_handler(SendChallengeHandler())
server.add_handler(RejectChallengeHandler())
server.add_handler(AcceptChallengeHandler())
server.add_handler(CloseForRandomChallengeHandler())
server.add_handler(ExpireChallengesHandler())
server.add_handler(PlayerAuthorizationHandler())
server.add_handler(InitializationHandler())
server.add_handler(MoveHandler())
server.add_handler(RegisterSlaveHandler())
server.add_handler(TimeHandler())
server.add_handler(ObserverAuthorizationHandler())
server.setLostConnectionFunc(lostConnectionFunc)
server.launch()