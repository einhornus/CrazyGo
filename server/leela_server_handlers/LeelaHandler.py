from Server import *
from utils.Validator import *
import math
import random

import leela_server_handlers.leela_utils

class LeelaHandler(Handler):
    def get_validator(self):
        res = Validator(['moves', 'index', 'times', 'delegate', 'komi', 'playouts', 'handi', 'size'])
        return res

    def get_type(self):
        return 'leela'

    def merge(self, old, new):
        move1, iters1, winrate1, _pv1 = old
        move2, iters2, winrate2, _pv2 = new

        resMove = move1
        resIters = iters1 + iters2
        w1 = iters1
        w2 = iters2

        resWinrate = 0
        if w1 + w2 == 0:
            resWinrate = (winrate1 + winrate2)/2.0
        else:
            resWinrate = (winrate1 * w1 + winrate2*w2)/(w1+w2)

        resPV = _pv1
        if iters2 > iters1:
            resPV = _pv2

        merged = (resMove, resIters, resWinrate, resPV)
        return merged

    def run(self, moves, times, komi, handi, playouts, size):
        bestMoves = {}
        resContinuations = []
        for i in range(times):
            bestMove, continuations = leela_server_handlers.leela_utils.run(moves, playouts, komi, handi, size)


            for k in range(len(continuations)):
                index = -1
                for j in range(len(resContinuations)):
                    if str(resContinuations[j][0]) == str(continuations[k][0]):
                        index = j
                        break
                if index == -1:
                    resContinuations.append(continuations[k])
                else:
                    oldContinuation = resContinuations[index]
                    newContinuation = continuations[k]
                    result = self.merge(oldContinuation, newContinuation)
                    resContinuations[index] = result

            if bestMove in bestMoves:
                bestMoves[bestMove] += 1
            else:
                bestMoves[bestMove] = 1
        maxVal = -1
        resBestMove = None
        for key in bestMoves:
            if bestMoves[key] > maxVal:
                maxVal = bestMoves[key]
                resBestMove = key

        return resBestMove, resContinuations

    def action(self, request, factory, me):
        moves = request['moves'].split('|')
        index = request['index']
        times = int(request['times'])
        delegate = int(request['delegate'])
        komi = float(request['komi'])
        handi = int(request['handi'])
        playouts = int(request['playouts'])
        size = int(request['size'])

        if delegate == 0:
            l = []
            for i in range(len(moves)):
                if len(moves[i])>0:
                    l.append(leela_server_handlers.leela_utils.Move.parse(moves[i]))
            bestMove, continuations =  leela_server_handlers.leela_utils.run(l, playouts, komi, handi, size)

            limit = 10/(handi+1)
            print(limit)
            if len(continuations) > 0 and continuations[0][1] > 200 and continuations[0][4] < limit:
                bestMove = 'resign'

            me.sendLine("best_move "+index+";"+str(bestMove))
            for i in range(len(continuations)):
                move, iters, winrate, _pv, actual = continuations[i]
                pv = ""
                for i in range(len(_pv)):
                    pv += str(_pv[i])
                    if i != len(_pv)-1:
                        pv += "#"
                me.sendLine("variation " + index + ";" + str(move)+";"+str(winrate)+";"+str(iters)+";"+pv)
            me.sendLine("end "+index)
        else:
            if factory.provider != None:
                ind = str(random.randint(1, 1000000000))
                factory.provider.sendLine("help " + ind + ";" + request['moves'])
                if not hasattr(factory, 'help_need_array'):
                    factory.need_help = []
                factory.need_help.append((me, ind, index))



