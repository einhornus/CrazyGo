def setup_player_permission(self, index):
    self._open_permission_callback(index)
    for i in range(self.n_players):
        if i != index:
            self._close_permission_callback(i)
    self._state_callback(self.get_state())

def open_permissions_for_all(self):
    for i in range(self.n_players):
        self._open_permission_callback(i)

def next_player(self):
    self.current_move+=1
    if self.current_move == self.n_players:
        self.current_move = 0
    self._open_permission_callback(self.current_move)
    for i in range(self.n_players):
        if i != self.current_move:
            self._close_permission_callback(i)
    self.time_remaining = self.time
    self._time_callback(self.current_move, self.time_remaining)
