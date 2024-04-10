from tkinter import *

# ---------------------------- CONSTANTS ------------------------------- #
PINK = "#e2979c"
RED = "#e7305b"
GREEN = "#9bdeac"
YELLOW = "#f7f5dd"
FONT_NAME = "Courier"
WORK_MIN = 25
SHORT_BREAK_MIN = 5
LONG_BREAK_MIN = 20
reps = 0
timer = None


# ---------------------------- TIMER RESET ------------------------------- #
def reset_timer():
    global reps, timer
    window.after_cancel(str(timer))
    reps = 0
    title_label.config(text="Timer")
    checkmark_label.config(text="")
    start_button.config(state=NORMAL)
    reset_button.config(state=DISABLED)
    canvas.itemconfig(timer_text, text="0:00")


# ---------------------------- TIMER MECHANISM ------------------------------- #

def start_timer():
    global reps
    reps = (reps % 8) + 1
    work_sec = int(WORK_MIN * 60)
    s_break_sec = int(SHORT_BREAK_MIN * 60)
    l_break_sec = int(LONG_BREAK_MIN * 60)
    if reps == 8:
        title_label.config(text="Break", fg=RED)
        count_down(l_break_sec)
    elif reps % 2 == 0:
        title_label.config(text="Break", fg=PINK)
        count_down(s_break_sec)
    else:
        title_label.config(text="Work", fg=GREEN)
        count_down(work_sec)
    start_button.config(state=DISABLED)
    reset_button.config(state=NORMAL)


# ---------------------------- COUNTDOWN MECHANISM ------------------------------- #
def count_down(count):
    global reps, timer
    minute = count // 60
    second = count % 60
    if second < 10:
        second_string = "0" + str(second)
    else:
        second_string = str(second)
    canvas.itemconfig(timer_text, text=f"{minute}:{second_string}")
    if count > 0:
        timer = window.after(1000, count_down, count - 1)
    else:
        start_timer()
        if reps % 2 == 0:
            new_text = checkmark_label["text"] + "✔"
            checkmark_label.config(text=f"{new_text}")


# ---------------------------- GUI SETUP ------------------------------- #
window = Tk()
window.title("Pomodoro")
window.config(padx=100, pady=50, bg=YELLOW)
window.resizable(False, False)

image = PhotoImage(file="tomato.png")
canvas = Canvas(width=200, height=224, bg=YELLOW, highlightthickness=0)
canvas.create_image(100, 112, image=image)
timer_text = canvas.create_text(103, 130, text="0:00", font=(FONT_NAME, 35, "bold"), fill="white")
canvas.grid(row=1, column=1)

title_label = Label(text="Timer", font=(FONT_NAME, 50, "bold"), fg=GREEN, bg=YELLOW)
title_label.grid(row=0, column=1)

start_button = Button(text="Start", font=(FONT_NAME, 20, "bold"), highlightthickness=0, command=start_timer,
                      bg=GREEN)
start_button.grid(row=2, column=0)

reset_button = Button(text="Reset", font=(FONT_NAME, 20, "bold"), highlightthickness=0, command=reset_timer,
                      state=DISABLED, bg=GREEN)
reset_button.grid(row=2, column=2)

checkmark_label = Label(text="", font=(FONT_NAME, 30, "normal"), fg=GREEN, bg=YELLOW)
checkmark_label.grid(row=3, column=1)

window.mainloop()
