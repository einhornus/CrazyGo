import game.go.Phase
import abc
import game.go.go_logic_utils
import game.go.go_base_utils
import game.go.Time

class GamePhase(game.go.Phase.Phase):
    def __init__(self, host):
        self.host = host
        self.current_move = self.host.get_first_player()
        self.moves_history = []
        self.board_states_history = []
        self.last_move = "nada"
        self.move_list = []
        self.times_list = []
        self.reveals = [3, 3]
        self.asking = False

    def get_moves(self):
        res = ""
        for i in range(len(self.move_list)):
            res += self.move_list[i]
            if i != len(self.move_list) - 1:
                res+='#'
        return res

    def get_phase_title(self):
        return "game"

    def get_special_state_part(self, obj):
        obj.last_move = str(self.last_move)
        obj.moves = self.move_list
        if self.asking:
            obj.asking = 1

        else:
            obj.asking = 0
        obj.rev1 = self.reveals[0]
        obj.rev2 = self.reveals[1]
        obj.current = self.current_move
        obj.time1 = self.times_list[0].__str__()
        obj.time2 = self.times_list[1].__str__()

    def is_move_legal(self, move, index):
        if self.asking:
            if (move == 'yes') or (move == 'no'):
                return True
            else:
                return False
        else:
            if self.reveals[self.current_move]>0:
                if move == 'ask':
                    return True
            if self.current_move != index:
                return False
            if move == 'pass':
                return True
            try:
                x_, y_ = move.split('-')
                x = int(x_)
                y = int(y_)
                if self.host.board[x][y] == 9:
                    return game.go.go_logic_utils.is_placement_legal(self.board_states_history, self.host.board, x, y, index)
                else:
                    return False
            except BaseException:
                return False

    def handle_pass(self, index):
        self.host._event_callback("pass|" + str(index))
        haveAllPassed = True
        for i in range(self.host.n_players-1):
            if len(self.moves_history) - 1 - i >= 0:
                if self.moves_history[-(i+1)] != 'pass':
                    haveAllPassed = False
                    break
            else:
                haveAllPassed = False
                break

        if haveAllPassed:
            self.host.set_phase("counting")
        return haveAllPassed

    def activate(self):
        time_settings = self.host.settings["time"]
        self.times_list = [game.go.Time.Time.create(time_settings) for i in range(self.host.n_players)]
        self.current_move = self.host.get_first_player()
        game.go.go_base_utils.setup_player_permission(self.host, self.host.get_first_player())
        self.host._event_callback('current|' + str(self.current_move))

    def on_time(self):
        time_is_up = not self.times_list[self.current_move].tick()
        if time_is_up:
            if self.current_move == 0:
                self.host.terminate("t#0#1")
            else:
                self.host.terminate("t#1#0")
        self.host._event_callback('time|' + self.times_list[0].__str__()+"#"+self.times_list[1].__str__())


    def on_reconnect(self):
        game.go.go_base_utils.setup_player_permission(self.host, self.current_move)

    def next_player(self):
        self.current_move += 1
        if self.current_move >= self.host.n_players:
            self.current_move = 0
        self.host._event_callback('current|' + str(self.current_move))
        game.go.go_base_utils.setup_player_permission(self.host, self.current_move)

    def make_move(self, move, index):
        self._make_move(move, index)

    def _make_move(self, move, index):
        self.times_list[self.current_move].move()
        if move == 'ask':
            self.reveals[self.current_move]-=1
            self.asking = True
            self.host._event_callback('ask|' + str(self.current_move)+""+str(1-self.current_move))
            self.host._state_callback(self.host.get_state())
            self.next_player()
            return
        if move == 'yes':
            self.asking = False
            self.host.is_revealed = 1
            self.host._state_callback(self.host.get_state())
            self.next_player()
            return
        if move == 'no':
            self.asking = False
            self.host._state_callback(self.host.get_state())
            self.next_player()
            return
        if move != 'pass':
            x_, y_ = move.split('-')
            x = int(x_)
            y = int(y_)
            new_board = game.go.go_logic_utils.put_stone(self.host.board, x, y, index)
            self.host.board = new_board
            self.board_states_history.append(game.go.go_logic_utils.hash_board(self.host.board))
            self.host._event_callback('move|' + move)
            self.move_list.append(move)
            self.last_move = move
        else:
            self.move_list.append('pass')
            self.handle_pass(index)
        self.moves_history.append(move)

        if self.host.current_phase.get_phase_title() == "game":
            self.host._state_callback(self.host.get_state())
            self.next_player()
            game.go.go_logic_utils.print_board(self.host.board)