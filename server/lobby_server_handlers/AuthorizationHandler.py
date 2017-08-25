from Server import *
from utils.Validator import *

from utils.print_utils import *
from utils.db_queries import *
from utils.security import *
from lobby_server_handlers.factory_utils import *

class AuthorizationHandler(Handler):
    def get_validator(self):
        res = Validator(['id', 'token'])
        return res

    def get_type(self):
        return 'authorize'

    def userEntered(self, me, id):
        print("New connection from " + str(id))
        me.id = id
        sendMessageToAllClients(me.factory, "+ "+str(me.id))
        if not hasattr(me.factory, "clientProtocols"):
            me.factory.clientProtocols = []
        me.factory.clientProtocols.append(me)
        me.sendLine("all_users "+getAllIdsString(me.factory))
        if not hasattr(me.factory, 'games'):
            me.factory.games = []
        me.sendLine("all_games "+getAllGamesString(me.factory))

    def action(self, request, factory, me):
        id = request["id"]
        token = request["token"]
        if check_token(id, token):
            if not hasattr(me.factory, "clientProtocols"):
                me.factory.clientProtocols = []
            for i in range(len(me.factory.clientProtocols)):
                if int(id) == me.factory.clientProtocols[i].id:
                    me.sendLine(print_error(CONNECTION_REPEAT, ""))
                    return
            else:
                self.userEntered(me, int(id))
        else:
            me.sendLine(print_error(SECURITY_ERROR, ""))