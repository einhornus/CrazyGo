from Server import *
from utils.Validator import *

from utils.print_utils import *
from utils.db_queries import *

from utils.security import *
import lobby_server_handlers.factory_utils


class ChatHandler(Handler):
    def get_validator(self):
        res = Validator(['id', 'receiver', 'message'])
        return res

    def get_type(self):
        return 'chat'

    def action(self, request, factory, me):
        id = int(request['id'])
        message = request['message']
        receiverId = int(request['receiver'])
        if receiverId != 0:
            receiverClient = lobby_server_handlers.factory_utils.findClient(factory, receiverId)
            if receiverClient != None:
                receiverClient.sendLine("chat_message "+str(id)+";"+message)

            senderClient = lobby_server_handlers.factory_utils.findClient(factory, id)
            if senderClient != None:
                senderClient.sendLine("chat_message "+str(id)+";"+message)
        else:
            if not hasattr(factory, 'clientProtocols'):
                factory.clientProtocols = []
            for client in factory.clientProtocols:
                client.sendLine("chat_message "+str(id)+";"+message)

