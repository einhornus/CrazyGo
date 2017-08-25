from Server import *
from utils.Validator import *

from utils.print_utils import *
from utils.db_queries import *

from utils.security import *
from game_server_handlers.game_server_utils import *



class TimeHandler(Handler):
    def get_validator(self):
        res = Validator([])
        return res

    def get_type(self):
        return 'time'

    def action(self, request, factory, me):
        if hasattr(factory, "games"):
            for game_id in factory.games:
                game = factory.games[game_id]
                if game.stopped:
                    continue
                game.on_time()

