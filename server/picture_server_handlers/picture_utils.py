from utils.db import *

pictures_folder_path = "C://Cloud@Mail.Ru/Brain Duels/server/pics/"

def set_ava_id(user_id, server_id, ava_id, size):
    if size == 'large':
        query = 'update users set avatar_id={}, avatar_server_id={} where user_id = {};'.format(ava_id, server_id, user_id)
    else:
        query = 'update users set mini_avatar_id={}, avatar_server_id={} where user_id = {};'.format(ava_id, server_id, user_id)
    execute_query(query)