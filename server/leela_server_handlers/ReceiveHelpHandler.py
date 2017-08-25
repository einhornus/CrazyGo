from Server import *
from utils.Validator import *
import math

import leela_server_handlers.leela_utils

class ReceiveHelpHandler(Handler):
    def get_validator(self):
        res = Validator(['index', 'best_move', 'variations'])
        return res

    def get_type(self):
        return 'receive_help'

    def action(self, request, factory, me):
        index = request['index']
        bestMove = request['best_move']
        variations = request['variations']
        for i in range(len(factory.need_help)):
            if factory.need_help[i][1] == index:
                client = factory.need_help[i][0]
                clientIndex = factory.need_help[i][2]
                client.sendLine("best_move " + clientIndex + ";" + str(bestMove))
                vars = variations.split('%')
                if variations != "":
                    for var in vars:
                        comps = var.split('*')
                        move = comps[0]
                        winrate = comps[1]
                        iters = comps[2]
                        pv = comps[3]
                        client.sendLine("variation " + clientIndex + ";" + str(move) + ";" + str(winrate) + ";" + str(iters) + ";" + pv)
                client.sendLine("end " + clientIndex)
