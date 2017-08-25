from Server import *
from utils.Validator import *

from utils.print_utils import *
from utils.db_queries import *

from utils.security import *


class AddCoinsHandler(Handler):
    def get_validator(self):
        res = Validator(['id', 'token', 'amount'])
        return res

    def get_type(self):
        return 'add_coins'

    def action(self, request, factory, me):
        id = request["id"]
        token = request["token"]
        amount = int(request["amount"])
        if check_token(id, token):
            add_coins(id, amount)
            me.sendLine(print_object({}))
        else:
            me.sendLine(print_error(SECURITY_ERROR, token))