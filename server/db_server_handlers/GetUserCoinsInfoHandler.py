from Server import *
from utils.Validator import *

from utils.print_utils import *
from utils.db_queries import *

from utils.security import *


class GetUserCoinsInfoHandler(Handler):
    def get_validator(self):
        res = Validator(['id', 'token', 'user_id'])
        return res

    def get_type(self):
        return 'get_user_coins'

    def action(self, request, factory, me):
        id = request["id"]
        user_id = int(request["user_id"])
        token = request["token"]
        if check_token(id, token):
            info = get_user_coins_info(user_id)
            xp = int(info[1])
            coins = int(info[2])
            object = {'user_id':user_id,
                      'xp':xp,
                      'coins':coins
                      }
            me.sendLine(print_object(object))
        else:
            me.sendLine(print_error(SECURITY_ERROR, token))