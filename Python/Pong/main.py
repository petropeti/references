from turtle import Screen, Turtle
from paddle import Paddle, WIDTH, HEIGHT
from ball import Ball
from scoreboard import Scoreboard

SCORE_TO_WIN = 3


def new_screen():
    scr = Screen()
    scr.setup(width=WIDTH, height=HEIGHT)
    scr.bgcolor("black")
    scr.title("Pong")
    scr.tracer(0)
    scr.listen()
    t = Turtle()
    t.color("white")
    t.hideturtle()
    t.pensize(5)
    t.teleport(0, - (HEIGHT // 2))
    t.setheading(90)
    while t.ycor() < HEIGHT // 2:
        t.forward(15)
        t.penup()
        t.forward(15)
        t.pendown()
    return scr


def player_won(num):
    p1_score.clear()
    p2_score.clear()
    if num == 1:
        x_coord = -(WIDTH // 4)
    else:
        x_coord = WIDTH // 4
    t = Turtle()
    t.color("white")
    t.hideturtle()
    t.teleport(x_coord, 0)
    t.write(arg=f"Player{num} Won!", align="center", font=("Courier", 20, "normal"))


screen = new_screen()
player1 = Paddle(-(WIDTH // 2) + 35, HEIGHT // 4)
player2 = Paddle((WIDTH // 2) - 50, HEIGHT // 4)
p1_score = Scoreboard(1)
p2_score = Scoreboard(2)

screen.onkey(player1.move_up, "w")
screen.onkey(player1.move_down, "s")
screen.onkey(player2.move_up, "Up")
screen.onkey(player2.move_down, "Down")

ball = Ball()

game_over = False
while not game_over:
    screen.update()
    ball.move()
    if ball.check_collision_with_wall():
        ball.bounce_from_wall()
    if ball.check_collision_with_paddle(player1):
        ball.bounce_from_paddle("right")
    if ball.check_collision_with_paddle(player2):
        ball.bounce_from_paddle("left")
    if ball.hit_left_edge():
        p2_score.scored()
        if p2_score.score >= SCORE_TO_WIN:
            player_won(2)
            game_over = True
        ball.start("right")
    if ball.hit_right_edge():
        p1_score.scored()
        if p1_score.score >= SCORE_TO_WIN:
            player_won(1)
            game_over = True
        ball.start("left")

screen.exitonclick()
