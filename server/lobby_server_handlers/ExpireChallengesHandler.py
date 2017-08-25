
from Server import *
from utils.Validator import *
from utils.print_utils import *
from utils.db_queries import *

from utils.security import *
import settings

from lobby_server_handlers.factory_utils import *

class ExpireChallengesHandler(Handler):
    def get_validator(self):
        res = Validator(["id", "challenge_id"])
        return res

    def get_type(self):
        return 'expire'

    def action(self, request, factory, me):
        if not hasattr(me.factory, "stored_challenges"):
            me.factory.stored_challenges = {}

        if isAuthorized(me):
            me_id = int(request["id"])
            challenge_id = int(request["challenge_id"])
            if challenge_id in me.factory.stored_challenges:
                id, challengerId, title, settings = me.factory.stored_challenges[challenge_id]
                if me_id == id:
                    challengerClient = findClient(factory, challengerId)
                    challengerClient.sendLine('expired_challenge '+str(challenge_id))
                    me.factory.stored_challenges.pop(challenge_id)
                else:
                    me.sendLine(print_error(SECURITY_ERROR, ""))
            else:
                pass
                #me.sendLine(print_error(SECURITY_ERROR, ""))
        else:
            me.sendLine(print_error(SECURITY_ERROR, ""))