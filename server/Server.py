from twisted.internet import reactor

from twisted.internet.protocol import ServerFactory
from twisted.protocols.basic import LineOnlyReceiver
from twisted.python import log

from utils.exceptions import *
from utils.Request import *
from utils.Validator import *
from utils.print_utils import *

import traceback

class ServerProtocol(LineOnlyReceiver):
    def connectionMade(self):
        log.msg('New connection')
        pass

    def connectionLost(self, reason):
        if not(self.factory.lost_connection_func is None):
            self.factory.lost_connection_func(self)
        print("lost connection")
        log.msg('Lost connection')
        pass

    def lineReceived(self, bytes):
        try:
            string = bytes.decode('utf-8')
            if string[-1] == '\r':
                string = string[0:-1]
            print("Received "+str(string))
            log.msg('Got '+str(string))
            request = Request(string)
            if not (request.type in self.factory.handlers):
                raise UnexpectedRequestTypeException(request.type)
            handler = self.factory.handlers[request.type]
            handler.execute(request, self, self.factory)
        except SecurityException:
            self.sendLine(print_error(SECURITY_ERROR, traceback.format_exc()))
        except ParseException:
            self.sendLine(print_error(PARSE_ERROR, traceback.format_exc()))
        except UnexpectedParameterException:
            self.sendLine(print_error(UNEXPECTED_PARAMETER, traceback.format_exc()))
        except MissedParameterException:
            self.sendLine(print_error(MISSED_PARAMETER, traceback.format_exc()))
        except UnexpectedRequestTypeException:
            self.sendLine(print_error(UNEXPECTED_REQUEST_TYPE, traceback.format_exc()))
        except BaseException:
            self.sendLine(print_error(UNEXPECTED_ERROR, traceback.format_exc()))

    def sendLine(self, line):
        print('send '+str(line))
        if hasattr(self, "id"):
            #print('send')
            log.msg("Send line '"+line+"' to "+str(self.id))
        else:
            #print('send')
            log.msg("Send line '"+line+"'")
        line = line.replace('\n', '@')
        self.transport.write((line+"\r\n").encode('utf-8'))

class ServerProtocolFactory(ServerFactory):
    handlers = {}
    protocol = ServerProtocol
    lost_connection_func = None

    def __init__(self):
        self.clientProtocols = []

class Handler:
    def get_validator(self):
        pass

    def get_type(self):
        pass

    def action(self, request, me, factory):
        pass

    def execute(self, request, factory, me):
        validator = self.get_validator()
        validator.validate(request)
        self.action(request, me, factory)

class Server:
    port = 0
    factory = ServerProtocolFactory()

    def __init__(self, port):
        self.port = port

    def setLostConnectionFunc(self, func):
        self.factory.lost_connection_func = func

    def add_handler(self, handler):
        type = handler.get_type()
        if not (type in self.factory.handlers):
            self.factory.handlers[type] = handler
        else:
            raise HandlerCollisionException()

    def launch(self):
        reactor.listenTCP(self.port, self.factory)
        reactor.run()