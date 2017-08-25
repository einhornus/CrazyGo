from Server import *
from utils.Validator import *

from utils.print_utils import *
from utils.db_queries import *

from utils.security import *

import game_server_handlers.game_server_utils

class InitializationHandler(Handler):
    def get_validator(self):
        res = Validator(['id', 'token', 'game_id', 'users', 'title', 'settings'])
        return res

    def get_type(self):
        return 'initialize'

    def action(self, request, factory, me):
        id = int(request['id'])
        token = request['token']
        game_id = int(request['game_id'])
        _users = request['users']
        lst = _users.split('?')
        users = [int(lst[i]) for i in range(len(lst))]
        title = request['title']
        settings = request['settings']
        if check_token(id, token):
            if not(game_id in factory.games):
                new_game = game_server_handlers.game_server_utils.init_game(factory, game_id, title, settings, users)
                if not hasattr(factory, 'applications'):
                    factory.applications = []
                for i in range(len(factory.applications)):
                    application_game_id, application_index, application_user_id = factory.applications[i]
                    if application_game_id == game_id:
                        actual_user_id = new_game.users[application_index]
                        if application_user_id != actual_user_id:
                            me.sendLine(print_error(SECURITY_ERROR, ""))
                            pass
                        else:
                            game_server_handlers.game_server_utils.handle_application(factory, new_game, application_user_id)
            else:
                print('empty init')
        else:
            me.sendLine(print_error(SECURITY_ERROR, ""))