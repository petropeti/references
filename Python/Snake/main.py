from turtle import Screen
from snake import Snake
from food import Food, HEIGHT, WIDTH
from scoreboard import Scoreboard
from gameover import GameOver


def new_screen():
    scr = Screen()
    scr.setup(width=WIDTH, height=HEIGHT)
    scr.bgcolor("black")
    scr.title("Snake")
    scr.tracer(0)
    scr.listen()
    return scr


def check_food_collision():
    if snake.head.distance(food) < 8:
        food.relocate()
        return True
    return False


def check_wall_collision():
    return (abs(snake.head.xcor()) > WIDTH / 2 - 5) or (abs(snake.head.ycor()) > HEIGHT / 2 - 5)


def check_tail_collision():
    for s in snake.segments[1:]:
        if snake.head.distance(s) == 0:
            return True
    return False


def end_game():
    GameOver(scoreboard.highscore)
    snake.hide_snake()
    food.hideturtle()
    screen.update()
    global game_over
    game_over = True


screen = new_screen()
snake = Snake()
food = Food()
scoreboard = Scoreboard()

screen.onkey(snake.up, "Up")
screen.onkey(snake.down, "Down")
screen.onkey(snake.left, "Left")
screen.onkey(snake.right, "Right")
screen.onkey(snake.up, "w")
screen.onkey(snake.down, "s")
screen.onkey(snake.left, "a")
screen.onkey(snake.right, "d")

screen.onkey(end_game, "0")

with open("highscore.txt", mode="r") as hs:
    scoreboard.highscore = int(hs.read())
    scoreboard.update_scoreboard()

game_over = False
while not game_over:
    screen.update()
    snake.move()
    if check_food_collision():
        scoreboard.refresh_score()
        snake.size_increase()
    if check_wall_collision() or check_tail_collision():
        scoreboard.game_over()
        with open("highscore.txt", mode="w") as hs:
            hs.write(str(scoreboard.highscore))
        snake.reset()

screen.exitonclick()
