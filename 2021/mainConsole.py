from datetime import datetime

from common.adventBase import AdventBase
from days.day1.day import Day1

# main functions
def getDaysList():
    days: list[AdventBase] = []
    days.append(Day1())
    return days

def run():
    days = getDaysList()
    currentDay = datetime.now().day
    dayIndex = currentDay - 1

    if (dayIndex >= len(days)):
        print('Day not implemented')
        return
    
    dayToRun = days[dayIndex]
    dayToRun.run()

# run the main function only if this module is executed as the main script
# (if you import this as a module then nothing is executed)
if __name__=="__main__":
    # call the main function
    # main()
    run()


