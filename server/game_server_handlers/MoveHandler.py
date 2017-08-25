from Server import *
from utils.Validator import *

from utils.print_utils import *
from utils.db_queries import *

from utils.security import *

from game_server_handlers.game_server_utils import *

class MoveHandler(Handler):
    def get_validator(self):
        res = Validator(['game_db_id', 'move'])
        return res

    def get_type(self):
        return 'move'

    def action(self, request, factory, me):
        move = request['move']
        game_db_id = int(request['game_db_id'])
        if isAuthorized(me):
            game = factory.games[game_db_id]
            index = -1
            for i in range(len(game.users)):
                if game.users[i] == me.id:
                    index = i
            if index == -1:
                me.sendLine(print_error(SECURITY_ERROR, ""))
            if game.is_move_legal(move, index):
                game.make_move(move, index)
            else:
                me.sendLine(print_error(MOVE_IS_ILLEGAL, move))
        else:
            me.sendLine(print_error(SECURITY_ERROR, ""))
