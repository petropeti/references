from car_manager import HEIGHT
from turtle import Turtle

STARTING_POSITION = (0, -(HEIGHT // 2) + 20)
MOVE_DISTANCE = 10
FINISH_LINE_Y = HEIGHT // 2 - 20


class Player(Turtle):

    def __init__(self):
        super().__init__()
        self.shape("turtle")
        self.penup()
        self.color("green")
        self.speed("fastest")
        self.start()

    def start(self):
        self.goto(STARTING_POSITION)
        self.setheading(90)

    def move(self):
        self.forward(MOVE_DISTANCE)

    def finished(self):
        return self.ycor() > FINISH_LINE_Y
