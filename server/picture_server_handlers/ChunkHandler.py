from Server import *
from utils.Validator import *

from utils.print_utils import *
from utils.db_queries import *

from utils.security import *
from picture_server_handlers.picture_utils import *

class ChunkHandler(Handler):
    def get_validator(self):
        res = Validator(['id', 'token', 'index', 'chunk'])
        return res

    def get_type(self):
        return 'chunk'

    def action(self, request, factory, me):
        id = request["id"]
        token = request["token"]
        index = int(request["index"])
        chunk_content = request['chunk']

        if check_token(id, token):
            if index == 0:
                me.chunks = []
            me.chunks.append(chunk_content)
        else:
            me.sendLine(print_error(SECURITY_ERROR, ""))