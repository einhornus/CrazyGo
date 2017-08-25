import subprocess
import os
import time
import platform
import game.go.handicap

class Move:

    def __init__(self, x, y):
        self.x = x
        self.y = y
        self.chars = ['A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T']

    def __str__(self):
        if self.x == -1 and self.y == -1:
            return "pass"
        sym1 = self.chars[self.x]
        sym2 = str(self.y+1)
        return sym1 + sym2

    @staticmethod
    def parse(string):
        #print('parsing '+string)
        if string == 'pass':
            return Move(-1, -1)
        else:
            x = 0
            mv = Move(0, 0)
            for i in range(len(mv.chars)):
                if mv.chars[i] == string[0]:
                    x = i
            sym1 = x
            sym2 = int(string[1:])-1
            return Move(sym1, sym2)

def run(moves, playouts, komi, handi, size):
    print(platform.system())
    fname = os.getcwd()+"/"+""+"leela_l"
    print(fname)
    if str(platform.system()) == "Windows":
        fname = os.getcwd()+"/"+"leela_w"
    try:
        file = open(fname)
    except IOError as e:
        print(u'не удалось открыть файл')
        print(e)
    else:
        with file:
            pass
            #print(u'нашли файл')

    p = subprocess.Popen([
        fname,
       "-p"+str(playouts)],
        stdout=subprocess.PIPE, stderr=subprocess.STDOUT, stdin=subprocess.PIPE)

    _str = ""
    _str += "boardsize "+str(size)+"\n"
    _str += "komi " + str(komi) + "\n"

    handi_stones = game.go.handicap.get_stones(handi, size)
    handi_moves = []
    for i in range(len(handi_stones)):
        move = Move(handi_stones[i][0], handi_stones[i][1])
        handi_moves.append(move)

    for i in range(len(handi_moves)):
        move = handi_moves[i]
        moveString = "play black " + str(move)
        _str += moveString + "\n"

    starts = game.go.handicap.who_starts(handi, size, 0)
    who_black = game.go.handicap.get_black(handi, size, 0)

    black_starts = 0
    if starts == who_black:
        black_starts = 1


    for i in range(len(moves)):
        move = moves[i]
        moveString = str(move)
        if i%2 == 1-black_starts:
            moveString = "play black "+moveString
        else:
            moveString = "play white " + moveString
        _str += moveString + "\n"

    if len(moves)%2 == 1-black_starts:
        _str += "genmove black\n"
    else:
        _str += "genmove white\n"

    print(_str)

    stdout_data = p.communicate(input=_str.encode())[0]
    result = stdout_data.decode()

    strings = result.split('\n')
    print(strings)

    bestMove = Move.parse("pass")
    continuations = []

    for i in range(len(strings)):
        string = strings[i]
        if len(string)>0:
            if string[0] == '=' and 7 > len(string) > 3:
                result = string[2:]
                print(string, result)
                bestMove = Move.parse(result)

        if "->" in string:
            fu = string.replace("->", ";").replace("(", ";").replace(")", ";").replace(":", ";").replace("%", ";").replace('\r', ';')
            parts = fu.split(';')
            _move = Move.parse(parts[0].strip())
            iters = int(parts[1].strip())
            winrate = float(parts[3].strip())
            actual_winrate = winrate

            if len(moves)%2 == black_starts:
                winrate = 100-winrate

            pv = []
            pvStr = parts[-1].split(' ')
            if platform.system() == "Windows":
                pvStr = parts[-2].split(' ')
            for i in range(len(pvStr)):
                if len(pvStr[i])>0:
                    move = Move.parse(pvStr[i])
                    pv.append(move)

            #print((move.__str__(), iters, winrate))
            continuations.append((_move, iters, winrate, pv, actual_winrate))
    #print(bestMove.__str__())

    return bestMove, continuations