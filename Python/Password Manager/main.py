import json
from tkinter import *
from tkinter import messagebox
import password_generator
import pyperclip
import os

FILE = "data.json"


# ---------------------------- PASSWORD GENERATOR ------------------------------- #
def generate_password():
    password = password_generator.generate()
    password_entry.delete(0, END)
    password_entry.insert(END, password)
    pyperclip.copy(password)


def delete_passwords():
    if messagebox.askyesno(title="Delete Passwords",
                           message="Are you sure you want to delete all your previous password?"):
        os.remove("data.json")


# ---------------------------- SAVE PASSWORD ------------------------------- #
def add_password():
    website = website_entry.get()
    email = email_entry.get()
    password = password_entry.get()
    new_data = {
        website: {
            "email": email,
            "password": password,
        }
    }

    if website == "" or email == "" or password == "":
        messagebox.showinfo(title="Error", message="You left some fields empty.")
        return

    if messagebox.askokcancel(title="Save New Password",
                              message=f"Website: {website}\nUsername: {email}\nPassword: {password}\n"
                                      f"Do you want to save this?"):
        try:
            with open(FILE, mode="r") as file:
                data = json.load(file)
                data.update(new_data)
        except FileNotFoundError:
            with open(FILE, mode="w") as file:
                json.dump(new_data, file, indent=4)
        else:
            with open(FILE, mode="w") as file:
                json.dump(data, file, indent=4)
        finally:
            website_entry.delete(0, END)
            password_entry.delete(0, END)
            website_entry.focus()


def search_login():
    website = website_entry.get()
    try:
        with open(FILE, mode="r") as file:
            data = json.load(file)
    except FileNotFoundError:
        messagebox.showinfo(title="Error",
                            message=f"No data registered yet.")
        return
    else:
        if website in data:
            email = data[website]["email"]
            password = data[website]["password"]
            messagebox.showinfo(title=f"{website}",
                                message=f"Username: {email}\nPassword: {password}")
        else:
            messagebox.showinfo(title=f"Error",
                                message=f"No data registered for {website}.")


# ---------------------------- UI SETUP ------------------------------- #
window = Tk()
window.title("Password Manager")
window.config(padx=40, pady=40)
# window.minsize(width=500, height=500)
window.resizable(False, False)

logo = PhotoImage(file="logo.png")

canvas = Canvas(width=200, height=200, highlightthickness=0)
canvas.create_image(100, 100, image=logo)
canvas.grid(row=0, column=0, columnspan=3)

website_label = Label(text="Website:")
website_label.grid(row=1, column=0, sticky="e")
email_label = Label(text="Email/Username:")
email_label.grid(row=2, column=0, sticky="e")
password_label = Label(text="Password:")
password_label.grid(row=3, column=0, sticky="e")

website_entry = Entry(width=20)
website_entry.grid(row=1, column=1, columnspan=2, sticky="w")
website_entry.focus()
email_entry = Entry(width=20)
email_entry.grid(row=2, column=1, columnspan=2, sticky="w")
email_entry.insert(END, "popovics.hajnalka@gmail.com")
password_entry = Entry(width=20, show="*")
password_entry.grid(row=3, column=1, sticky="w")

generate_password_button = Button(text="Generate Password", width=11, command=generate_password)
generate_password_button.grid(row=3, column=2, sticky="e")

add_button = Button(text="Add", width=33, command=add_password)
add_button.grid(row=4, column=1, columnspan=2, sticky="w")

delete_passwords_button = Button(text="Delete passwords", width=33, command=delete_passwords)
delete_passwords_button.grid(row=5, column=1, columnspan=2, sticky="w")

search_button = Button(text="Search", width=11, command=search_login)
search_button.grid(row=1, column=2, sticky="e")

window.mainloop()
