from common import adventBase
from common import helpers 

class DayX(adventBase.AdventBase):
    def __init__(self) -> None:
        super().__init__('', -1, __file__)

    # --- First ---
    def test(self, input: list)-> adventBase.TestResult:
        value = self.__first(input)
        return adventBase.TestResult(-1, value)

    def first(self, input: list) -> adventBase.Result:
        value = self.__first(input)
        return adventBase.Result(value)

    def __first(self, input: list) -> int:
        return 0

    # --- Second ---
    def test2(self, input: list) -> adventBase.TestResult:
        value = self.__second(input)
        return adventBase.TestResult(-2, value)

    def second(self, input: list) -> adventBase.Result:
        value = self.__second(input)
        return adventBase.Result(value)

    def __second(self, input: list):
        return 0

    # -- Helpers --
    def __convert_input(self, input: list[str]):
        return input

