# Labirynth
A simple but fun console-based labyrinth game written in C#. The player explores a randomly generated maze, collects items, fights monsters, and tries to find the exit. Nice to explore :D
🎮 Features

🔀 Random Maze Generation using depth-first search (DFS) algorithm with a stack.

👾 Monster Encounter – face a monster and survive if you have a sword.

🔑 Items to Collect:

F – Lantern (helps you navigate).

! – Key (needed to open the exit).

S – Sword (to defeat the monster).

P – Medkit (+50 HP).

🚪 Exit Mechanism:

% – Locked exit (requires the key).

X – Open exit to win the game.

❤️ Player Stats (HP, Inventory).

🧭 Dynamic Location Descriptions depending on where the player is.

🛠️ How It Works

The maze is generated fully from scratch with walls # and paths ..

Random items and enemies are placed inside free cells of the labyrinth.

The player moves using WASD keys.

The goal is to find the exit %, unlock it with the key !, and escape alive.

▶️ Controls

W → Move Up

A → Move Left

S → Move Down

D → Move Right

Q → Quit the game

⚙️ How to Run
Option 1: Run via dotnet run
cd lab2
dotnet run

Option 2: Build an executable

If you want a simple .exe (requires .NET installed on target machine):

dotnet publish -c Release -r win-x64 --self-contained false -p:PublishSingleFile=true


The .exe will appear in:

bin\Release\netX.0\win-x64\publish\

📷 Example Gameplay
###########
#..F....M.#
#.#.###.#.#
#.#...#.#.#
#S.#P.#.!.X
###########

📌 To-Do / Ideas

 Multiple monsters instead of one.

 Fog-of-war effect with lantern.

 Save/load game state.

 Better UI (colors, smoother movement).
