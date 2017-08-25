from Server import *
from utils.Validator import *

from utils.print_utils import *
from utils.db_queries import *

from utils.security import *


class LeaderboardHandler(Handler):
    def get_validator(self):
        res = Validator(['id', 'token', 'size', "narde", "backgammon"])
        return res

    def get_type(self):
        return 'leaderboard'

    def action(self, request, factory, me):
        id = request["id"]
        token = request["token"]
        if check_token(id, token):
            include_narde = int(request["narde"]) == 1
            include_backgammon = int(request["backgammon"]) == 1
            size = int(request["size"])
            table = get_leaderboard(size, include_backgammon, include_narde)
            me.sendLine(print_object(table))
        else:
            me.sendLine(print_error(SECURITY_ERROR, token))
