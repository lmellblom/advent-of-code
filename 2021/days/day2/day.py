from common import adventBase
from common import helpers 

class Command():
    def __init__(self, direction: str, value: int) -> None:
        self.direction = direction
        self.value = value

class Submarine():
    def __init__(self, startDepth, startHorizontal) -> None:
        self.depth = startDepth
        self.horizontal = startHorizontal

    def apply_command(self, command: Command) -> None:
        if (command.direction == 'forward'):
            self.forward(command.value)
        elif (command.direction == 'down'):
            self.down(command.value)
        elif (command.direction == 'up'):
            self.up(command.value)

    def forward(self, value) -> None:
        self.horizontal += value
    
    def down(self, value) -> None:
        self.depth += value

    def up(self, value) -> None:
        self.depth -= value

class SubmarineV2(Submarine):
    def __init__(self, startDepth, startHorizontal, startAim) -> None:
        super().__init__(startDepth, startHorizontal)
        self.aim = startAim
        
    def forward(self, value) -> None:
        self.horizontal += value
        self.depth += self.aim * value
    
    def down(self, value) -> None:
        self.aim += value

    def up(self, value) -> None:
        self.aim -= value

class Day2(adventBase.AdventBase):
    def __init__(self) -> None:
        super().__init__('Dive!', 2, __file__)

    # --- First ---
    def test(self, input: list)-> adventBase.TestResult:
        value = self.__first(input)
        return adventBase.TestResult(150, value)

    def first(self, input: list) -> adventBase.Result:
        value = self.__first(input)
        return adventBase.Result(value)

    def __first(self, input: list):
        submarine = Submarine(0,0)
        return self.__run_submarine(submarine, input)

    # --- Second ---
    def test2(self, input: list) -> adventBase.TestResult:
        value = self.__second(input)
        return adventBase.TestResult(900, value)

    def second(self, input: list) -> adventBase.Result:
        value = self.__second(input)
        return adventBase.Result(value)

    def __second(self, input: list):
        submarine = SubmarineV2(0,0,0)
        return self.__run_submarine(submarine, input)

    # -- Helpers --
    def __run_submarine(self, submarine: Submarine, input: list[str]):
        commands = self.__convertInputToCommands(input)
        for command in commands:
            submarine.apply_command(command)
        return submarine.depth * submarine.horizontal

    def __convertInputToCommands(self, input: list[str]) -> list[Command]:
        commands = []
        for row in input:
            values = row.split()
            commands.append(Command(values[0], int(values[1])))
        return commands
