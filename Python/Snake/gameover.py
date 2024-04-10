from turtle import Turtle


class GameOver(Turtle):

    def __init__(self, final_score):
        super().__init__()
        self.color("white")
        self.hideturtle()
        self.write(arg=f"Game Over\nFinal highscore: {final_score}",
                   align="center",
                   font=("Courier", 30, "normal"))
