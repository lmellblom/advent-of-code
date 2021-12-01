import pygame
import pygame_menu

from datetime import datetime

from common.adventBase import AdventBase
from days.day1.day import Day1

# main functions
def getDaysList():
    days: list[AdventBase] = []
    days.append(Day1())
    return days

class MainAoC:
    daysToSelect = []
    selectedDay = datetime.now().day
    height = 400
    width = 700

    @staticmethod
    def initGame():
        pygame.init()
        surface = pygame.display.set_mode((MainAoC.width, MainAoC.height))
        MainAoC.daysToSelect = getDaysList()

        menu = pygame_menu.Menu('AdventOfCode', MainAoC.width, MainAoC.height,
                       theme=pygame_menu.themes.THEME_SOLARIZED)

        days = []
        for x in MainAoC.daysToSelect:
            obj = (x.codeName, x.day)
            days.append(obj)

        menu.add.selector('Välj problem :', days, onchange=MainAoC.selectDay)
        menu.add.button('KÖR', MainAoC.start_the_game)
        menu.add.button('Avsluta', pygame_menu.events.EXIT)

        menu.mainloop(surface)

        submenu_theme = pygame_menu.themes.THEME_DEFAULT.copy()
        submenu_theme.widget_font_size = 15
        play_submenu = pygame_menu.Menu(
            height=MainAoC.height,
            theme=submenu_theme,
            title='Submenu',
            width=MainAoC.width
            )

    def selectDay(dayTuple, dayNr):
        MainAoC.selectedDay = dayNr

    def start_the_game():
        foundDat  = next((x for x in MainAoC.daysToSelect if x.day == MainAoC.selectedDay), None)
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
