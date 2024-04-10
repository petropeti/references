from turtle import Turtle
from paddle import HEIGHT, WIDTH


class Scoreboard(Turtle):

    def __init__(self, player):
        super().__init__()
        self.score = 0
        self.player = player
        if self.player == 1:
            self.x_coord = -(WIDTH // 4)
        else:
            self.x_coord = WIDTH // 4
        self.color("white")
        self.hideturtle()
        self.write_score()

    def write_score(self):
        self.clear()
        self.teleport(self.x_coord, HEIGHT // 2 - 30)
        self.write(arg=f"Player {self.player}'s score: {self.score}", align="center", font=("Courier", 15, "normal"))

    def scored(self):
        self.score += 1
        self.write_score()
