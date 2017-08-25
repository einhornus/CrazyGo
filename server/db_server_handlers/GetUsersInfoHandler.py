from Server import *
from utils.Validator import *

from utils.print_utils import *
from utils.db_queries import *

from utils.security import *


class GetUsersInfoHandler(Handler):
    def get_validator(self):
        res = Validator(['id', 'token', 'ids'])
        return res

    def get_type(self):
        return 'get_users'

    def action(self, request, factory, me):
        ids = request["ids"].split('-')
        id = request["id"]
        token = request["token"]
        if check_token(id, token):
            db_res = get_users_info(ids)
            array = []
            for i in range(len(db_res)):
                info = db_res[i]
                name = check_none(info[1])
                birthday = check_none(info[2])
                country = check_none(info[3])
                city = check_none(info[4])
                sex = check_none(info[5])
                avatar_id = check_none(info[6])
                login = check_none(info[7])
                avatar_server_id = check_none(info[9])
                mini_avatar_id = check_none(info[10])
                object = {'name':name,
                      'birthday':birthday,
                      'country_id':country,
                      'city':city,
                      'sex_id':sex,
                      'avatar_id':avatar_id,
                      'avatar_server_id':avatar_server_id,
                      'login':login,
                      'mini_avatar_id':mini_avatar_id
                      }
                array.append(object)
            me.sendLine(print_object(array))
        else:
            me.sendLine(print_error(SECURITY_ERROR, token))