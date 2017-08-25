import json

UNEXPECTED_ERROR = "UNEXPECTED_ERROR"

SECURITY_ERROR = "SECURITY"
CONNECTION_REPEAT = "CONNECTION_REPEAT"
PARSE_ERROR = "PARSE"
UNEXPECTED_PARAMETER = "UNEXPECTED_PARAMETER"
MISSED_PARAMETER = "MISSED_PARAMETER"
UNEXPECTED_REQUEST_TYPE = "UNEXPECTED_REQUEST_TYPE"

LOGIN_EXISTS = "LOGIN_EXISTS"
WRONG_LOGIN_PASSWORD = "WRONG_LOGIN_PASSWORD"

MOVE_IS_ILLEGAL = "MOVE_IS_ILLEGAL"

def print_object(object):
    res = json.dumps(object)
    return res

def print_error(error_code, message):
    res = "!"+error_code+"$"+message
    return res

def check_none(value):
    if value is None:
        return 'null'
    else:
        return value
