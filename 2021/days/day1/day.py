from common.adventBase import AdventBase

class Day1(AdventBase):
    def __init__(self) -> None:
        super().__init__('Dag1 - Titel', 1)

    def first(self):
        input = self.readInputFile()
        print(input)