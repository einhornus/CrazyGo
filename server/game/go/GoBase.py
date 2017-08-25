from game.GameState import *
import game.go.go_logic_utils
import game.go.go_base_utils
import abc
import game.go.GamePhase
import game.go.CountingPhase
import random
import game.go.handicap
import utils.db_queries
import types


class GoBase(GameState):
    def get_rank(self, index):
        id = self.users[index]
        rank = utils.db_queries.get_user_rank(id)
        return rank

    @abc.abstractmethod
    def _get_n_players(self):
        raise NotImplementedError

    @abc.abstractmethod
    def _get_title(self):
        raise NotImplementedError

    @abc.abstractmethod
    def _set_initial_phase(self):
        raise NotImplementedError

    @abc.abstractmethod
    def _get_available_phases(self, host):
        raise NotImplementedError

    def get_komi(self, index):
        handi = int(self.settings["handi"])
        if handi == 0:
            if index == self.get_first_player():
                print('komi', index, 0)
                return 0
            else:
                val = game.go.handicap.get_komi_value(0, self.n)
                print('komi', index, val)
                return val
        else:
            if index == self.black_player:
                return 0
            else:
                rank_diff = abs(self.get_rank(0) - self.get_rank(1))
                res = game.go.handicap.get_komi_value(rank_diff, self.n)
                return res

    def resign(self, index):
        if index == 0:
            self.terminate("r#0#1")
        else:
            self.terminate("r#1#0")

    fpl = -1

    def init_attrs(self):
        self.fill_handi()
        self.komi1 = self.get_komi(0)
        self.komi2 = self.get_komi(1)
        self.get_first_player()


    def get_first_player(self):
        handi = int(self.settings["handi"])
        if handi == 0:
            self.fpl = self.black_player
        else:
            rank0 = self.get_rank(0)
            rank1 = self.get_rank(1)
            if rank0 == rank1:
                self.fpl = random.randint(0, self.n_players-1)
            else:
                han = abs(rank0 - rank1)
                skilled_one = 0
                if rank1 > rank0:
                    skilled_one = 1
                fpl_value = game.go.handicap.who_starts(han, self.n, skilled_one)
                self.fpl = fpl_value
        return self.fpl

    def fill_handi(self):
        handi = int(self.settings["handi"])
        if handi == 0:
            self.black_player = random.randint(0, self.n_players-1)
        else:
            rank0 = self.get_rank(0)
            rank1 = self.get_rank(1)
            han = abs(rank0 - rank1)
            stones = game.go.handicap.get_stones(han, self.n)
            self.handi_stones = han

            rank0 = self.get_rank(0)
            rank1 = self.get_rank(1)
            han = abs(rank0 - rank1)
            skilled_one = 0
            if rank1 > rank0:
                skilled_one = 1
            black_player = game.go.handicap.get_black(han, self.n, skilled_one)
            self.black = black_player

            for i in range(len(stones)):
                x = stones[i][0]
                y = stones[i][1]
                self.board[x][y] = black_player
            self.black_player = black_player


    current_phase = None

    def get_attrs(self, object):
        object.black_player = self.black_player
        object.komi1 = self.komi1
        object.komi2 = self.komi2
        object.is_revealed = self.is_revealed
        object.handi = self.handi_stones
        res = ""
        res += str(self.black_player)+"*"
        res += str(self.komi1)+"*"
        res += str(self.komi2)+"*"
        res += str(self.is_revealed)+"*"
        res += str(self.handi_stones)
        return res


    def init(self, factory):
        super(GoBase, self).init(factory)

        self.time = 60
        self.komi1 = 0
        self.komi2 = 0
        self.black_player = 0
        self.is_revealed = 0
        self.handi_stones = 0

        self.title = self._get_title()
        self.n_players = self._get_n_players()
        self.n = int(self.settings['board_size'])
        self.board = [[game.go.go_logic_utils.empty_color for i in range(self.n)] for j in range(self.n)]
        self.phases = self._get_available_phases(self)
        self.init_attrs()

    def set_phase(self, title):
        self._event_callback('fpl|' + str(self.fpl))
        self._event_callback('phase|'+title)
        self.current_phase = self.phases[title]
        self.current_phase.activate()

    def on_reconnect(self):
        self._state_callback(self.get_state())
        self.current_phase.on_reconnect()

    def get_initial_response(self):
        self._set_initial_phase()
        self._state_callback(self.get_state())

    def is_move_legal(self, move, index):
        if move == 'resign':
            return True
        return self.current_phase.is_move_legal(move, index)

    def on_time(self):
        if not(self.current_phase is None):
            self.current_phase.on_time()

    def make_move(self, move, index):
        if move == 'resign':
            self.resign(index)
            return
        self.current_phase.make_move(move, index)

    def count_result(self):
        points = []
        winner_index = -1
        for i in range(self.n_players):
            player_points = game.go.go_logic_utils.count_result(self.n_players, self.board, self.current_phase.life_table, i)
            player_points += self.get_komi(i)
            points.append(player_points)
            if player_points == max(points):
                winner_index = i
        self.terminate("p#"+str(points[0])+"#"+str(points[1]))

    def get_board(self):
        res = ""
        for i in range(self.n):
            for j in range(self.n):
                res += str(self.board[i][j])
        return res

    def get_state(self):
        obj = types.SimpleNamespace()
        self.get_attrs(obj)
        self.current_phase.get_special_state_part(obj)
        obj.phase = self.current_phase.get_phase_title()
        obj.n = self.n
        obj.title = self._get_title()
        obj.board = self.get_board()
        obj.fpl = self.fpl
        self.get_attrs(obj)
        res_str = json.dumps(obj.__dict__)
        res_str = res_str.replace('\n', "")
        res_str = res_str.replace(" ", "")
        return res_str
