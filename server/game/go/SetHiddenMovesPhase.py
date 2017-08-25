import game.go.Phase
import abc
import game.go.go_logic_utils
import game.go.go_base_utils
import game.go.Time

class SetHiddenMovesPhase(game.go.Phase.Phase):
    def __init__(self, host):
        self.host = host
        self.TIME = 10
        self.times_list = []
        self.hidden_moves = []
        self.agreed_table = [False for i in range(self.host.n_players)]

    def get_phase_title(self):
        return "setup"

    def get_hidden_moves_count(self, color):
        res = 0
        for i in range(len(self.hidden_moves)):
            if self.hidden_moves[i][2] == color:
                res += 1
        return res

    def hm_index(self, x, y, color):
        for i in range(len(self.hidden_moves)):
            if self.hidden_moves[i][2] == color:
                if self.hidden_moves[i][0] == x and self.hidden_moves[i][1] == y:
                    return i
        return -1

    def get_hm(self):
        if self.host.n == 9:
            return 2
        if self.host.n == 13:
            return 4
        if self.host.n == 19:
            return 8

    def get_special_state_part(self, obj):
        obj.hidden_moves = []
        obj.hm_count = self.get_hm()
        for i in range(len(self.hidden_moves)):
            x__, y__, color = self.hidden_moves[i]
            sss = str(x__)+"#"+str(y__)+"#"+str(color)
            obj.hidden_moves.append(sss)

    def is_move_legal(self, move, index):
        if move == 'go':
            count = self.get_hidden_moves_count(index)
            if count != int(self.get_hm()):
                return False
            else:
                return True
        try:
            x_, y_ = move.split('-')
            x = int(x_)
            y = int(y_)
            return True
        except BaseException:
            return False

    def activate(self):
        self.times_list = [game.go.Time.Absolute(self.TIME) for i in range(self.host.n_players)]
        self.host._state_callback(self.host.get_state())
        game.go.go_base_utils.open_permissions_for_all(self.host)

    def are_all_agreed(self):
        for i in range(len(self.agreed_table)):
            if not self.agreed_table[i]:
                return False
        return True

    def on_reconnect(self):
        game.go.go_base_utils.open_permissions_for_all(self.host)

    def on_time(self):
        if len(self.times_list) == 0:
            return
        for i in range(self.host.n_players):
            if not self.agreed_table[i]:
                time_is_up = self.times_list[i].tick()
                if time_is_up:
                    if i == 0:
                        self.host.terminate("t#0#1")
                    else:
                        self.host.terminate("t#1#0")
        self.host._event_callback('time|' + self.times_list[0].__str__()+"#"+self.times_list[1].__str__())

    def make_move(self, move, index):
        if move == 'go':
            self.agreed_table[index] = True
            if self.are_all_agreed():
                self.host.set_phase("game")
                for i in range(len(self.hidden_moves)):
                    self.host.current_phase.add_hidden_move(self.hidden_moves[i][0], self.hidden_moves[i][1], self.hidden_moves[i][2])
                self.host.current_phase.filterMoves()
                self.host.current_phase.cont()
        else:
            x_, y_ = move.split('-')
            x = int(x_)
            y = int(y_)
            ind = self.hm_index(x, y, index)
            if ind == -1:
                self.hidden_moves.append((x, y, index))
            else:
                self.hidden_moves.pop(ind)
            self.host._state_callback(self.host.get_state())