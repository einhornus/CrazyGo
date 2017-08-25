from Server import *
import leela_server_handlers.LeelaHandler
import leela_server_handlers.ReceiveHelpHandler
import leela_server_handlers.RegisterProviderHandler
import settings


server = Server(settings.leela_servers[0][1])
server.add_handler(leela_server_handlers.LeelaHandler.LeelaHandler())
server.add_handler(leela_server_handlers.ReceiveHelpHandler.ReceiveHelpHandler())
server.add_handler(leela_server_handlers.RegisterProviderHandler.RegisterProviderHandler())
server.launch()
print("Start server")