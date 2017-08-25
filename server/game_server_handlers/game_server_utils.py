from utils.db import *
from utils.security import *
from utils.GameChallenge import *
from game.GameState import *
from utils.server_factory_utils import *
from game.Gomoku import *
import game.go.GoBase
import game.go.RegularGo
import game.go.OneColorGo
import game.go.BlindGo
import game.go.HiddenMoveGo



def userEntered(self, id):
    self.id = id
    self.factory.clientProtocols.append(self)

def add_observer(game, id):
    print("ADD OBSERVER", game.get_state(), id)
    game.observers.append(id)
    game._state_callback(game.get_state())

def start_game(factory, game):
    send_to_players_and_observers(factory, game, 'game ')
    game.get_initial_response()

def init_game(factory, game_id, title, settings, users):
    new_game = create_game(users, title, settings)
    if not hasattr(factory, "games"):
        factory.games = {}
    factory.games[game_id] = new_game
    new_game.lobby_id = game_id
    new_game.init(factory)
    return new_game

def is_game_exists(factory, game_id):
    if not hasattr(factory, 'games'):
        factory.games = {}
    return (game_id in factory.games)

def place_application(factory, game_id, index, user_id):
    if not hasattr(factory, 'applications'):
        factory.applications = []
    factory.applications.append((game_id, index, user_id))

def handle_application(factory, game, id):
    if id in game.users:
        index = game.users.index(id)
        if game.user_flags[index]:
            print('empty authorize')
            game.active_players.append(id)
            game.on_reconnect()
            return
    all_users_came = game.user_came(id)
    if all_users_came:
        start_game(factory, game)

def create_game(users, title, settings):
    if title == 'gomoku':
        res = GomokuGame(users, settings)
        return res
    if title == 'go':
        res = game.go.RegularGo.RegularGo(users, settings)
        return res
    if title == 'one-color-go':
        res = game.go.OneColorGo.OneColorGo(users, settings)
        return res
    if title == 'blind-go':
        res = game.go.BlindGo.BlindGo(users, settings)
        return res
    if title == 'hidden-move-go':
        res = game.go.HiddenMoveGo.HiddenMoveGo(users, settings)
        return res
    print("Can't find a game")
    return None

def terminate_game(factory, game, result):
    users = ""
    for i in range(len(game.users)):
        users += str(game.users[i])
        if i != len(game.users) - 1:
            users += "?"
    message = 'terminate '+game.title+';'+result+";"+users+";"+str(game.settings)+";"+str(game.lobby_id)
    if hasattr(factory, 'slave'):
        factory.slave.sendLine(message)
    for i in range(len(game.users)):
        game._close_permission_callback(i)
    factory.games.pop(game.lobby_id)