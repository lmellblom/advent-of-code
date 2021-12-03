from datetime import datetime

from common.adventBase import AdventBase
from days.day1.day import Day1
from days.day2.day import Day2
from days.day3.day import Day3

# main functions
def get_days_list():
    days: list[AdventBase] = []
    days.append(Day1())
    days.append(Day2())
    days.append(Day3())
    return days

def run():
    days = get_days_list()
    current_day = datetime.now().day
    day_index = current_day - 1

    if (day_index >= len(days)):
        print('Day not implemented')
        return
    
    day_to_run = days[day_index]
    day_to_run.run()

# run the main function only if this module is executed as the main script
# (if you import this as a module then nothing is executed)
if __name__=="__main__":
    # call the main function
    # main()
    run()


