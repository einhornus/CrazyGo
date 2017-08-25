from utils.db import *
from settings import *
import json
import random

class GameChallenge:
    def __init__(self, id, title, settings, max):
        self.title = title
        self.settings = settings
        self.id = id
        self.max = int(max)
        self.users = []
        self.status = 'open'
        self.server = random.randint(0, len(game_servers) - 1)

    def __str__(self):
        j = json.dumps(self, default=lambda o: o.__dict__, sort_keys=True, indent=4)
        j = j.replace('\n', '')
        j = j.replace(' ', '')
        return j