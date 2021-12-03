from common import adventBase
from common import helpers 


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
        return adventBase.TestResult(230, value)

    def second(self, input: list) -> adventBase.Result:
        value = self.__second(input)
        return adventBase.Result(value)

    def __second(self, input: list):
        ox = self.__find_rating(input, self.__oxygen)
        co2 = self.__find_rating(input, self.__CO2_scrubber)
        return ox * co2

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

    def __find_rating(self, input: list[str], rating_func):
        bit_index = 0
        while len(input) > 1:
            new_str = helpers.get_str_at_index(input, bit_index)

            strCount = len(new_str)
            ones = helpers.hamming_weight(new_str)

            filter_val = rating_func(strCount-ones, ones)
            input = list(filter(lambda bit: bit[bit_index] == filter_val, input))

            bit_index += 1
        
        item = input[0]
        return int(item, 2)

    def __oxygen(self, zeros, ones) -> str:
        if (zeros > ones):
            return '0'
        return '1'
    
    def __CO2_scrubber(self, zeros, ones) -> str:
        if (ones >= zeros):
            return '0'
        return '1'
