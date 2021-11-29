import os
import inspect


class AdventBase():

    def __init__(self, codeName: str, day: int):
        self.codeName = codeName
        self.day = day

    def readInputFile(self):
        return self.__readFile('input.txt')

    def readTestFile(self):
        return self.__readFile('input_test.txt')

    def first(self):
        print('not impl.')

    def second(self):
        print('not impl.')

    def run(self):
        self.first()
        self.second()

    def __readFile(self, fileName):
        fileDir = os.path.dirname(os.path.realpath('__file__'))
        print(fileDir)

        dirname = os.path.dirname(os.path.abspath(inspect.stack()[0][1]))
        filename2 = os.path.join(dirname, fileName)
        print(filename2)

        f = open(fileName, "r")
        #loop trough the file line by line? store in string list
        rows = []
        for row in f:
            rows.append(row.strip())
        return rows