from Server import *
from utils.Validator import *

from utils.print_utils import *
from utils.db_queries import *

from utils.security import *

from lobby_server_handlers.factory_utils import *

class EnterGameHandler(Handler):
    def get_validator(self):
        res = Validator(['game_id'])
        return res

    def get_type(self):
        return 'enter'

    def action(self, request, factory, me):
        game_id = request['game_id']
        game = findGame(factory, game_id)
        if isAuthorized(me):
            if not(game is None):
                if not(me.id in game.users) and game.status == 'open':
                    game.users.append(me.id)
                    sendMessageToAllClients(factory, "game+ "+str(game.id)+";"+str(me.id))
                    if len(game.users) == game.max:
                        game.status = 'closed'
                        sendMessageToAllClients(factory, "game_closed "+str(game.id))
                        for i in range(len(game.users)):
                            client = findClient(factory, game.users[i])
                            client.sendLine("go_to_game "+str(game))
                else:
                    me.sendLine("go_to_game " + str(game))
                    me.sendLine(print_error(SECURITY_ERROR+"_exist", ""))
            else:
                me.sendLine(print_error(SECURITY_ERROR+"_not_open", ""))
        else:
            me.sendLine(print_error(SECURITY_ERROR+"_not_auth", ""))