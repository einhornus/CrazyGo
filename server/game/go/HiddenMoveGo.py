from game.go.RegularGo import *
from game.go.GoBase import *
import game.go.GamePhase
import game.go.HiddenMoveGamePhase
import game.go.CountingPhase
import game.go.SetHiddenMovesPhase


class HiddenMoveGo(RegularGo):
    def _get_title(self):
        return "hidden-move-go"

    def _set_initial_phase(self):
        self.set_phase("setup")

    def _get_available_phases(self, host):
        p1 = game.go.HiddenMoveGamePhase.HiddenMoveGoGamePhase(host)
        p2 = game.go.CountingPhase.CountingPhase(host)
        p3 = game.go.SetHiddenMovesPhase.SetHiddenMovesPhase(host)
        return {"game":p1, "counting":p2, "setup":p3}