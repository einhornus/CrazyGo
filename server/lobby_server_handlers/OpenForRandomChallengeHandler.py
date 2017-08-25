from Server import *
from utils.Validator import *


from utils.print_utils import *
from utils.db_queries import *

from utils.security import *

from lobby_server_handlers.factory_utils import *

class OpenForRandomChallengeHandler(Handler):
    def get_validator(self):
        res = Validator(['id', 'game', 'settings'])
        return res

    def get_type(self):
        return 'open_for_random_challenge'


    def pair(self, factory):
        map = {}
        for i in range(len(factory.open_people)):
            id, game, settings = factory.open_people[i]
            if (game, settings) in map:
                map[(game, settings)].append(id)
            else:
                map[(game, settings)] = [id]
        for (gameTitle, settings) in map:
            array = map[(gameTitle, settings)]
            print('pairing: ',array)
            for i in range(len(array)//2):
                firstPlayer = array[2*i]
                secondPlayer = array[2*i+1]
                game = GameChallenge(len(factory.games), gameTitle, settings, 2)
                game.users.append(firstPlayer)
                game.users.append(secondPlayer)
                if not hasattr(factory, "games"):
                    factory.games = []
                factory.games.append(game)
                client1 = findClient(factory, game.users[0])
                client2 = findClient(factory, game.users[1])
                client1.sendLine("go_to_game "+str(game))
                client2.sendLine("go_to_game "+str(game))
                factory.open_people.remove((firstPlayer, gameTitle, settings))
                factory.open_people.remove((secondPlayer, gameTitle, settings))


    def action(self, request, factory, me):
        id = int(request["id"])
        game = request["game"]
        settings = request["settings"]

        if isAuthorized(me):
            if not hasattr(me.factory, "open_people"):
                me.factory.open_people = []
            me.factory.open_people.append((id, game, settings))
        else:
            me.sendLine(print_error(SECURITY_ERROR, ""))
        self.pair(factory)
