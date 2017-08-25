import game.go.Phase
import game.go.GamePhase
import abc
import game.go.go_logic_utils
import game.go.go_base_utils
import random
import game.go.Time

class HiddenMoveGoGamePhase(game.go.GamePhase.GamePhase):
    hidden_moves = []
    hidden_moves_randomized = False

    def randomizeHiddenMoves(self):
        hidden_move_count = int(self.host.settings['hm_count'])
        moves = []
        while(len(moves) < 2*hidden_move_count):
            x = random.randint(0, self.host.n-1)
            y = random.randint(0, self.host.n-1)
            for j in range(len(moves)):
                if moves[j][0] == x and moves[j][1] == y:
                    break
            else:
                moves.append((x, y))

        for i in range(hidden_move_count):
            self.add_hidden_move(moves[i][0], moves[i][1], 0)

        for i in range(hidden_move_count):
            self.add_hidden_move(moves[hidden_move_count+i][0], moves[hidden_move_count+i][1], 1)
        self.hidden_moves_randomized = True

    def activate(self):
        time_settings = self.host.settings["time"]
        self.times_list = [game.go.Time.Time.create(time_settings) for i in range(self.host.n_players)]
        self.hidden_moves = []
        self.hidden_moves_randomized = []
        self.current_move = self.host.get_first_player()
        """
        if self.host.settings['randomize'] == "1":
            if not self.hidden_moves_randomized:
                self.randomizeHiddenMoves()
        """

    def cont(self):
        self.host._state_callback(self.host.get_state())
        game.go.go_base_utils.setup_player_permission(self.host, self.host.get_first_player())
        self.host._event_callback('current|' + str(self.current_move))



    def filterMoves(self):
        newlist = []
        for i in range(len(self.hidden_moves)):
            cnt = 0
            for j in range(len(self.hidden_moves)):
                if self.hidden_moves[j][0] == self.hidden_moves[i][0] and self.hidden_moves[j][1] == self.hidden_moves[i][1]:
                    cnt+=1
            if cnt > 1:
                self.host.board[self.hidden_moves[i][0]][self.hidden_moves[i][1]] = 9
                continue
            else:
                newlist.append(self.hidden_moves[i])
        self.hidden_moves = newlist

    def add_hidden_move(self, x, y, color):
        self.host.board[x][y] = color
        self.hidden_moves.append((x, y, color))

    def get_special_state_part(self, obj):
        super(HiddenMoveGoGamePhase, self).get_special_state_part(obj)
        obj.hidden_moves = []
        for i in range(len(self.hidden_moves)):
            x__, y__, color = self.hidden_moves[i]
            sss = str(x__)+"#"+str(y__)+"#"+str(color)
            obj.hidden_moves.append(sss)

    def is_move_legal(self, move, index):
        if self.current_move != index:
            return False
        if move == 'pass':
            return True
        try:
            x_, y_ = move.split('-')
            x = int(x_)
            y = int(y_)

            for i in range(len(self.hidden_moves)):
                x__, y__, color = self.hidden_moves[i]
                if x__ == x and y__ == y and color != index:
                    return True

            board = self.host.board
            history = self.board_states_history
            if board[x][y] == game.go.go_logic_utils.empty_color:
                return game.go.go_logic_utils.is_placement_legal(history, board, x, y, index)
            else:
                return False
        except BaseException:
            return False

    def reveal_hidden_move(self, x, y):
        for i in range(len(self.hidden_moves)):
            x__, y__, color = self.hidden_moves[i]
            if x__ == x and y__ == y:
                self.hidden_moves.pop(i)
                self.host._event_callback("reveal|")
                self.host._state_callback(self.host.get_state())
                return


    def make_move(self, move, index):
        if move != 'pass':
            x_, y_ = move.split('-')
            x = int(x_)
            y = int(y_)
            for i in range(len(self.hidden_moves)):
                x__, y__, color = self.hidden_moves[i]
                if x__ == x and y__ == y and color != index:
                    self.reveal_hidden_move(x, y)
                    self.host._state_callback(self.host.get_state())
                    game.go.go_base_utils.setup_player_permission(self.host, self.current_move)
                    return
        self._make_move(move, index)
