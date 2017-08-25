from Server import *
from utils.Validator import *

from utils.print_utils import *
from utils.db_queries import *

from utils.security import *

from lobby_server_handlers.factory_utils import *

class LeaveHandler(Handler):
    def get_validator(self):
        res = Validator(['game_id'])
        return res

    def get_type(self):
        return 'leave'

    def action(self, request, factory, me):
        if isAuthorized(me):
            game_id = int(request['game_id'])
            game = findGame(factory, game_id)
            if not(game is None):
                leaveGame(me, game)
            else:
                me.sendLine("game_not_found ")
        else:
            me.sendLine(print_error(SECURITY_ERROR, ""))