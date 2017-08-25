from Server import *
from utils.Validator import *

from utils.print_utils import *
from utils.db_queries import *

from utils.security import *
from picture_server_handlers.picture_utils import *

import os


class EndHandler(Handler):
    def get_validator(self):
        res = Validator(['id', 'token', 'server_index'])
        return res

    def get_type(self):
        return 'end'

    def action(self, request, factory, me):
        id = request["id"]
        token = request["token"]
        server_index = int(request['server_index'])

        if check_token(id, token):      
            content = ""
            for i in range(len(me.chunks)):
                content += me.chunks[i]
            number_of_image_files = len(os.listdir(pictures_folder_path))
            try:
                file = open(pictures_folder_path+str(number_of_image_files), 'wb')
                file.write(content.encode("utf-8"))
                me.sendLine("index "+str(number_of_image_files))
            except BaseException as a:
                me.sendLine(print_error(UNEXPECTED_ERROR, traceback.format_exc()))
        else:
            me.sendLine(print_error(SECURITY_ERROR, ""))