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
    def __init__(self, codeName: str, day: int, basepath =__file__):
        self.codeName = codeName
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
        print('-----------')
        print('--', self.codeName , '--')
        print('-----------')
        testInput = self.__readTestFile()
        input = self.__readInputFile()

        testResult1 = self.test(testInput)
        if (testResult1.succeded):
            result1 = self.first(input)
            self.writeResultMessage('Part1', result1)
        else:
            self.writeTestResultMessage('Test1', testResult1)

        testResult2 = self.test2(testInput)
        if (testResult2.succeded):
            result2 = self.second(input)
            self.writeResultMessage('Part2', result2)
        else:
            self.writeTestResultMessage('Test2', testResult2)
        
    def writeResultMessage(self, message: str, result: Result):
        if (result is not None): 
            print(message, ' answer :', result.value)

    def writeTestResultMessage(self, message: str, result: TestResult): 
        if (result is not None): 
            if result.succeded:
                print(message, ' succeded!')
            else:
                print(message, ' failed :/')
                print('Value: ', result.value)
                print('Expected: ', result.expected)
                pass

    def __readInputFile(self):
        return self.__readFile('input.txt')

    def __readTestFile(self):
        return self.__readFile('input_test.txt')

    def __current_path(self):
        pth, _ = os.path.split(os.path.abspath(self.basepath))
        return pth

    def __readFile(self, fileName):
        dirname = self.__current_path()
        filePath = os.path.join(dirname, fileName)

        f = open(filePath, "r")

        #loop trough the file line by line? store in string list
        rows = []
        for row in f:
            rows.append(row.strip())
        return rows