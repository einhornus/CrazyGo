UPPER_LEFT_HOSHI = [3, 3]
UPPER_RIGHT_HOSHI = [15, 3]
LOWER_LEFT_HOSHI = [3, 15]
LOWER_RIGHT_HOSHI = [15, 15]
LEFT_HOSHI = [3, 9]
RIGHT_HOSHI = [15, 9]
UPPER_HOSHI = [9, 3]
LOWER_HOSHI = [9, 15]
TENGEN = [9, 9]


REVERSE_KOMI_9_9 = 2
REVERSE_KOMI_13_13 = 4
REVERSE_KOMI_19_19 = 8


def get_komi_value(handi, size):
    if size != 9 and size != 13 and size != 19:
        raise ValueError()
    if handi < 0:
        raise ValueError()
    if size == 19:
        if handi == 0:
            return 7.5
        else:
            if handi <= 9:
                return 0.5
            else:
                additional = handi - 9
                res = 0.5 - REVERSE_KOMI_19_19*additional
                return res
    if size == 9:
        return 7.5 - REVERSE_KOMI_9_9*handi
    if size == 13:
        return 7.5 - REVERSE_KOMI_13_13*handi


def get_black(handi, size, skilled_one):
    if size != 9 and size != 13 and size != 19:
        raise ValueError()
    if handi < 0:
        raise ValueError()
    return 1-skilled_one


def who_starts(handi, size, skilled_one):
    if size != 9 and size != 13 and size != 19:
        raise ValueError()
    if handi < 0:
        raise ValueError()
    if size == 19:
        if handi > 1:
            return skilled_one
        else:
            return 1-skilled_one
    else:
        return 1-skilled_one


def get_stones(handi, size):
    if size != 9 and size != 13 and size != 19:
        raise ValueError()
    if handi < 0:
        raise ValueError()
    if size == 19:
        if handi == 0:
            return []
        if handi == 1:
            return []
        if handi == 2:
            return [UPPER_RIGHT_HOSHI, LOWER_LEFT_HOSHI]
        if handi == 3:
            return [UPPER_RIGHT_HOSHI, LOWER_LEFT_HOSHI, LOWER_RIGHT_HOSHI]
        if handi == 4:
            return [UPPER_RIGHT_HOSHI, LOWER_LEFT_HOSHI, LOWER_RIGHT_HOSHI, UPPER_LEFT_HOSHI]
        if handi == 5:
            return [UPPER_RIGHT_HOSHI, LOWER_LEFT_HOSHI, LOWER_RIGHT_HOSHI, UPPER_LEFT_HOSHI, TENGEN]
        if handi == 6:
            return [UPPER_RIGHT_HOSHI, LOWER_LEFT_HOSHI, LOWER_RIGHT_HOSHI, UPPER_LEFT_HOSHI, LEFT_HOSHI, RIGHT_HOSHI]
        if handi == 7:
            return [UPPER_RIGHT_HOSHI, LOWER_LEFT_HOSHI, LOWER_RIGHT_HOSHI, UPPER_LEFT_HOSHI, LEFT_HOSHI, RIGHT_HOSHI, TENGEN]
        if handi == 8:
            return [UPPER_RIGHT_HOSHI, LOWER_LEFT_HOSHI, LOWER_RIGHT_HOSHI, UPPER_LEFT_HOSHI, LEFT_HOSHI, RIGHT_HOSHI, UPPER_HOSHI, LOWER_HOSHI]
        if handi >= 9:
            return [UPPER_RIGHT_HOSHI, LOWER_LEFT_HOSHI, LOWER_RIGHT_HOSHI, UPPER_LEFT_HOSHI, LEFT_HOSHI, RIGHT_HOSHI, UPPER_HOSHI, LOWER_HOSHI, TENGEN]
    else:
        return []