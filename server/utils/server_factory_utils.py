def sendMessageToAllClients(self, mesg):
    if not hasattr(self, 'clientProtocols'):
        self.clientProtocols = []
    for client in self.clientProtocols:
        client.sendLine(mesg)

def findGame(self, id):
    if not hasattr(self, 'games'):
        self.games = []

    for game in self.games:
        if str(game.id) == str(id):
            return game
    return None

def findClient(self, id):
    if not hasattr(self, 'clientProtocols'):
        self.clientProtocols = []
    for client in self.clientProtocols:
        if client.id == id:
            return client
    return None

def send_to_players_and_observers(self, game, message):
    for i in range(len(game.active_players)):
        clientId = game.active_players[i]
        client = findClient(self, clientId)
        if not (client is None):
            index = 9
            for j in range(len(game.users)):
                if game.users[j] == client.id:
                    index = j
            client.sendLine(message+";"+str(index))

    for i in range(len(game.observers)):
        clientId = game.observers[i]
        client = findClient(self, clientId)
        if not (client is None):
            client.sendLine(message+";"+"9")

def isAuthorized(self):
    if not hasattr(self.factory, 'clientProtocols'):
        self.factory.clientProtocols = []
    for client in self.factory.clientProtocols:
        if client.id == self.id:
            return True
    return False