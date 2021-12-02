import pygame
import pygame_menu

from datetime import datetime

from common.adventBase import AdventBase
from days.day1.day import Day1
from days.day2.day import Day2

from game import sprites

# main functions
def get_days_list():
    days: list[AdventBase] = []
    days.append(Day1())
    days.append(Day2())
    return days

class MainAoC:
    days_to_select = []
    selected_day = datetime.now().day
    height = 1200 / 2
    width = 1920 / 2

    @staticmethod
    def init_game():
        pygame.init()
        surface = pygame.display.set_mode((MainAoC.width, MainAoC.height))
        MainAoC.days_to_select = get_days_list()

        menu = pygame_menu.Menu('AdventOfCode', MainAoC.width, MainAoC.height,
                       theme=pygame_menu.themes.THEME_SOLARIZED)

        days = []
        for x in MainAoC.days_to_select:
            obj = (x.code_name, x.day)
            days.append(obj)

        menu.add.selector('Välj problem :', days, onchange=MainAoC.select_day)
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

    def init_calendar():
        pygame.init()
        surface = pygame.display.set_mode((MainAoC.width, MainAoC.height))

        bg = sprites.Background('christmas.jpg', (0,0))

        days = sprites.DaySprit('day.png', (10,10), 1)
        
        # define a variable to control the main loop
        running = True
        
        # main loop
        while running:
            bg.draw(surface)
            days.draw(surface)

            pygame.display.flip() # update the display

            # event handling, gets all event from the event queue
            for event in pygame.event.get():
                # only do something if the event is of type QUIT
                if event.type == pygame.QUIT:
                    # change the value to False, to exit the main loop
                    running = False


    def select_day(day_tuple, day_nr):
        MainAoC.selected_day = day_nr

    def start_the_game():
        found_day  = next((x for x in MainAoC.days_to_select if x.day == MainAoC.selected_day), None)
        if (found_day == None):
            print('not found')
        else:
            print(found_day.code_name)
            found_day.run()
        pass

def main():
    MainAoC.init_game()
    # MainAoC.init_calendar()

# run the main function only if this module is executed as the main script
# (if you import this as a module then nothing is executed)
if __name__=="__main__":
    # call the main function
    main()
