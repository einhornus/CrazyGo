from Server import *
from utils.Validator import *

from utils.print_utils import *
from utils.db_queries import *

from utils.security import *

import game_server_handlers.game_server_utils

class PlayerAuthorizationHandler(Handler):
    def get_validator(self):
        res = Validator(['id', 'token', 'game_id', 'index'])
        return res

    def get_type(self):
        return 'authorize'

    def action(self, request, factory, me):
        id = int(request['id'])
        token = request['token']
        game_id = int(request['game_id'])
        index = int(request['index'])
        if check_token(id, token):
            game_server_handlers.game_server_utils.userEntered(me, id)
            if game_server_handlers.game_server_utils.is_game_exists(factory, game_id):
                game = factory.games[game_id]
                game_server_handlers.game_server_utils.handle_application(factory, game, id)
            else:
                game_server_handlers.game_server_utils.place_application(factory, game_id, index, id)
        else:
            me.sendLine(print_error(SECURITY_ERROR, ""))