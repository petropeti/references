import pandas
from turtle import *

screen = Screen()
screen.setup(width=622, height=695)
screen.title("Label Generator")
image = "bkk_metro.gif"
screen.addshape(image)
shape(image)


def mouse_click(x, y):
    n = screen.textinput(title="Name The Stops", prompt="Name: ").title()
    stops.append(n)
    xs.append(int(x))
    ys.append(int(y))
    print(cords)
    pandas.DataFrame(cords).to_csv("stop_names_new")


onscreenclick(mouse_click)
stops = []
xs = []
ys = []
cords = {
    "stop": stops,
    "x": xs,
    "y": ys,
}

mainloop()
