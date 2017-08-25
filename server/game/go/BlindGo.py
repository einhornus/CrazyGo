from game.go.GoBase import *
import game.go.GamePhase
import game.go.CountingPhase
from game.go.RegularGo import *


class BlindGo(RegularGo):
    def _get_title(self):
        return "blind-go"