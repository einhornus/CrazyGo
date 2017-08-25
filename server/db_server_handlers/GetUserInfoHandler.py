from Server import *
from utils.Validator import *

from utils.print_utils import *
from utils.db_queries import *

from utils.security import *


class GetUserInfoHandler(Handler):
    def get_validator(self):
        res = Validator(['id', 'token', 'user_id'])
        return res

    def get_type(self):
        return 'get_user'

    def action(self, request, factory, me):
        id = request["id"]
        user_id = request["user_id"]
        token = request["token"]
        if check_token(id, token):
            info = get_user_info(user_id)
            login = check_none(info[1])
            rank = check_none(info[3])
            object = {
                      'login' : login,
                      'rank' : rank
                      }
            me.sendLine(print_object(object))
        else:
            me.sendLine(print_error(SECURITY_ERROR, token))