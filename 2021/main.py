import pygame
import pygame_menu

from common.adventBase import AdventBase
from days.day1.day import Day1

# main functions
def getDaysList():
    days: list[AdventBase] = []
    days.append(Day1())
    return days

class MainAoC:
    daysClasses = []
    selectedDay = 1

    @staticmethod
    def initGame():
        pygame.init()
        surface = pygame.display.set_mode((600, 400))
        MainAoC.daysClasses = getDaysList()

        menu = pygame_menu.Menu('AdventOfCode', 400, 300,
                       theme=pygame_menu.themes.THEME_SOLARIZED)

        days = []
        for x in MainAoC.daysClasses:
            obj = (x.codeName, x.day)
            days.append(obj)

        menu.add.selector('Dag :', days, onchange=MainAoC.selectDay)

        menu.add.button('Play', MainAoC.start_the_game)
        menu.add.button('Quit', pygame_menu.events.EXIT)

        menu.mainloop(surface)

        submenu_theme = pygame_menu.themes.THEME_DEFAULT.copy()
        submenu_theme.widget_font_size = 15
        play_submenu = pygame_menu.Menu(
            height=WINDOW_SIZE[1] * 0.5,
            theme=submenu_theme,
            title='Submenu',
            width=WINDOW_SIZE[0] * 0.7
            )

    def selectDay(day, day2):
        MainAoC.selectedDay = day2

    def start_the_game():
        foundDat  = next((x for x in MainAoC.daysClasses if x.day == MainAoC.selectedDay), None)
        if (foundDat == None):
            print('not found')
        else:
            print(foundDat.codeName)
            foundDat.run()
        pass



def main():
    MainAoC.initGame()

# run the main function only if this module is executed as the main script
# (if you import this as a module then nothing is executed)
if __name__=="__main__":
    # call the main function
    main()


