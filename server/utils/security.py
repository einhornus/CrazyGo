import hashlib
import base64
import uuid
import datetime
import random

def hash_password(password):
    salt = 'Anna Zhavoronkova'
    t_sha = hashlib.sha512()
    t_sha.update((password+salt).encode('utf-8'))
    hashed_password = base64.urlsafe_b64encode(t_sha.digest())
    return hashed_password.decode('utf-8')

def hash(word):
    t_sha = hashlib.sha512()
    t_sha.update((word).encode('utf-8'))
    res = base64.urlsafe_b64encode(t_sha.digest())
    return res.decode('utf-8')

def gen_token(id):
    time = str(datetime.datetime.now().timestamp())
    salt = 'Anyusha chan'
    word = time+salt+str(id)
    token = hash(word)
    token = (time + '|' + token).replace('=', '+')
    return token

def check_token(id, token):
    salt = 'Anyusha chan'
    time = token[0:token.index('|')]
    word = time + salt + str(id)
    new_token = (time + "|" + hash(word)).replace('=', '+')
    return new_token == token