from game.go.GoBase import *
import game.go.GamePhase
import game.go.CountingPhase
import game.go.handicap

class RegularGo(GoBase):
    def _get_n_players(self):
        return 2

    def _get_title(self):
        return "go"

    def _set_initial_phase(self):
        self.set_phase("game")

    def _get_available_phases(self, host):
        p1 = game.go.GamePhase.GamePhase(host)
        p2 = game.go.CountingPhase.CountingPhase(host)
        return {"game":p1, "counting":p2}
