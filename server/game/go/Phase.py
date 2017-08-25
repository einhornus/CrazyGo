import abc
class Phase():
    def get_phase_title(self):
        raise NotImplementedError

    def get_special_state_part(self, obj):
        raise NotImplementedError

    def is_move_legal(self, move, index):
        raise NotImplementedError

    def make_move(self, move, index):
        raise NotImplementedError

    def activate(self):
        raise NotImplementedError

    def on_reconnect(self):
        raise NotImplementedError


