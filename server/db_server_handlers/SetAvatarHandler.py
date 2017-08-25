from Server import *
from utils.Validator import *

from utils.print_utils import *
from utils.db_queries import *

from utils.security import *
import picture_server_handlers.picture_utils


class SetAvatarHandler(Handler):
    def get_validator(self):
        res = Validator(['id', 'token', 'index', 'server_id', 'size'])
        return res

    def get_type(self):
        return 'set_ava'

    def action(self, request, factory, me):
        id = request["id"]
        token = request["token"]
        if check_token(id, token):
            server = request['server_id']
            index = request['index']
            size = request['size']
            picture_server_handlers.picture_utils.set_ava_id(id, server, index, size)
            me.sendLine(print_object({}))
        else:
            me.sendLine(print_error(SECURITY_ERROR, token))