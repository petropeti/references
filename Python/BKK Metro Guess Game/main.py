import pandas
import turtle

FONT = ('Calibri', 8, 'normal')


def new_screen():
    scr = turtle.Screen()
    scr.setup(width=622, height=695)
    scr.title("BKK Metro Guessing Game")
    scr.addshape("bkk_metro_ures.gif")
    turtle.shape("bkk_metro_ures.gif")
    return scr


screen = new_screen()
writer = turtle.Turtle()
writer.hideturtle()
writer.penup()
writer.color("black")

data = pandas.read_csv("stop_names")
stop_list = data.stop.to_list()
correct_guesses = set()
game_over = False
answer = screen.textinput(title="Guess a metro stop!", prompt="Name a stop: ").title()
while not game_over:
    if answer in stop_list:
        x = int(data[data.stop == answer].x.item())
        y = int(data[data.stop == answer].y.item())
        writer.goto(x, y)
        writer.write(arg=answer, align="left", font=FONT)
        correct_guesses.add(answer)
        if len(correct_guesses) == len(stop_list):
            game_over = True
            screen.textinput(title="Congratulations!", prompt="You guessed all the stops correctly!")
            break
    elif answer == "Exit":
        stops_to_learn = [stop for stop in stop_list if stop not in correct_guesses]
        pandas.DataFrame({"Stops": stops_to_learn}).to_csv("stops_to_learn")
        break
    answer = screen.textinput(title=f"{len(correct_guesses)}/{len(stop_list)} Stops Correct",
                              prompt="Name another stop: ").title()
