class Time:
    def tick(self):
        return False

    def reset(self):
        pass

    @staticmethod
    def create(settings):
        if settings[0] == 'b':
            main_string, overtime_string, periods_string = settings[1:].split('#')
            return Byoyomi(int(main_string), int(overtime_string), int(periods_string))
        return None

class Absolute(Time):
    def __init__(self, maintime):
        self.time = maintime
        self.maintime = maintime

    def tick(self):
        self.time -= 1
        if self.time <= 0:
            return True
        else:
            return False


    def reset(self):
        self.time = self.maintime

    def __str__(self):
        return "a"+str(self.time)


class Byoyomi(Time):
    def __init__(self, maintime, overtime, periods):
        self.maintime = maintime
        self.overtime = overtime
        self.periods = periods
        self.maintime_used = False
        self.periods_remaining = self.periods
        self.overtime_time = 0
        self.maintime_time = self.maintime

    def move(self):
        self.overtime_time = self.overtime

    def reset(self):
        self.maintime_used = False
        self.periods_remaining = self.periods
        self.overtime_time = 0
        self.maintime_time = self.maintime

    def tick(self):
        if not self.maintime_used:
            self.maintime_time -= 1
            if self.maintime_time == 0:
                self.maintime_used = True
                self.overtime_time = self.overtime
            return True
        else:
            self.overtime_time -= 1
            if self.overtime_time == 0:
                if self.periods_remaining == 1:
                    return False
                else:
                    self.periods_remaining -= 1
                    self.overtime_time = self.overtime
                    return True
            else:
                return True

    def __str__(self):
        return "b"+str(self.maintime_time)+"#"+str(self.overtime_time)+"#"+str(self.periods_remaining)