from Server import *
from utils.Validator import *

class EchoHandler(Handler):
    def get_validator(self):
        res = Validator(['text'])
        return res

    def get_type(self):
        return 'echo'

    def action(self, request, factory, me):
        me.sendLine(request['text'])