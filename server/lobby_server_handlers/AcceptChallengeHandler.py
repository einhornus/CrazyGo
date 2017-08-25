
from Server import *
from utils.Validator import *
from utils.print_utils import *
from utils.db_queries import *

from utils.security import *
import random

from lobby_server_handlers.factory_utils import *

class AcceptChallengeHandler(Handler):
    def get_validator(self):
        res = Validator(['id', 'from_id', 'title', 'settings', 'challenge_id'])
        return res

    def get_type(self):
        return 'accept_challenge'

    def action(self, request, factory, me):
        id = int(request["id"])
        fromId = int(request["from_id"])
        title = request["title"]
        settings = request["settings"]
        challenge_id = int(request["challenge_id"])
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
            gid = random.randint(0, 1000000000)
            game = GameChallenge(gid, title, settings, 2)
            game.users.append(fromId)
            game.users.append(id)
            if not hasattr(factory, "games"):
                factory.games = []
            factory.games.append(game)
            sendMessageToAllClients(factory, "new_game "+str(game))
            game.status = 'closed'
            sendMessageToAllClients(factory, "game_closed " + str(game.id))
            fromClient.sendLine("go_to_game "+str(game))
            meClient.sendLine("go_to_game "+str(game))
        else:
            me.sendLine(print_error(SECURITY_ERROR, ""))