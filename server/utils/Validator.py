from utils.exceptions import *
from utils.Request import *

class Validator:
    params = []
    def __init__(self, params):
        self.params = params

    def validate(self, request):
        array = [False for i in range(len(self.params))]
        for i in range(len(request.parameters)):
            for j in range(len(self.params)):
                if self.params[j] == request.parameters[i].key:
                    array[j] = True
                    break
            else:
                raise UnexpectedParameterException(request.parameters[i].key)
        for i in range(len(self.params)):
            if not array[i]:
                raise MissedParameterException(self.params[i])


