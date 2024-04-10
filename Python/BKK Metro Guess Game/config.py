import pandas
import turtle

FONT = ('Calibri', 8, 'normal')


def new_screen():
    scr = turtle.Screen()
    scr.setup(width=622, height=695)
    scr.title("Configuration")
    scr.addshape("bkk_metro_ures.gif")
    turtle.shape("bkk_metro_ures.gif")
    return scr


screen = new_screen()
writer = turtle.Turtle()
writer.hideturtle()
writer.penup()
writer.color("black")

data_frame = pandas.read_csv("stop_names")
# data_dict = {row.letter: row.code for (index, row) in data_frame.iterrows()}

for (index, row) in data_frame.iterrows():
    x = int(row.x)
    y = int(row.y)
    stop = row.stop
    writer.goto(x, y)
    writer.write(arg=stop, align="left", font=FONT)

screen.exitonclick()
