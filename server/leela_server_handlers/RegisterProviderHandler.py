from Server import *
from utils.Validator import *
import math

import leela_server_handlers.leela_utils

class RegisterProviderHandler(Handler):
    def get_validator(self):
        res = Validator(['code'])
        return res

    def get_type(self):
        return 'register_provider'

    def action(self, request, factory, me):
        factory.provider = me
