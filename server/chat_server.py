from Server import *
import lobby_server_handlers.AuthorizationHandler
import chat_server_handlers.ChatHandler
import lobby_server_handlers.factory_utils
import settings


server = Server(settings.chat_server[1])
server.add_handler(lobby_server_handlers.AuthorizationHandler.AuthorizationHandler())
server.add_handler(chat_server_handlers.ChatHandler.ChatHandler())
server.setLostConnectionFunc(lobby_server_handlers.factory_utils.lostConnectionFunc)
log.startLogging(open('chat_server.txt', 'w+'))
server.launch()