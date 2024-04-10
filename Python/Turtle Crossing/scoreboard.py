from turtle import Turtle
from car_manager import HEIGHT, WIDTH

FONT = ("Courier", 24, "normal")


class Scoreboard(Turtle):

    def __init__(self):
        super().__init__()
        self.level = 1
        self.color("black")
        self.hideturtle()
        self.write_score()

    def write_score(self):
        self.clear()
        self.teleport(-(WIDTH // 2) + 20, (HEIGHT // 2) - 40)
        self.write(arg=f"Level: {self.level}", align="left", font=FONT)

    def level_up(self):
        self.level += 1
        self.write_score()

    def write_game_over(self):
        self.home()
        self.write(arg="Game Over", align="center", font=FONT)
