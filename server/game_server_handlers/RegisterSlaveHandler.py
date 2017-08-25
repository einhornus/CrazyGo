from Server import *
from utils.Validator import *

from utils.print_utils import *
from utils.db_queries import *

from utils.security import *
from game_server_handlers.game_server_utils import *



class RegisterSlaveHandler(Handler):
    def get_validator(self):
        res = Validator([])
        return res

    def get_type(self):
        return 'register_slave'

    def action(self, request, factory, me):
        factory.slave = me
        pass

