from Server import *
from utils.Validator import *

from utils.print_utils import *
from utils.db_queries import *

from utils.security import *
import random

from lobby_server_handlers.factory_utils import *

class CreateGameHandler(Handler):
    def get_validator(self):
        res = Validator(['max', 'game', 'settings'])
        return res

    def get_type(self):
        return 'create_game'

    def genId(self):
        return random.randint(0, 1000000000)

    def action(self, request, factory, me):
        max = request["max"]
        game = request["game"]
        settings = request['settings']
        if isAuthorized(me):
            game = GameChallenge(self.genId(), game, settings, max)
            if not hasattr(me.factory, "games"):
                me.factory.games = []
            me.factory.games.append(game)
            sendMessageToAllClients(factory, "new_game "+str(game))

            if not(game is None) and game.status == 'open':
                if not(me.id in game.users):
                    game.users.append(me.id)
                    sendMessageToAllClients(factory, "game+ "+str(game.id)+";"+str(me.id))
                    if len(game.users) == game.max:
                        game.status = 'closed'
                        sendMessageToAllClients(factory, "game_closed "+str(game.id))
                        for i in range(len(game.users)):
                            client = findClient(factory, game.users[i])
                            client.sendLine("go_to_game "+str(game))
                else:
                    me.sendLine(print_error(SECURITY_ERROR, ""))
            else:
                me.sendLine(print_error(SECURITY_ERROR, ""))
        else:
            me.sendLine(print_error(SECURITY_ERROR, ""))