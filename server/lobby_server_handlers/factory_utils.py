from utils.db import *
from utils.GameChallenge import *
from utils.server_factory_utils import *

def getAllIdsString(self):
    if not hasattr(self, 'clientProtocols'):
        self.clientProtocols = []

    res = ""
    for client in self.clientProtocols:
        res += str(client.id)+";"
    return res

def getAllGamesString(self):
    if not hasattr(self, 'games'):
        self.games = []
    res = ""
    print("LEEEN", len(self.games))
    for game in self.games:
        res += str(game)+";"
    return res

def lostConnectionFunc(self):
    if not hasattr(self.factory, 'clientProtocols'):
        self.factory.clientProtocols = []
    if self in self.factory.clientProtocols:
        self.factory.clientProtocols.remove(self)
    sendMessageToAllClients(self.factory, '- '+str(self.id))
    if not hasattr(self.factory, "open_people"):
        self.factory.open_people = []
    cleanup_random_challenges(self, self.factory, self)
    leaveAllGames(self.factory, self)


def cleanup_random_challenges(self, factory, me):
    if not hasattr(factory, "open_people"):
        factory.open_people = []
    for i in range(len(factory.open_people)):
        new_open = []
        if factory.open_people[i][0] != me.id:
            new_open.append(factory.open_people[i])
        factory.open_people = new_open
    print(factory.open_people)

def leaveAllGames(factory, me):
    if not hasattr(factory, 'games'):
        factory.games = []
    games_to_leave = []
    for game in factory.games:
        if me.id in game.users and game.status == 'open':
            games_to_leave.append(game)
    for game in games_to_leave:
        leaveGame(me, game)


def leaveGame(self, game):
    my_index = -1
    for i in range(len(game.users)):
        current_id = game.users[i]
        if current_id == self.id:
            my_index = i
    game.users.pop(my_index)
    if my_index == -1:
        self.sendLine("you_did_not_entered_the_game ")
    else:
        sendMessageToAllClients(self.factory, "game- "+str(game.id)+";"+str(self.id))
        if len(game.users) == 0:
            print('leave')
            self.factory.games.remove(game)
            sendMessageToAllClients(self.factory, "game_removed "+str(game.id))