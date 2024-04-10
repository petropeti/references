from turtle import Turtle

PADDLE_LENGTH = 5
WIDTH = 800
HEIGHT = 500


class Paddle:

    def __init__(self, x, y):
        self.segments = []
        self.x_coord = x
        self.y_starting_coord = y
        self.create_paddle()

    def create_paddle(self):
        for _ in range(PADDLE_LENGTH):
            t = Turtle()
            t.fillcolor("white")
            t.shape("square")
            t.speed("fastest")
            t.penup()
            t.goto(x=self.x_coord, y=self.y_starting_coord - len(self.segments) * 20)
            self.segments.append(t)

    def move_up(self):
        if self.segments[0].ycor() < HEIGHT//2 - 20:
            for s in self.segments:
                s.setheading(90)
                s.forward(20)

    def move_down(self):
        if self.segments[-1].ycor() > -(HEIGHT//2) + 30:
            for s in self.segments:
                s.setheading(-90)
                s.forward(20)
