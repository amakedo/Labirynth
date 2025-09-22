# 🧱 Гра "Лабіринт"  

Цей проєкт — це консольна гра на C#, у якій гравець досліджує випадково згенерований лабіринт, збирає предмети та намагається знайти вихід.  
Гра працює прямо в терміналі й не потребує додаткових бібліотек.  

---

## 🎮 Особливості гри  

- 🔀 Генерація випадкового лабіринту (алгоритм DFS зі стеком).  
- 🚶 Рух гравця по клітинках із відображенням у консолі.  
- 🎒 Система інвентарю:  
  - можна підбирати предмети;  
  - переглядати їх описи;  
  - використовувати.  
- ❌ Вихід з лабіринту позначається `X`.  
- 📜 Всі стіни та шляхи формуються автоматично при запуску гри.  

---

## ⚙️ Алгоритм роботи  

1. Спочатку вся мапа заповнюється стінами.  
2. Генератор випадковим чином створює прохід, використовуючи **DFS** (пошук у глибину) зі стеком.  
3. У деяких клітинках розміщуються предмети.  
4. Гравець починає в стартовій точці та може рухатися стрілками.  
5. Якщо гравець досягає `X` — гра закінчується перемогою.  

---

## 🚀 Запуск  

### Варіант 1. Запуск із Visual Studio Code  

1. Клонувати репозиторій або завантажити код.  
2. Відкрити папку з проектом у **VS Code**.  
3. Виконати команду:  

```bash
dotnet run
```

### Варіант 2. Збірка у `.exe` (Windows)  

1. Виконати у терміналі:  

```bash
dotnet publish -c Release -r win-x64 --self-contained false
```

2. У папці `bin/Release/net8.0/win-x64/publish` з’явиться `.exe` файл.  
3. Запустіть його подвійним кліком або через консоль.  

---

## 📂 Структура проекту  

- `Program.cs` — основний файл із логікою гри.
- `README.md` — документація (цей файл).  

---

## 📊 Приклад роботи  

```
###########
#@   #   X#
### # # ###
#   #     #
# ### ### #
#     #   #
###########
```

- `@` — гравець.  
- `#` — стіна.  
- `X` — вихід.  

---

## 📌 Плани на майбутнє  

- Додати різні типи предметів (ключі, зброю, їжу).  
- Реалізувати ворогів.  
- Зробити кольорове відображення для кращої читабельності.  


---

# 🏰 Maze Game (English Version)

This project is a **console game in C#** where the player explores a randomly generated labyrinth, collects items, and tries to find the exit.  
The game runs directly in the terminal and does not require additional libraries.  

---

## 🎮 Features  

- 🔀 Random maze generation (DFS algorithm with a stack).  
- 🚶 Player movement inside the maze displayed in the console.  
- 🎒 Inventory system:  
  - pick up items;  
  - view descriptions;  
  - use items.  
- ❌ Exit is marked with `X`.  
- 📜 All walls and corridors are generated automatically at game start.  

---

## ⚙️ How It Works  

1. The entire map is initially filled with walls.  
2. The generator carves passages using **DFS (depth-first search)** with a stack.  
3. Items are placed randomly in some cells.  
4. The player starts at the initial point and can move using the keyboard.  
5. If the player reaches `X`, the game ends with victory.  

---

## 🚀 Run Instructions  

### Option 1: Run with Visual Studio Code  

1. Clone the repository or download the code.  
2. Open the folder in **VS Code**.  
3. Run the command:  

```bash
dotnet run
```

### Option 2: Build as `.exe` (Windows)  

1. Run in terminal:  

```bash
dotnet publish -c Release -r win-x64 --self-contained false
```

2. The `.exe` file will appear in:  

```
bin/Release/net8.0/win-x64/publish/
```

3. Launch it with double-click or via console.  

---

## 📂 Project Structure  

- `Program.cs` — main game logic.
- `README.md` — documentation (this file).  

---

## 📊 Example Output  

```
###########
#@   #   X#
### # # ###
#   #     #
# ### ### #
#     #   #
###########
```

- `@` — player.  
- `#` — wall.  
- `X` — exit.  

---

## 📌 Future Plans  

- Add more item types (keys, weapons, food).  
- Add enemies.  
- Improve visuals with color rendering.  
