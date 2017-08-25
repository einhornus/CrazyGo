from Server import *
from utils.Validator import *

from utils.print_utils import *
from utils.db_queries import *

from utils.security import *

from lobby_server_handlers.factory_utils import *

class KillGameHandler(Handler):
    def get_validator(self):
        res = Validator(['game_id'])
        return res

    def get_type(self):
        return 'kill_dfkfwwqpfdkg4ssvdfgg'

    def action(self, request, factory, me):
        game_id = int(request['game_id'])
        index = -1
        for i in range(len(factory.games)):
            if factory.games[i].id == game_id:
                index = i
        print('index ='+str(index))
        me.sendLine("!KILLED")
        if index != -1:
            factory.games.pop(index)
            print(factory.games, index)
            sendMessageToAllClients(factory, "game_killed " + str(game_id))