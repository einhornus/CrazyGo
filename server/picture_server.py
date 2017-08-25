from Server import *
from settings import *

#from picture_server_handlers.EndHandler import *
import picture_server_handlers.EndHandler
import picture_server_handlers.ChunkHandler
import picture_server_handlers.GetPictureHandler

index = 0
server = Server(picture_server[index][1])
server.add_handler(picture_server_handlers.EndHandler.EndHandler())
server.add_handler(picture_server_handlers.ChunkHandler.ChunkHandler())
server.add_handler(picture_server_handlers.GetPictureHandler.GetPictureHandler())
server.launch()