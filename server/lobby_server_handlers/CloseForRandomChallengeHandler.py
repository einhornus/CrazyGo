from Server import *
from utils.Validator import *


from utils.print_utils import *
from utils.db_queries import *

from utils.security import *

from lobby_server_handlers.factory_utils import *

class CloseForRandomChallengeHandler(Handler):
    def get_validator(self):
        res = Validator(['id', 'game', 'settings'])
        return res

    def get_type(self):
        return 'close_for_random_challenge'

    def action(self, request, factory, me):
        id = int(request["id"])
        game = request["game"]
        settings = request["settings"]

        if isAuthorized(me):
            if not hasattr(me.factory, "open_people"):
                me.factory.open_people = []
            me.factory.open_people.remove((id, game, settings))
            print(me.factory.open_people)
        else:
            me.sendLine(print_error(SECURITY_ERROR, ""))
