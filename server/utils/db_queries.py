from utils.db import *
import time

def escape_string(_str):
    __str = str(_str)
    res = ''
    for i in range(len(__str)):
        if __str[i] >= 'a' or __str[i] <= 'z':
            res += __str[i]
            continue
        if __str[i] >= 'A' or __str[i] <= 'Z':
            res += __str[i]
            continue
        if __str[i] >= '0' or __str[i] <= '9':
            res += __str[i]
            continue
        if __str[i] == '!':
            res += '!'
            continue
        if __str[i] == ' ':
            res += ' '
            continue
    return res

import time


def check_login_available(login):
    query = 'select * from users where users.login="{}";'.format(escape_string(login))
    result = execute_query_fetched(query)
    return result == None or result[0] == 0

def add_user(login, password):
    query = 'insert into users(login, password) values("{}","{}");'.format(escape_string(login), escape_string(password))
    execute_query(query)
    return True

def check_login_password_pair(login, password):
    query = 'select * from users where users.login="{}" and users.password="{}";'.format(escape_string(login), escape_string(password))
    result = execute_query_fetched(query)
    return not(result == None or result[0] == 0)

def get_user_id_by_login(login):
    query = 'select * from users where users.login="{}";'.format(escape_string(login))
    result = execute_query_fetched(query)
    return int(result[0])

def get_user_info(id):
    query = 'select * from users where users.user_id={};'.format(escape_string(id))
    result = execute_query_fetched(query)
    return result

def get_user_rank(id):
    result = get_user_info(id)
    rank = int(result[3])
    return rank


large_query_limit = 50

def construct_in_clause(ids):
    print(ids)
    if ids == ['']:
        return '()'
    in_clause = '( '
    for i in range(len(ids)):
        in_clause += str(int(ids[i]))
        if i != len(ids)-1:
            in_clause += ','
    in_clause += ')'
    return in_clause

def get_users_info(ids):
    in_clause = construct_in_clause(ids)
    query = 'select * from users where users.user_id in '+in_clause
    result = execute_query_fetched_all(query)
    return result


def get_user_coins_info(id):
    query = 'select * from coins where coins.user_id={};'.format(id)
    result = execute_query_fetched(query)
    if result == None:
        query = 'insert into coins (user_id, xp, coins) values ("{}", "{}", "{}")'.format(escape_string(id), 0, 300)
        execute_query(query)
        return (id, 0, 300)
    return result


def get_users_coins_info(ids):
    in_clause = construct_in_clause(ids)
    query = 'select * from coins where coins.user_id in '+in_clause
    result = execute_query_fetched_all(query)
    return result


def add_coins(id, coins):
    print('add' +str(coins)+' coins to '+str(id))
    query = 'select coins from coins where coins.user_id={};'.format(id)
    result = execute_query_fetched(query)
    resulting = int(result[0])+coins
    query = 'update coins set coins = {} where coins.user_id = {};'.format(escape_string(resulting), escape_string(id))
    execute_query(query)


def add_xp(id, xp):
    print('add' +str(xp)+' xp to '+str(id))
    query = 'select xp from coins where coins.user_id={};'.format(id)
    result = execute_query_fetched(query)
    resulting = int(result[0])+xp
    query = 'update coins set xp = {} where coins.user_id = {};'.format(escape_string(resulting), escape_string(id))
    execute_query(query)


def set_user_info(id, new_rank):
    query = 'update users set rank="{}" where user_id={};'.format(escape_string(new_rank), escape_string(id))
    execute_query(query)

def add_game_result(title, settings, winner, loser, points):
    t = int(time.time())
    query = 'insert into games (title, settings, winner, loser, points, time) values ("{}", "{}", {}, {}, {}, {})'.format(escape_string(title), escape_string(settings), escape_string(winner), escape_string(loser), escape_string(points), escape_string(t))
    execute_query(query)


def get_leaderboard(size, include_backgammon, include_narde):
    month = 1000*60*60*24*30
    t = int(time.time())
    critical = t - month
    query1 = 'select winner, loser, points from games where (0 '
    if include_backgammon:
        query1 += 'or title = "backgammon" '
    if include_narde:
        query1 += 'or title = "narde") and time > '+str(critical)

    results = execute_query_fetched_all(query1)

    users = {}
    for i in range(len(results)):
        winner = results[i][0]
        loser =  results[i][1]
        points = results[i][2]

        if not(winner in users):
            users[winner] = [0, 0]

        if not(loser in users):
            users[loser] = [0, 0]

        users[winner][0]+=1
        users[loser][0]+=1

        users[winner][1] += points
        users[loser][1] -= points

    table = []
    for key in users:
        games = users[key][0]
        won_points = users[key][1]
        roi = won_points/games
        rating = (games) * roi * roi * roi
        table.append((key, games, roi, rating))

    table.sort(key=lambda x:-x[2])
    if len(table) > size:
        table = table[0:size]

    return table


