from Server import *
from db_server_handlers.EchoHandler import *
from db_server_handlers.RegistrationHandler import *
from db_server_handlers.AuthorizationHandler import *
from db_server_handlers.GetUserInfoHandler import *
from db_server_handlers.SetUserInfoHandler import *
from db_server_handlers.SetAvatarHandler import *
from db_server_handlers.ReportGameHandler import *
from db_server_handlers.GetUserCoinsInfoHandler import *
from db_server_handlers.LeaderboardHandler import *
from db_server_handlers.AddCoinsHandler import *
from db_server_handlers.GetUsersInfoHandler import *
from db_server_handlers.GetUsersCoinsInfoHandler import *
from settings import *
import sys



server = Server(db_server[1])
server.add_handler(RegistrationHandler())
server.add_handler(AuthorizationHandler())
server.add_handler(GetUserInfoHandler())
server.add_handler(SetUserInfoHandler())
server.add_handler(SetAvatarHandler())
server.add_handler(ReportGameHandler())
server.add_handler(GetUserCoinsInfoHandler())
server.add_handler(LeaderboardHandler())
server.add_handler(AddCoinsHandler())
server.add_handler(GetUsersInfoHandler())
server.add_handler(GetUsersCoinsInfoHandler())
#log.startLogging(open('db_server.txt', 'w+'))
server.launch()
