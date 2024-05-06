from attributes.singleton import singleton


@singleton
class DependencyContainer:
    def __init__(self) -> None:
        self.__dependencies = {}

    def add_dependency(self, dependency_type, dependency):
        self.__dependencies[dependency_type.__name__] = dependency

    def clear_dependencies(self):
        self.__dependencies.clear()

    def get_dependency(self, dependency_type):
        if dependency_type.__name__ in self.__dependencies:
            return self.__dependencies[dependency_type.__name__]
        raise ValueError(f"No dependency of type {dependency_type.__name__} was found!")
