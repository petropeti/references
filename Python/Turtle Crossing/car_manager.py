import random
from turtle import Turtle

COLORS = ["red", "orange", "yellow", "green", "blue", "purple"]
STARTING_MOVE_DISTANCE = 5
MOVE_INCREMENT = 5
WIDTH = 600
HEIGHT = 600


class CarManager:

    def __init__(self):
        self.cars = []

    def new_car(self):
        c = Turtle()
        c.shape("square")
        c.shapesize(stretch_len=2, stretch_wid=1)
        c.color(random.choice(COLORS))
        c.penup()
        c.setheading(180)
        c.speed("fast")
        c.goto(WIDTH // 2 + 20, random.randint(-(HEIGHT // 2) + 70, HEIGHT // 2 - 70))
        self.cars.append(c)

    def move_cars(self, level):
        for c in self.cars:
            if c.xcor() < -(WIDTH // 2) - 20:
                self.cars.remove(c)
            c.forward(STARTING_MOVE_DISTANCE + (level - 1) * MOVE_INCREMENT)

    def turtle_crashed(self, turtle):
        for c in self.cars:
            if c.distance(turtle) < 15:
                return True
        return False
