class Settings():
    def __init__(self, str):
        self._str = str
        _pairs = str.split("|")
        self.pairs = {}
        for i in range(len(_pairs)):
            key, value = _pairs[i].split('-')
            self.pairs[key] = value

    def __getitem__(self, item):
        return self.pairs[item]

    def __str__(self):
        return self._str