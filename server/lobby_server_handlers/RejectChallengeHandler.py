
from Server import *
from utils.Validator import *
from utils.print_utils import *
from utils.db_queries import *

from utils.security import *

from lobby_server_handlers.factory_utils import *

class RejectChallengeHandler(Handler):
    def get_validator(self):
        res = Validator(['id', 'from_id', 'title', 'settings', 'challenge_id'])
        return res

    def get_type(self):
        return 'reject_challenge'

    def action(self, request, factory, me):
        id = int(request["id"])
        fromId = int(request["from_id"])
        title = request["title"]
        settings = request["settings"]
        challenge_id = int(request['challenge_id'])
        if isAuthorized(me):
            meClient = findClient(factory, id)
            if not hasattr(me.factory, "stored_challenges"):
                me.factory.stored_challenges = {}
            if challenge_id in me.factory.stored_challenges:
                me.factory.stored_challenges.pop(challenge_id)
            else:
                me.sendLine(print_error(SECURITY_ERROR, ""))
                return

            fromClient = findClient(factory, fromId)
            fromClient.sendLine('reject_challenge '+str(id)+';'+str(title)+";"+str(settings)+";"+str(challenge_id))
        else:
            me.sendLine(print_error(SECURITY_ERROR, ""))