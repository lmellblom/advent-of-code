from common import adventBase
from common import helpers 

class BitSum:
    def __init__(self, zero: int, one: int) -> None:
        self.zero = zero
        self.one = one

    def common(self) -> int:
        if (self.zero > self.one):
            return 0
        return 1

    def least_common(self) -> int:
        if (self.one > self.zero):
            return 1
        return 0

class Day3(adventBase.AdventBase):
    def __init__(self) -> None:
        super().__init__('Binary Diagnostic', 3, __file__)

    # --- First ---
    def test(self, input: list)-> adventBase.TestResult:
        value = self.__first(input)
        return adventBase.TestResult(198, value)

    def first(self, input: list) -> adventBase.Result:
        value = self.__first(input)
        return adventBase.Result(value)

    def __first(self, input: list):
        gamma_str = self.__convert_input_to_gamma_str(input)
        gamma = int(gamma_str, 2)

        # kunde inte hitta något bättre...
        epsilon_str = ''.join(['1' if i == '0' else '0' for i in gamma_str])
        epsilon = int(epsilon_str, 2)
        
        return gamma * epsilon

    # --- Second ---
    def test2(self, input: list) -> adventBase.TestResult:
        value = self.__second(input)
        return adventBase.TestResult(0, value)

    def second(self, input: list) -> adventBase.Result:
        value = self.__second(input)
        return adventBase.Result(value)

    def __second(self, input: list):
        return None

    # -- Helpers --
    def __convert_input_to_gamma_str(self, input: list[str]) -> str:
        new_list = helpers.transpose(input)

        gamma_str = ''

        for item in new_list:
            strCount = len(item)
            ones = helpers.hamming_weight(item)

            if (strCount-ones > ones):
                gamma_str += '0'
            else:
                gamma_str += '1'

        return gamma_str