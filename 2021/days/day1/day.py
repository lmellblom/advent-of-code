from common import adventBase
from common import helpers 

class Day1(adventBase.AdventBase):
    def __init__(self) -> None:
        super().__init__('Sonar Sweep', 1, __file__)

    # --- First ---
    def test(self, input: list)-> adventBase.TestResult:
        value = self.__first(input)
        return adventBase.TestResult(7, value)

    def first(self, input: list) -> adventBase.Result:
        value = self.__first(input)
        return adventBase.Result(value)

    def __first(self, input: list):
        intList = helpers.convert_list_to_int(input)
        value = self.__countDepthIncreases(intList)
        return value

    # --- Second ---
    def test2(self, input: list) -> adventBase.TestResult:
        value = self.__second(input)
        return adventBase.TestResult(5, value)

    def second(self, input: list) -> adventBase.Result:
        value = self.__second(input)
        return adventBase.Result(value)

    def __second(self, input: list):
        intList = helpers.convert_list_to_int(input)
        slidingWindow = self.__createSlidingWindow(intList)
        value = self.__countDepthIncreases(slidingWindow)
        return value

    # Helper methods for calculating
    def __createSlidingWindow(self, values: list) -> list:
        newValues = []
        for prev, current, next in zip(values, values[1:], values[2:]):
            sum = prev + current + next
            newValues.append(sum)
        return newValues

    def __countDepthIncreases(self, values) -> int:
        increases = 0
        for prev, current in zip(values, values[1:]):
            if (current > prev):
                increases += 1
        return increases
    
