from turtle import Turtle
import random

WIDTH = 500
HEIGHT = 500


class Food(Turtle):

    def __init__(self):
        super().__init__()
        self.shape("circle")
        self.penup()
        self.shapesize(stretch_len=0.3, stretch_wid=0.3)
        self.color("blue")
        self.speed("fastest")
        x = random.randint(-(WIDTH // 2 - 20), WIDTH // 2 - 20)
        y = random.randint(-(HEIGHT // 2 - 20), HEIGHT // 2 - 20)
        self.goto(x, y)

    def relocate(self):
        x = random.randint(-(WIDTH // 2 - 20), WIDTH // 2 - 20)
        y = random.randint(-(HEIGHT // 2 - 20), HEIGHT // 2 - 20)
        self.goto(x, y)
