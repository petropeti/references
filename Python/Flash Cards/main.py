import pandas
import random
from tkinter import *
import os.path

FIRST_LANGUAGE = "English"
SECOND_LANGUAGE = "Hungarian"
FILE_NAME = "en-hu.csv"
FILE_TO_LEARN = "en-hu-to-learn.csv"

BACKGROUND_COLOR = "#B1DDC6"
FONT_ITALIC = ("Ariel", 40, "italic")
FONT_BOLD = ("Ariel", 60, "bold")

current_word = ""
current_translation = ""


# ----------- Data management --------------- #
def new_random_word():
    global current_word, current_translation
    current_word = random.choice(data[FIRST_LANGUAGE])
    current_translation = data[data[FIRST_LANGUAGE] == current_word][SECOND_LANGUAGE].item()

    language = canvas.itemcget(language_text, "text")
    if language == SECOND_LANGUAGE:
        flip_card()
    else:
        canvas.itemconfig(word_text, text=current_word)


def init_texts():
    canvas.itemconfig(language_text, text=FIRST_LANGUAGE)
    new_random_word()


def on_closing():
    data.to_csv(f"./data/{FILE_TO_LEARN}", index=False)
    window.destroy()


if os.path.exists(f"./data/{FILE_TO_LEARN}"):
    data = pandas.read_csv(f"./data/{FILE_TO_LEARN}")
else:
    data = pandas.read_csv(f"./data/{FILE_NAME}")


# ----------- Event methods --------------- #
def right_answer():
    global data
    data = data.drop(data[data[FIRST_LANGUAGE] == current_word].index)
    new_random_word()


def wrong_answer():
    new_random_word()


def flip_card():
    global current_word, current_translation
    language = canvas.itemcget(language_text, "text")
    if language == FIRST_LANGUAGE:
        canvas.itemconfig(card_img, image=card_back_image)
        canvas.itemconfig(language_text, text=SECOND_LANGUAGE, fill="white")
        canvas.itemconfig(word_text, text=current_translation, fill="white")
    else:
        canvas.itemconfig(card_img, image=card_front_image)
        canvas.itemconfig(language_text, text=FIRST_LANGUAGE, fill="black")
        canvas.itemconfig(word_text, text=current_word, fill="black")


# ----------- UI Setup --------------- #
window = Tk()
window.title("Flash Cards")
window.config(padx=50, pady=50, bg=BACKGROUND_COLOR)
window.resizable(False, False)

right_image = PhotoImage(file="./images/right.png")
wrong_image = PhotoImage(file="./images/wrong.png")
flip_image = PhotoImage(file="./images/flip.png")
card_front_image = PhotoImage(file="./images/card_front.png")
card_back_image = PhotoImage(file="./images/card_back.png")

canvas = Canvas(width=800, height=526, highlightthickness=0, bg=BACKGROUND_COLOR)
card_img = canvas.create_image(400, 263, image=card_front_image)
language_text = canvas.create_text(400, 150, text="Language", font=FONT_ITALIC, fill="black")
word_text = canvas.create_text(400, 263, text="Word", font=FONT_BOLD, fill="black")
canvas.grid(row=0, column=0, columnspan=3)

wrong_button = Button(image=wrong_image, highlightthickness=0, bg=BACKGROUND_COLOR, borderwidth=0,
                      command=wrong_answer)
wrong_button.grid(row=1, column=0)

right_button = Button(image=right_image, highlightthickness=0, bg=BACKGROUND_COLOR, borderwidth=0,
                      command=right_answer)
right_button.grid(row=1, column=2)

card_button = Button(image=flip_image, highlightthickness=0, bg=BACKGROUND_COLOR, borderwidth=0,
                     command=flip_card)
card_button.grid(row=1, column=1)

init_texts()

window.protocol("WM_DELETE_WINDOW", on_closing)
window.mainloop()
