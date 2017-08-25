from Server import *
from utils.Validator import *

from utils.print_utils import *
from utils.db_queries import *

from utils.security import *


class GetUsersCoinsInfoHandler(Handler):
    def get_validator(self):
        res = Validator(['id', 'token', 'ids'])
        return res

    def get_type(self):
        return 'get_users_coins'

    def action(self, request, factory, me):
        ids = request["ids"].split('-')
        id = int(request["id"])
        token = request["token"]

        if check_token(id, token):
            db_res = get_users_coins_info(ids)
            array = []
            for i in range(len(db_res)):
                info = db_res[i]
                if info != None:
                    _id = int(info[0])
                    xp = int(info[1])
                    coins = int(info[2])
                    object = {'user_id':_id,
                            'xp':xp,
                            'coins':coins
                            }
                    array.append(object)
            me.sendLine(print_object(array))
        else:
            me.sendLine(print_error(SECURITY_ERROR, token))