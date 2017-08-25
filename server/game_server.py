from Server import *
from game_server_handlers.PlayerAuthorizationHandler import *
from game_server_handlers.MoveHandler import *
from game_server_handlers.InitializationHandler import *
from game_server_handlers.RegisterSlaveHandler import *
from game_server_handlers.TimeHandler import *
from game_server_handlers.ObserverAuthorizationHandler import *
from settings import *

index = 0

def gameLostConnectionFunc(self):
    if not hasattr(self.factory, 'clientProtocols'):
        self.factory.clientProtocols = []
    if self in self.factory.clientProtocols:
        self.factory.clientProtocols.remove(self)
    for __id in self.factory.games:
        gm = self.factory.games[__id]
        obs = gm.observers
        ins = self.id in obs
        if ins:
            obs.remove(self.id)

    for __id in self.factory.games:
        gm = self.factory.games[__id]
        obs = gm.active_players
        ins = self.id in obs
        if ins:
            obs.remove(self.id)


server = Server(game_servers[index][1])
server.add_handler(PlayerAuthorizationHandler())
server.add_handler(InitializationHandler())
server.add_handler(MoveHandler())
server.add_handler(RegisterSlaveHandler())
server.add_handler(TimeHandler())
server.add_handler(ObserverAuthorizationHandler())
server.setLostConnectionFunc(gameLostConnectionFunc)
#log.startLogging(open('game_server.txt', 'w+'))
server.launch()