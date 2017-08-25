empty_color = 9

def copy_board(board):
    n = len(board)
    res = [[board[j][i] for i in range(n)] for j in range(n)]
    return res

def hash_board(board):
    res = 0
    for i in range(len(board)):
        for j in range(len(board)):
            res = res * 10 + board[i][j]
    return res

def clear_color(board, index):
    reachable_from_empty_color = \
        find_points_reachable_from_color(board, empty_color)
    for i in range(len(board)):
        for j in range(len(board)):
            if board[i][j] != index:
                if not reachable_from_empty_color[i][j]:
                    board[i][j] = empty_color

def put_stone(board, x, y, index):
    res_board = copy_board(board)
    res_board[x][y] = index
    reachable_from_empty_color = find_points_reachable_from_color(res_board, empty_color)
    for i in range(len(board)):
        for j in range(len(board)):
            if res_board[i][j] != index:
                if not reachable_from_empty_color[i][j]:
                    res_board[i][j] = empty_color

    reachable_from_empty_color = find_points_reachable_from_color(res_board, empty_color)
    for i in range(len(board)):
        for j in range(len(board)):
            if res_board[i][j] == index:
                if not reachable_from_empty_color[i][j]:
                    res_board[i][j] = empty_color
    return res_board


def is_placement_legal(history, board, x, y, index):
    after_placement = put_stone(board, x, y, index)
    hash = hash_board(after_placement)
    for i in range(len(history)):
        if history[i] == hash:
            return False
    return True

def find_points_reachable_from_color(board, color):
    n = len(board)
    visited = [[False for i in range(n)] for j in range(n)]
    for i in range(n):
        for j in range(n):
            if board[i][j] == color:
                if not visited[i][j]:
                    dfs(i, j, visited, board, color, True)
    return visited

def dfs(x, y, visited, board, color, prev):
    visited[x][y] = True
    dx = [0, 1, 0, -1]
    dy = [-1, 0, 1, 0]
    for i in range(len(dx)):
        newX = x+dx[i]
        newY = y+dy[i]
        if 0 <= newX < len(visited):
            if 0 <= newY < len(visited):
                if not visited[newX][newY]:
                    if board[newX][newY] == board[x][y] or (prev and board[x][y] == color):
                        dfs(newX, newY, visited, board, color, prev)

def toggle_life_table(table, board, i, j):
    res = copy_board(table)
    if board[i][j] != empty_color:
        visited = [[False for i in range(len(board))] for j in range(len(board))]
        color = board[i][j]
        dfs(i, j, visited, board, color, False)
        for i in range(len(board)):
            for j in range(len(board)):
                if visited[i][j]:
                    res[i][j] = not res[i][j]
    return res

def get_territory_table(table, board):
    n = len(board)
    res = [[9 for i in range(n)] for j in range(n)]
    count0 = _count_result(2, board, table, 0)
    count1 = _count_result(2, board, table, 1)
    for i in range(len(count0)):
        res[count0[i][0]][count0[i][1]] = 0
    for i in range(len(count1)):
        res[count1[i][0]][count1[i][1]] = 1
    return res

def _count_result(n_players, board, life_table, color):
    res = []
    free_dead_stones_board = copy_board(board)
    for i in range(len(board)):
        for j in range(len(board)):
            if not life_table[i][j]:
                free_dead_stones_board[i][j] = empty_color
    reachabilities = []
    for i in range(n_players):
        reachable_from_i_color = \
            find_points_reachable_from_color(free_dead_stones_board, i)
        reachabilities.append(reachable_from_i_color)
    for i in range(len(free_dead_stones_board)):
        for j in range(len(free_dead_stones_board)):
            is_my_territory = True
            for k in range(n_players):
                if reachabilities[k][i][j] and k != color:
                    is_my_territory = False
            if reachabilities[color][i][j] == False:
                is_my_territory = False
            if free_dead_stones_board[i][j] != empty_color:
                is_my_territory = False
            if is_my_territory:
                res.append([i, j])
    return res



def count_result(n_players, board, life_table, color):
    territory = len(_count_result(n_players, board, life_table, color))
    stones = 0
    for i in range(len(board)):
        for j in range(len(board)):
            if life_table[i][j] and board[i][j] == color:
                stones += 1
    res = territory + stones
    return res

def print_board(board):
    for i in range(len(board)):
        row = ""
        for j in range(len(board)):
            if board[i][j] == 0:
                row += 'X'
            if board[i][j] == 1:
                row += 'O'
            if board[i][j] == 9:
                row += '*'
        print(row)

"""
board = [
    [0, 9, 0, 1, 9],
    [9, 0, 1, 1, 9],
    [0, 1, 1, 1, 1],
    [1, 9, 1, 0, 0],
    [9, 9, 1, 0, 9],
]

life_table = [
    [True, True, True, True, True],
    [True, True, True, True, True],
    [True, True, True, True, True],
    [True, True, True, True, True],
    [True, True, True, True, True],
]

toggled_table = toggle_life_table(life_table, board, 0, 0)
"""