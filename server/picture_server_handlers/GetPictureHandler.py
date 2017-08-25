from Server import *
from utils.Validator import *

from utils.print_utils import *
from utils.db_queries import *

from utils.security import *
from picture_server_handlers.picture_utils import *
from settings import *

class GetPictureHandler(Handler):
    def get_validator(self):
        res = Validator(['id', 'token', 'picture_id'])
        return res

    def get_type(self):
        return 'get'

    def action(self, request, factory, me):
        id = request["id"]
        token = request["token"]
        picture_id = request['picture_id']
        if check_token(id, token):
            try:
                file = open(pictures_folder_path+str(picture_id), 'rb')
                content = file.read().decode('utf-8')
                chunks = []
                pointer = 0
                while pointer < len(content):
                    end = min(pointer+chunk_size, len(content))
                    chunk = content[pointer:end]
                    pointer += chunk_size
                    chunks.append(chunk)
                me.sendLine("begin ")
                for i in range(len(chunks)):
                    me.sendLine("chunk "+str(i)+";"+chunks[i])
                me.sendLine("end ")
            except BaseException as a:
                me.sendLine(print_error(UNEXPECTED_ERROR, traceback.format_exc()))
        else:
            me.sendLine(print_error(SECURITY_ERROR, ""))