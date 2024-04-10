from turtle import Turtle
from paddle import WIDTH, HEIGHT
import random

SPEED = 5  # 1 - slowest, 5 - fastest


class Ball(Turtle):

    def __init__(self):
        super().__init__()
        self.starting_degrees_to_left = []
        self.starting_degrees_to_right = []
        self.generate_starting_degrees()
        self.shape("circle")
        self.penup()
        self.shapesize(stretch_len=0.5, stretch_wid=0.5)
        self.color("white")
        self.speed("fast")
        self.start("right")

    def generate_starting_degrees(self):
        for i in range(0, 60):
            self.starting_degrees_to_right.append(i)
        for i in range(120, 240):
            self.starting_degrees_to_left.append(i)
        for i in range(300, 360):
            self.starting_degrees_to_right.append(i)

    def start(self, side):
        self.home()
        if side == "left":
            self.setheading(random.choice(self.starting_degrees_to_left))
        else:
            self.setheading(random.choice(self.starting_degrees_to_right))

    def move(self):
        self.forward(SPEED)

    def check_collision_with_wall(self):
        return abs(self.ycor()) > HEIGHT / 2 - 5

    def check_collision_with_paddle(self, paddle):
        for s in paddle.segments:
            if self.distance(s) < 20:
                return True
        return False

    def bounce_from_wall(self):
        self.setheading(-self.heading())

    def bounce_from_paddle(self, side):
        if side == "left":
            self.setheading(random.choice(self.starting_degrees_to_left))
        else:
            self.setheading(random.choice(self.starting_degrees_to_right))

    def hit_left_edge(self):
        return self.xcor() < - (WIDTH // 2) + 20

    def hit_right_edge(self):
        return self.xcor() > (WIDTH // 2) - 20
