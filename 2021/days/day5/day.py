from common import adventBase
from common import helpers 

class Coordinate():
    def __init__(self, x, y) -> None:
        self.x = x
        self.y = y
    
    def get_key(self) -> str:
        return '{},{}'.format(self.x, self.y)

class LineString():
    def __init__(self, start: Coordinate, end: Coordinate) -> None:
        self.start = start
        self.end = end

    def is_horizontal_or_vertical(self) -> bool:
        return self.start.x == self.end.x or self.start.y == self.end.y

    def get_all_points(self) -> list[Coordinate]:
        calculated_coords = self.__get_points()
        
        coords = []
        coords.append(self.start)
        for coord in calculated_coords:
            coords.append(coord)
        coords.append(self.end)

        return coords

    def __get_points(self) -> list[Coordinate]:
        # init variables
        coords = []
        dx = 0
        dy = 0
        length = 0

        # determine slope
        if (self.start.x == self.end.x):
            length = self.end.y - self.start.y
            dy = 1 if length > 0 else -1
        elif (self.start.y == self.end.y):
            length = self.end.x - self.start.x
            dx = 1 if length > 0 else -1
        else:
            length = self.end.y - self.start.y
            dx = 1 if (self.end.x - self.start.x) > 0 else -1
            dy = 1 if (self.end.y - self.start.y) > 0 else -1

        # start values
        x = self.start.x
        y = self.start.y
        for i in range(1, abs(length)):
            x += dx
            y += dy
            coords.append(Coordinate(x,y))

        return coords

class Area():
    def __init__(self) -> None:
        self.points = {}

    def add_line_strings(self, lines: list[LineString]):
        for line in lines:
            coords = line.get_all_points()
            for coord in coords:
                self.add_coord(coord)

    def add_coord(self, coord: Coordinate):
        # check if exists in the points
        key = coord.get_key()
        if (key not in self.points):
            self.points[key] = 0
        
        self.points[key] = self.points[key] + 1

    def nr_of_lines_overlap(self, value: int) -> int:
        all_occupied_coord_values_in_area = self.__get_values()
        overlaped = len(list(filter(lambda v: v >= value, all_occupied_coord_values_in_area)))
        return overlaped

    def __get_values(self) -> list[int]:
        values = list(self.points.values())
        return values

class Day5(adventBase.AdventBase):
    def __init__(self) -> None:
        super().__init__('Hydrothermal Venture', 5, __file__)

    # --- First ---
    def test(self, input: list)-> adventBase.TestResult:
        value = self.__first(input)
        return adventBase.TestResult(5, value)

    def first(self, input: list) -> adventBase.Result:
        value = self.__first(input)
        return adventBase.Result(value)

    def __first(self, input: list) -> int:
        line_strings = self.__convert_input(input)
        line_strings = list(filter(lambda line: line.is_horizontal_or_vertical(), line_strings))
        
        area = Area()
        area.add_line_strings(line_strings)

        overlaps = area.nr_of_lines_overlap(2)
        return overlaps

    # --- Second ---
    def test2(self, input: list) -> adventBase.TestResult:
        value = self.__second(input)
        return adventBase.TestResult(12, value)

    def second(self, input: list) -> adventBase.Result:
        value = self.__second(input)
        return adventBase.Result(value)

    def __second(self, input: list):
        line_strings = self.__convert_input(input)
        
        area = Area()
        area.add_line_strings(line_strings)
        
        overlaps = area.nr_of_lines_overlap(2)
        return overlaps

    # -- Helpers --
    def __convert_input(self, input: list[str]) -> list[LineString]:
        line_strings = []
        for line in input:
            coordinates = list(map(str.strip, line.split('->')))
            start = list(map(int, coordinates[0].split(',')))
            end = list(map(int, coordinates[1].split(',')))
            line_string = LineString(Coordinate(start[0], start[1]), Coordinate(end[0], end[1]))
            line_strings.append(line_string)
        return line_strings

