from Server import *
from utils.Validator import *

from utils.print_utils import *
from utils.db_queries import *

from utils.security import *

class AuthorizationHandler(Handler):
    def get_validator(self):
        res = Validator(['login', 'password'])
        return res

    def get_type(self):
        return 'authorize'

    def action(self, request, factory, me):
        login = request["login"]
        password = request["password"]
        hashed = hash_password(password)
        if check_login_password_pair(login, hashed):
            id = get_user_id_by_login(login)
            token = gen_token(id)
            me.sendLine(print_object({"id":id, "token":token}))
        else:
            me.sendLine(print_error(WRONG_LOGIN_PASSWORD, ""))
        pass