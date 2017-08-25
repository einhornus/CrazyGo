from game.GameState import *


class GomokuGame(GameState):
    row_size = 5
    n = 2
    time = 60
    time_remaining = time

    def init(self, factory):
        super(GomokuGame, self).init(factory)
        self.title = 'gomoku'
        self.n = int(self.settings)
        self.board = [[0 for i in range(self.n)] for j in range(self.n)]
        self.current_move = 1

    def get_initial_response(self):
        self._open_permission_callback(0)
        self._close_permission_callback(1)
        self._state_callback(self.get_state())

    def is_move_legal(self, move, index):
        if self.current_move - 1 != index:
            return False
        x_, y_ = move.split('-')
        x = int(x_)
        y = int(y_)
        if self.board[x][y] == 0:
            return True
        else:
            return False

    def resign(self, index):
        if index == 0:
            self.terminate("second")
        else:
            self.terminate("first")

    def on_time(self):
        if not self.stopped:
            self.time_remaining -= 1
            self._time_callback(self.current_move - 1, self.time_remaining)
            if self.time_remaining <= 0:
                self.resign(self.current_move - 1)

    def make_move(self, move, index):
        x_, y_ = move.split('-')
        x = int(x_)
        y = int(y_)
        self.board[x][y] = self.current_move
        self.current_move = 3 - self.current_move

        cw = self.check_win()
        self._state_callback(self.get_state())
        self._open_permission_callback(self.current_move - 1)
        self._close_permission_callback((3-self.current_move) - 1)
        self.time_remaining = self.time
        self._time_callback(self.current_move - 1, self.time_remaining)
        if cw != 'playing':
            self.terminate(cw)


    def check_win(self):
        directions = [[1, 0], [1, 1], [0, 1], [-1, 1], [-1, 0], [-1, -1], [0, -1], [1, -1]]
        res = 'draw'
        for i in range(self.n):
            for j in range(self.n):
                if self.board[i][j] == 0:
                    res = 'playing'
                for direction in directions:
                    is_straight = True
                    dx = direction[0]
                    dy = direction[1]
                    for q in range(self.row_size):
                        x = i+dx*q
                        y = j+dy*q
                        if x >= 0 and x < self.n and y>=0 and y<self.n:
                            if self.board[x][y] == self.board[i][j]:
                                pass
                            else:
                                is_straight = False
                                break
                        else:
                            is_straight = False
                            break
                    if is_straight:
                        if self.board[i][j] == 1:
                            return 'first'
                        if self.board[i][j] == 2:
                            return 'second'
        return res

    def get_state(self):
        res = ""
        for i in range(self.n):
            for j in range(self.n):
                res += str(self.board[i][j])
        return res
