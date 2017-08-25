import game_server_handlers.game_server_utils
import json
import abc
import game.Settings

class GameState(metaclass=abc.ABCMeta):
    def __init__(self, users, settings):
        self.users = users
        self.settings = game.Settings.Settings(settings)
        self.max = len(users)
        self.observers = []
        self.active_players = []
        self.stopped = False
        self.user_flags = [False for i in range(len(self.users))]
        self.permissions = [False for i in range(len(self.users))]
        
    def init(self, factory):
        self.factory = factory
        pass

    def terminate(self, result):
        self._game_ended_callback(result)
        game_server_handlers.game_server_utils.terminate_game(self.factory, self, result)
        self.stopped = True

    @abc.abstractmethod
    def get_initial_response(self):
        raise NotImplementedError

    @abc.abstractmethod
    def make_move(self, move, index):
        raise NotImplementedError

    @abc.abstractmethod
    def is_move_legal(self, move, index):
        raise NotImplementedError

    @abc.abstractmethod
    def on_time(self):
        raise NotImplementedError

    def user_came(self, user_id):
        self.active_players.append(user_id)
        for i in range(len(self.users)):
            if self.users[i] == user_id:
                self.user_flags[i] = True

        for i in range(len(self.user_flags)):
            if self.user_flags[i] == False:
                return False
        return True

    @abc.abstractmethod
    def get_state(self):
        pass

    @abc.abstractmethod
    def on_reconnect(self):
        pass

    @abc.abstractmethod
    def resign(self, index):
        pass

    def _time_callback(self, index, time):
        if not self.stopped:
            game_server_handlers.game_server_utils.send_to_players_and_observers(self.factory, self,  "time "+str(index)+";"+str(time))

    def _close_permission_callback(self, index):
        self.permissions[index] = False
        if not self.stopped:
            game_server_handlers.game_server_utils.send_to_players_and_observers(self.factory, self, "-permission "+str(index))

    def _open_permission_callback(self, index):
        self.permissions[index] = True
        if not self.stopped:
            game_server_handlers.game_server_utils.send_to_players_and_observers(self.factory, self, "+permission "+str(index))

    def _event_callback(self, text):
        if not self.stopped:
            game_server_handlers.game_server_utils.send_to_players_and_observers(self.factory, self, "event "+str(text))

    def _state_callback(self, state):
        if not self.stopped:
            game_server_handlers.game_server_utils.send_to_players_and_observers(self.factory, self, "game_state "+str(state))

    def _game_ended_callback(self, result):
        if not self.stopped:
            game_server_handlers.game_server_utils.send_to_players_and_observers(self.factory, self, "end "+result)

    def __str__(self):
        j = json.dumps(self, default=lambda o: o.__dict__, sort_keys=True, indent=4)
        j = j.replace('\n', '')
        j = j.replace(' ', '')
        return j
