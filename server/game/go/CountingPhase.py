import game.go.Phase
import abc
import game.go.go_logic_utils
import game.go.go_base_utils
import game.go.Time

class CountingPhase(game.go.Phase.Phase):
    def __init__(self, host):
        self.host = host
        self.TIME = 300
        self.times_list = []
        self.life_table = [[True for i in range(host.n)] for j in range(host.n)]
        self.agreed_table = [False for i in range(host.n_players)]

    def get_phase_title(self):
        return "counting"

    def get_special_state_part(self, obj):
        res = ""
        lt = ""
        for i in range(self.host.n):
            for j in range(self.host.n):
                if self.life_table[i][j]:
                    lt += "1"
                else:
                    lt += "0"
        res += lt+ "|"
        tt = ""
        territory_table = game.go.go_logic_utils.get_territory_table(self.life_table, self.host.board)
        for i in range(self.host.n):
            for j in range(self.host.n):
                    tt += str(territory_table[i][j])
        res += tt+"|"

        obj.territory_table = tt
        obj.life_table = lt
        obj.current = -1
        obj.time1 = self.times_list[0].__str__()
        obj.time2 = self.times_list[1].__str__()
        return res

    def is_move_legal(self, move, index):
        if move == 'agree' or move == 'decline':
            return True
        try:
            x_, y_ = move.split('-')
            x = int(x_)
            y = int(y_)
            if x < 0 or x >= self.host.n:
                return False
            if y < 0 or y >= self.host.n:
                return False
            return True
        except BaseException:
            return False

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

    def activate(self):
        self.times_list = [game.go.Time.Absolute(self.TIME) for i in range(self.host.n_players)]
        self.life_table = [[True for i in range(self.host.n)] for j in range(self.host.n)]
        self.agreed_table = [False for i in range(self.host.n_players)]
        self.host._state_callback(self.host.get_state())
        game.go.go_base_utils.open_permissions_for_all(self.host)

    def are_all_agreed(self):
        for i in range(len(self.agreed_table)):
            if hasattr(self, 'has_resigned'):
                if self.has_resigned[i]:
                    continue
            if not self.agreed_table[i]:
                return False
        return True

    def make_move(self, move, index):
        if move == 'agree' or move == 'decline':
            if move == 'agree':
                self.agreed_table[index] = True
                if self.are_all_agreed():
                    self.host.count_result()
            if move == 'decline':
                self.host.set_phase("game")
                self.host._state_callback(self.host.get_state())
                game.go.go_base_utils.setup_player_permission(self.host, self.host.get_first_player())
        else:
            for i in range(self.host.n_players):
                self.times_list[index].reset()
            x_, y_ = move.split('-')
            x = int(x_)
            y = int(y_)
            self.life_table = game.go.go_logic_utils.toggle_life_table(self.life_table, self.host.board, x, y)
            self.host._state_callback(self.host.get_state())
            for i in range(self.host.n_players):
                self.agreed_table[i] = False
            self.host._state_callback(self.host.get_state())
            game.go.go_base_utils.open_permissions_for_all(self.host)

    def on_reconnect(self):
        game.go.go_base_utils.open_permissions_for_all(self.host)