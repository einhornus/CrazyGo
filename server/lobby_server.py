from Server import *
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
from lobby_server_handlers.KillGameHandler import *

from settings import *

server = Server(lobby_server[1])
server.add_handler(AuthorizationHandler())
server.add_handler(EnterGameHandler())
server.add_handler(LeaveHandler())
server.add_handler(CreateGameHandler())
server.add_handler(OpenForRandomChallengeHandler())
server.add_handler(SendChallengeHandler())
server.add_handler(RejectChallengeHandler())
server.add_handler(AcceptChallengeHandler())
server.add_handler(CloseForRandomChallengeHandler())
server.add_handler(ExpireChallengesHandler())
server.add_handler(KillGameHandler())
server.setLostConnectionFunc(lostConnectionFunc)
#log.startLogging(open('lobby_server.txt', 'w+'))
server.launch()