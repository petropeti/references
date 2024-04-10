import time
from turtle import Screen
from player import Player
from car_manager import CarManager, WIDTH, HEIGHT
from scoreboard import Scoreboard

CAR_SPAWN_RATE = 6  # 1 is the fastest rate, 10 is the lowest rate


def new_screen():
    scr = Screen()
    scr.setup(width=WIDTH, height=HEIGHT)
    scr.bgcolor("white")
    scr.title("Turtle Crossing")
    scr.tracer(0)
    scr.listen()
    return scr


screen = new_screen()
player = Player()
score = Scoreboard()
car_manager = CarManager()

screen.onkey(player.move, "space")

game_over = False
i = 0
while not game_over:
    time.sleep(0.1)
    screen.update()
    if i % (CAR_SPAWN_RATE - score.level) == 0:
        car_manager.new_car()
    car_manager.move_cars(score.level)
    if player.finished():
        score.level_up()
        player.start()
    if car_manager.turtle_crashed(player):
        game_over = True
        score.write_game_over()
    i += 1

screen.exitonclick()
