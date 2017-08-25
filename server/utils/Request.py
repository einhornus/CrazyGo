from utils.exceptions import *

class Parameter:
    key = ""
    value = ""

    def __init__(self, key, value):
        self.key = key
        self.value = value

class Request:
    parameters = []
    type = ""
    def __init__(self, str):
        try:
            #t, p = list(str.split(' '))
            _index = str.index(' ')
            t = str[0:_index]
            p = str[_index+1:]
            print(t, p)
        except Exception as e:
            raise ParseException(str(e))

        self.type = t
        params_pairs = list(p.split(';'))
        self.parameters = []

        for i in range(len(params_pairs)):
            if params_pairs[i] != "":
                try:
                    key, value = list(params_pairs[i].split('='))
                except Exception as e:
                    raise ParseException()
                parameter = Parameter(key, value)
                self.parameters.append(parameter)

    def __getitem__(self, item):
        for i in range(len(self.parameters)):
            if self.parameters[i].key == item:
                return self.parameters[i].value