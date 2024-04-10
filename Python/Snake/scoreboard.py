from turtle import Turtle
from food import HEIGHT

FONT = ("Courier", 20, "bold")


class Scoreboard(Turtle):

    def __init__(self):
        super().__init__()
        self.score = 0
        self.highscore = 0
        self.color("white")
        self.hideturtle()
        self.teleport(0, HEIGHT / 2 - 30)
        self.update_scoreboard()

    def refresh_score(self):
        self.score += 1
        self.update_scoreboard()

    def update_scoreboard(self):
        self.clear()
        self.write(arg=f"Score: {self.score} Highscore: {self.highscore}", align="center", font=FONT)

    def game_over(self):
        if self.score > self.highscore:
            self.highscore = self.score
        self.score = 0
        self.update_scoreboard()
