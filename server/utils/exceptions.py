class ServerException(BaseException):
    pass


class SecurityException(ServerException):
    pass

class HandlerCollisionException(ServerException):
    pass

class UnexpectedTypeException(ServerException):
    key = ""

    def __init__(self, key):
        self.key = key

    def __str__(self):
        return self.key

class ParameterException(ServerException):
    key = ""

    def __init__(self, key):
        self.key = key

    def __str__(self):
        return self.key

class MissedParameterException(ParameterException):
    pass

class UnexpectedParameterException(ParameterException):
    pass

class UnexpectedRequestTypeException(ParameterException):
    pass

class ParseException(ServerException):
    pass