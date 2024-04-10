from turtle import Turtle, Screen
import random


def new_turtle(color):
    turtle = Turtle()
    global turtle_count, turtles
    turtle_count += 1
    turtles.append(turtle)
    turtle.color(color)
    turtle.shape("turtle")
    turtle.penup()
    turtle.goto(x=-220, y=200 - turtle_count * 30)
    return turtle


def turtle_race(ts):
    while True:
        for t in ts:
            t.forward(random.randint(5, 15))
        winner = check_turtles(ts)
        if winner != "":
            return winner


def check_turtles(ts):
    for t in ts:
        if t.xcor() >= 200:
            return t.fillcolor()
    return ""


def new_screen():
    scr = Screen()
    scr.setup(width=500, height=400)
    t = Turtle()
    t.hideturtle()
    t.speed(0)
    t.color("black")
    t.pensize(5)
    t.teleport(x=220, y=-200)
    t.left(90)
    t.forward(400)
    return scr


screen = new_screen()
guess = screen.textinput(title="Make a bet!", prompt="Which turtle will win the race?: ")
turtle_count = 0
turtles = []
red = new_turtle("red")
blue = new_turtle("blue")
yellow = new_turtle("yellow")
pink = new_turtle("pink")
green = new_turtle("green")
black = new_turtle("black")
orange = new_turtle("orange")
brown = new_turtle("brown")

winner_turtle = turtle_race(turtles)
if winner_turtle == guess:
    print("You guessed correctly!")
else:
    print("Incorrect guess.")
screen.exitonclick()
