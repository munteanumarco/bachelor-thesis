import functools


def singleton(cls):
    instances = {}

    @functools.wraps(cls)
    def wrapper(*args, **kwargs):
        reset = kwargs.pop('reset', False)
        if reset or cls not in instances:
            instances[cls] = cls(*args, **kwargs)
        return instances[cls]
    return wrapper
