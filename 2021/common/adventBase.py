import os

from abc import ABC, abstractmethod

class Result():
    def __init__(self, value) -> None:
        self.value = value

class TestResult():
     def __init__(self, expected, value):
        self.succeded = expected == value
        self.expected = expected
        self.value = value

class AdventBase(ABC):
    def __init__(self, code_name: str, day: int, basepath =__file__):
        self.code_name = code_name
        self.day = day
        self.basepath = basepath

    @abstractmethod
    def first(self, input: list) -> Result:
        pass

    @abstractmethod
    def second(self, input: list) -> Result:
        pass

    @abstractmethod
    def test(self, input: list) -> TestResult:
        return TestResult('-', '')

    @abstractmethod
    def test2(self, input: list) -> TestResult:
         return TestResult('-', '')

    def run(self):
        length = 20
        lines = '-'.center(length, '-')
        title = self.code_name.center(length)
        print(lines)
        print(title)
        print(lines)
        testInput = self.__read_test_file()
        input = self.__read_input_file()

        testResult1 = self.test(testInput)
        if (testResult1.succeded):
            result1 = self.first(input)
            self.write_result_message('Part1', result1)
        else:
            self.write_test_result_message('Test1', testResult1)

        print(lines)

        testResult2 = self.test2(testInput)
        if (testResult2.succeded):
            result2 = self.second(input)
            self.write_result_message('Part2', result2)
        else:
            self.write_test_result_message('Test2', testResult2)
        
    def write_result_message(self, message: str, result: Result):
        if (result is not None): 
            print(message, ' answer :', result.value)

    def write_test_result_message(self, message: str, result: TestResult): 
        if (result is not None): 
            if result.succeded:
                print(message, ' succeded!')
            else:
                print(message, ' failed :/')
                print('Value'.ljust(10), ':', result.value)
                print('Expected'.ljust(10), ':',result.expected)
                pass

    def __read_input_file(self):
        return self.__read_file('input.txt')

    def __read_test_file(self):
        return self.__read_file('input_test.txt')

    def __current_path(self):
        pth, _ = os.path.split(os.path.abspath(self.basepath))
        return pth

    def __read_file(self, fileName):
        dirname = self.__current_path()
        filePath = os.path.join(dirname, fileName)

        f = open(filePath, "r")

        #loop trough the file line by line? store in string list
        rows = []
        for row in f:
            rows.append(row.strip())
        return rows