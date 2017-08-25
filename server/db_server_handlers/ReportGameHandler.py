from Server import *
from utils.Validator import *

from utils.print_utils import *
from utils.db_queries import *

from utils.security import *


class ReportGameHandler(Handler):
    def get_validator(self):
        res = Validator(['users', 'title', 'settings', 'result'])
        return res

    def get_type(self):
        return 'report_game'

    def get_bet(self, settings):
        _pairs = settings.split("|")
        pairs = {}
        for i in range(len(_pairs)):
            key, value = _pairs[i].split('-')
            pairs[key] = value
        return int(pairs['bet'])

    def action(self, request, factory, me):
        """
        title = request['title']
        users = request['users']
        settings = request['settings']
        result = request['result']
        result_array = result.split('-')
        users_str_list = users.split('?')
        users_array = [int(users_str_list[i]) for i in range(len(users_str_list))]
        #add_game_result(title, users_array, settings, result)


        winner = int(result_array[0])

        winner_id = users_array[0]
        if winner == 1:
            winner_id = users_array[1]

        loser_id = users_array[1]
        if winner == 1:
            loser_id = users_array[0]

        add_game_result(title, settings, winner_id, loser_id, int(result_array[1]))
        """

        me.sendLine(print_object({}))
