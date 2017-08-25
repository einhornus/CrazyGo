from Server import *
from utils.Validator import *

from utils.print_utils import *
from utils.db_queries import *

from utils.security import *

class RegistrationHandler(Handler):
    def get_validator(self):
        res = Validator(['login', 'password'])
        return res

    def get_type(self):
        return 'register'

    def action(self, request, factory, me):
        login = request["login"]
        password = request["password"]
        hashed = hash_password(password)
        if check_login_available(login):
            add_user(login, hashed)
            me.sendLine(print_object({}))
        else:
            me.sendLine(print_error(LOGIN_EXISTS, login))