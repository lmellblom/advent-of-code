from common import adventBase
from common import helpers 
import numpy as np


class BingoNumber():
    def __init__(self, nr: int) -> None:
        self.nr = nr
        self.marked = False

class BingoBoard():
    def __init__(self, board) -> None:
        self.board = board
        self.has_bingo = False

    def mark_number(self, number: int) -> bool:
        for row in self.board:
            # found the nr that is
            found_nrs = filter(lambda field: field.nr == number, row)
            for found in found_nrs:
                found.marked = True

        return self.__has_bingo()

    def sum_unmarked_numbers(self) -> int:
        total = 0
        for row in self.board:
            unmarked = filter(lambda field: field.marked == False, row)
            unmarked_nums = map(lambda n: n.nr, unmarked)
            total += sum(unmarked_nums)
        return total

    def __has_bingo(self):
        for row in self.board:
            if (self.__is_marked(row)):
                self.has_bingo = True
                return True

        # flip and se if bingo in cols
        flipped = np.array(self.board)
        nrs = len(flipped[0])
        for i in range(0, nrs-1):
            col = flipped[:, i]
            if (self.__is_marked(col)):
                self.has_bingo = True
                return True
        
        # no found!
        return False

    def __is_marked(self, input):
        total_nr = len(input)
        all_marked = len(list(filter(lambda field: field.marked == True, input)))
        if (total_nr == all_marked):
            return True
        return False

    def print(self):
        for row in self.board:
            new_str = ''
            for nr in row:
                new = str(nr.nr)
                if (nr.marked == True):
                    new = '[' + str(nr.nr) + ']'
                
                new_str += new.ljust(6)
            print(new_str)
        print('')

class Day4(adventBase.AdventBase):
    def __init__(self) -> None:
        super().__init__('Giant Squid', 4, __file__)

    # --- First ---
    def test(self, input: list)-> adventBase.TestResult:
        value = self.__first(input)
        return adventBase.TestResult(4512, value)

    def first(self, input: list) -> adventBase.Result:
        value = self.__first(input)
        return adventBase.Result(value)

    def __first(self, input: list) -> int:
        (nr_to_draw, boards) = self.__convert_input(input)

        for nr in nr_to_draw:
            for board in boards:
                has_bingo = board.mark_number(nr)
                if (has_bingo):
                    unmarked_nr = board.sum_unmarked_numbers()
                    return unmarked_nr * nr

        return 0

    # --- Second ---
    def test2(self, input: list) -> adventBase.TestResult:
        value = self.__second(input)
        return adventBase.TestResult(1924, value)

    def second(self, input: list) -> adventBase.Result:
        value = self.__second(input)
        return adventBase.Result(value)

    def __second(self, input: list):
        (nr_to_draw, boards) = self.__convert_input(input)

        total_nr_of_boards = len(boards)
        boards_won = 0

        for nr in nr_to_draw:
            for board in boards:
                if (board.has_bingo):
                    continue

                has_bingo = board.mark_number(nr)
                
                if (has_bingo):
                    boards_won += 1

                    if (total_nr_of_boards == boards_won):
                        unmarked_nr = board.sum_unmarked_numbers()
                        return unmarked_nr * nr

        return 0

    # -- Helpers --
    def __convert_input(self, input: list[str]):
        # the first row is the drawn numbers
        new_list = input.copy()
        numbers_to_draw = list(map(int, new_list.pop(0).split(',')))
        new_list.pop(0)

        input_to_boards = []
        board_nr = 0
        input_to_boards.append([])
        for row in new_list:
            # new board appeared
            if (row == ''):
                board_nr += 1
                input_to_boards.append([])
                continue
                
            # convert the input to numbers
            numbers = list(map(int, (filter(lambda nr: nr != '', map(str.strip, row.split(' '))))))
            bingo_nrs = list(map(lambda n: BingoNumber(n), numbers))
            input_to_boards[board_nr].append(bingo_nrs)

        boards = []
        for input_board in input_to_boards:
            boards.append(BingoBoard(input_board))

        return (numbers_to_draw, boards)

