from turtle import Turtle
import time

SPEED = 9.0  # 0 is slowest, 10 is fastest
SNAKE_START_LENGTH = 3


class Snake:
    def __init__(self):
        self.segments = []
        self.create_snake()
        self.head = self.segments[0]

    def create_snake(self):
        for _ in range(SNAKE_START_LENGTH):
            t = Turtle()
            t.fillcolor("white")
            t.shape("square")
            t.shapesize(stretch_len=0.5, stretch_wid=0.5)
            t.penup()
            t.goto(x=0 - len(self.segments) * 10, y=0)
            self.segments.append(t)

    def size_increase(self):
        t = Turtle()
        t.fillcolor("white")
        t.shape("square")
        t.shapesize(stretch_len=0.5, stretch_wid=0.5)
        t.penup()
        x = self.segments[-1].xcor()
        y = self.segments[-1].ycor()
        t.goto(x, y)
        self.segments.append(t)

    def move(self):
        i = len(self.segments) - 1
        while i > 0:
            x = self.segments[i - 1].pos()[0]
            y = self.segments[i - 1].pos()[1]
            self.segments[i].goto(x, y)
            i -= 1
        self.head.forward(10)
        time.sleep(1 - SPEED / 10)

    def reset(self):
        for t in self.segments:
            t.goto(1000, 1000)
        self.segments.clear()
        self.create_snake()
        self.head = self.segments[0]

    def hide_snake(self):
        for t in self.segments:
            t.hideturtle()

    def up(self):
        if self.head.heading() in [0, 180]:
            self.head.setheading(90)

    def down(self):
        if self.head.heading() in [0, 180]:
            self.head.setheading(270)

    def left(self):
        if self.head.heading() in [90, 270]:
            self.head.setheading(180)

    def right(self):
        if self.segments[0].heading() in [90, 270]:
            self.segments[0].setheading(0)
