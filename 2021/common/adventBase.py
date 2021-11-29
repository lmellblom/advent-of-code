import os

class AdventBase():
    def __init__(self, codeName: str, day: int, basepath =__file__):
        self.codeName = codeName
        self.day = day
        self.basepath = basepath

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