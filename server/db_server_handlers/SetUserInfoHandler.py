from Server import *
from utils.Validator import *

from utils.print_utils import *
from utils.db_queries import *

from utils.security import *


class SetUserInfoHandler(Handler):
    def get_validator(self):
        res = Validator(['id', 'token', 'new_rank'])
        return res

    def get_type(self):
        return 'set_user'

    def action(self, request, factory, me):
        id = request["id"]
        token = request["token"]
        if check_token(id, token):
            set_user_info(id, request['new_rank'])
            me.sendLine(print_object(""))
        else:
            me.sendLine(print_error(SECURITY_ERROR, token))