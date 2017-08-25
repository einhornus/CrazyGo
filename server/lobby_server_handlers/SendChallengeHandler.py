
from Server import *
from utils.Validator import *
from utils.print_utils import *
from utils.db_queries import *

from utils.security import *
import settings
import random

from lobby_server_handlers.factory_utils import *

class SendChallengeHandler(Handler):
    def get_validator(self):
        res = Validator(['id', 'challenger_id', 'title', 'settings'])
        return res

    def get_type(self):
        return 'challenge'

    def action(self, request, factory, me):
        id = int(request["id"])
        challengerId = int(request["challenger_id"])
        title = request["title"]
        settings = request["settings"]
        if isAuthorized(me):
            challengerClient = findClient(factory, challengerId)
            if not hasattr(me.factory, "stored_challenges"):
                me.factory.stored_challenges = {}
            challenge_id = random.randint(100, 1000000000)
            me.factory.stored_challenges[challenge_id] = (id, challengerId, title, settings)
            meClient = findClient(factory, id)
            challengerClient.sendLine('new_challenge '+str(id)+';'+str(title)+";"+str(settings)+";"+str(challenge_id))
            meClient.sendLine('created_challenge '+str(challenge_id))
        else:
            me.sendLine(print_error(SECURITY_ERROR, ""))