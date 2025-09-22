using System;
using System.Collections.Generic;

class LabyrinthGame
{
    static int width;
    static int height;
    static char[,] map;
    static int playerX = 1, playerY = 1;
    static bool hasLantern = false;
    static bool hasKey = false;
    static int exitX, exitY;
    static bool exitLocked = true;

    // Игрок
    static string playerName = "Игрок";
    static int playerHealth = 100;
    static List<string> inventory = new List<string>();
    static bool hasSword = false;
    static int medkits = 0;

    // Монстр
    static int monsterX, monsterY;
    static bool monsterAlive = true;

    // Кампания
    static int totalLevels = 1;
    static int currentLevel = 1;

    // Описание локации (храним описание для текущей занятой клетки)
    static string currentDescription = "";
    static int lastX = -1, lastY = -1;

    static void Main()
    {
        Console.CursorVisible = false;

        Console.Write("Введите имя персонажа: ");
        playerName = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(playerName)) playerName = "Безымянный герой";

        Console.Write("Введите количество уровней лабиринта: ");
        totalLevels = int.Parse(Console.ReadLine() ?? "1");

        Console.Write("Введите ширину лабиринта (нечётное число > 20): ");
        width = int.Parse(Console.ReadLine() ?? "41");

        Console.Write("Введите высоту лабиринта (нечётное число > 10): ");
        height = int.Parse(Console.ReadLine() ?? "21");

        // Настройка окна
        Console.SetWindowSize(Math.Min(width + 30, 150), Math.Min(height + 10, 50));
        Console.SetBufferSize(Math.Min(width + 30, 150), Math.Min(height + 10, 50));

        // Запускаем игру по уровням
        while (currentLevel <= totalLevels)
        {
            Console.Clear();
            Console.WriteLine($"=== Уровень {currentLevel} из {totalLevels} ===");
            Console.WriteLine("Нажмите любую клавишу для генерации...");
            Console.ReadKey();

            PlayLevel();

            if (playerHealth <= 0)
            {
                Console.Clear();
                Console.WriteLine("Вы погибли в лабиринте...");
                return;
            }

            if (currentLevel < totalLevels)
            {
                Console.WriteLine($"Вы прошли уровень {currentLevel}! Переход на следующий...");
                currentLevel++;
                ResetLevelState();
            }
            else
            {
                Console.WriteLine("🎉 Поздравляем! Вы прошли все лабиринты!");
                break;
            }
        }

        Console.WriteLine("Нажмите любую клавишу для выхода...");
        Console.ReadKey();
    }

    static void PlayLevel()
    {
        GenerateMaze();

        // Запоминаем описание стартовой клетки сразу после генерации
        currentDescription = GetTileDescription(map[playerY, playerX]);
        lastX = playerX;
        lastY = playerY;

        while (true)
        {
            Console.Clear();
            DrawMap();
            ShowLocationDescription();
            ShowPlayerStatus();

            ConsoleKeyInfo key = Console.ReadKey(true);
            int newX = playerX;
            int newY = playerY;

            switch (key.Key)
            {
                case ConsoleKey.W: newY--; break;
                case ConsoleKey.S: newY++; break;
                case ConsoleKey.A: newX--; break;
                case ConsoleKey.D: newX++; break;
                case ConsoleKey.H: // Для аптечки
                    UseMedkit();
                    break;
                case ConsoleKey.Escape:
                    Console.WriteLine("Выход из игры...");
                    Environment.Exit(0);
                    return;
            }

            // Проверка границ и проходимости
            if (newX >= 0 && newX < width && newY >= 0 && newY < height)
            {
                char target = map[newY, newX];
                if (target == '.' || target == 'X' || target == 'F' || target == '!' || target == 'M' || target == 'S' || target == 'P')
                {
                    if (target == 'X' && exitLocked)
                        continue;

                    // Запоминаем описание старой клетки пока ещё не изменили map (чтобы описание предмета не потерялось)
                    playerX = newX;
                    playerY = newY;
                    currentDescription = GetTileDescription(map[playerY, playerX]);
                    lastX = playerX;
                    lastY = playerY;
                }
            }

            // Подобор фонаря
            if (map[playerY, playerX] == 'F')
            {
                hasLantern = true;
                if (!inventory.Contains("Фонарь"))
                    inventory.Add("Фонарь");
                map[playerY, playerX] = '.';
            }

            // Подобор ключа
            if (map[playerY, playerX] == '!')
            {
                hasKey = true;
                if (!inventory.Contains("Ключ"))
                    inventory.Add("Ключ");
                map[playerY, playerX] = '.';
                UnlockExit();
            }

            // Подобор мечика
            if (map[playerY, playerX] == 'S')
            {
                hasSword = true;
                if (!inventory.Contains("Меч"))
                    inventory.Add("Меч");
                map[playerY, playerX] = '.';
            }

            // Подобор аптечки
            if (map[playerY, playerX] == 'P')
            {
                if (playerHealth < 100)
                {
                    playerHealth += 50;
                    if (playerHealth > 100) playerHealth = 100;
                    Console.WriteLine("Вы использовали аптечку сразу (+50 HP).");
                }
                else
                {
                    medkits++;
                    Console.WriteLine("Вы взяли аптечку и положили её в инвентарь.");
                }
                map[playerY, playerX] = '.';
                Console.ReadKey();
            }

            // Встретили монстра
            if (map[playerY, playerX] == 'M' && monsterAlive)
            {
                Battle();
                if (playerHealth <= 0)
                {
                    return;
                }
            }

            // Дошли до выхода
            if (map[playerY, playerX] == 'X')
            {
                Console.Clear();
                DrawMap();
                ShowPlayerStatus();
                Console.WriteLine($"\nПоздравляем! Вы нашли выход из уровня {currentLevel}!");
                break;
            }
        }
    }

    static string GetTileDescription(char tile)
    {
        switch (tile)
        {
            case '.': return "Вы находитесь в темном коридоре лабиринта.";
            case 'F': return "На полу лежит старый фонарь.";
            case '!': return "Вы видите блеск ключа на земле.";
            case '%': return "Перед вами закрытый выход с тяжелой дверью.";
            case 'X': return "Открытый проход зовет вас наружу.";
            case 'M': return "Перед вами жуткий монстр!";
            case 'S': return "На земле лежит старый меч.";
            case 'P': return "Вы нашли аптечку (+50 HP).";
            default: return "Каменные стены окружили вас.";
        }
    }

    static void GenerateMaze()
    {
        map = new char[height, width];
        for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
                map[y, x] = '#';

        Random rand = new Random();
        Stack<(int x, int y)> stack = new Stack<(int, int)>();

        int startX = 1, startY = 1;
        map[startY, startX] = '.';
        stack.Push((startX, startY));

        int[] dx = { 0, 0, -2, 2 };
        int[] dy = { -2, 2, 0, 0 };

        while (stack.Count > 0)
        {
            var (cx, cy) = stack.Peek();

            List<int> directions = new List<int> { 0, 1, 2, 3 };
            bool moved = false;

            while (directions.Count > 0)
            {
                int dir = directions[rand.Next(directions.Count)];
                directions.Remove(dir);

                int nx = cx + dx[dir];
                int ny = cy + dy[dir];

                if (nx > 0 && nx < width - 1 && ny > 0 && ny < height - 1 && map[ny, nx] == '#')
                {
                    map[cy + dy[dir] / 2, cx + dx[dir] / 2] = '.';
                    map[ny, nx] = '.';
                    stack.Push((nx, ny));
                    moved = true;
                    break;
                }
            }

            if (!moved)
                stack.Pop();
        }

        playerX = startX;
        playerY = startY;

        List<(int x, int y)> candidates = new List<(int, int)>();
        for (int x = 1; x < width - 1; x++)
        {
            if (map[1, x] == '.') candidates.Add((x, 0));
            if (map[height - 2, x] == '.') candidates.Add((x, height - 1));
        }
        for (int y = 1; y < height - 1; y++)
        {
            if (map[y, 1] == '.') candidates.Add((0, y));
            if (map[y, width - 2] == '.') candidates.Add((width - 1, y));
        }

        var exit = candidates[new Random().Next(candidates.Count)];
        exitX = exit.x;
        exitY = exit.y;
        map[exitY, exitX] = '%';

        // Фонарь
        PlaceRandom('F');

        // Ключ
        PlaceRandom('!');

        // Монстр
        PlaceRandom('M');
        monsterAlive = true;

        // Мечик
        PlaceRandom('S');

        // Аптечка
        PlaceRandom('P');
    }

    static void PlaceRandom(char symbol)
    {
        Random rand = new Random();
        while (true)
        {
            int x = rand.Next(1, width - 1);
            int y = rand.Next(1, height - 1);
            if (map[y, x] == '.')
            {
                map[y, x] = symbol;
                if (symbol == 'M') { monsterX = x; monsterY = y; }
                return;
            }
        }
    }

    static void UnlockExit()
    {
        exitLocked = false;
        map[exitY, exitX] = 'X';
    }

    static void ResetLevelState()
    {
        if (hasLantern && inventory.Contains("Фонарь"))
        {
            inventory.Remove("Фонарь");
        }
        hasLantern = false;
        if (hasKey && inventory.Contains("Ключ"))
        {
            inventory.Remove("Ключ");
        }
        hasKey = false;
        if (hasSword && inventory.Contains("Меч"))
        {
            inventory.Remove("Меч");
        }
        hasSword = false;
        exitLocked = true;
        monsterAlive = true;
    }

    static void DrawMap()
    {
        int visionRadius = hasLantern ? Math.Max(width, height) : 4;

        for (int y = 0; y < map.GetLength(0); y++)
        {
            for (int x = 0; x < map.GetLength(1); x++)
            {
                if (x == playerX && y == playerY)
                {
                    Console.ForegroundColor = (playerHealth <= 50) ? ConsoleColor.Red : ConsoleColor.Yellow;
                    Console.Write('@');
                }
                else
                {
                    int dist = Math.Abs(x - playerX) + Math.Abs(y - playerY);
                    if (dist <= visionRadius)
                    {
                        bool isBorder = (x == 0 || x == width - 1 || y == 0 || y == height - 1);
                        switch (map[y, x])
                        {
                            case '#':
                                if (isBorder)
                                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                                else
                                    Console.ForegroundColor = ConsoleColor.DarkGray;
                                break;
                            case '.': Console.ForegroundColor = ConsoleColor.White; break;
                            case '%': Console.ForegroundColor = ConsoleColor.Red; break;
                            case 'X': Console.ForegroundColor = ConsoleColor.Green; break;
                            case 'F': Console.ForegroundColor = ConsoleColor.Blue; break;
                            case '!': Console.ForegroundColor = ConsoleColor.Magenta; break;
                            case 'M': Console.ForegroundColor = ConsoleColor.DarkRed; break;
                            case 'S': Console.ForegroundColor = ConsoleColor.Cyan; break;
                            case 'P': Console.ForegroundColor = ConsoleColor.Green; break;
                            default: Console.ResetColor(); break;
                        }
                        Console.Write(map[y, x]);
                    }
                    else
                    {
                        Console.Write(' ');
                    }
                }
            }
            Console.WriteLine();
        }
        Console.ResetColor();
    }

    static void ShowLocationDescription()
    {
        // Если currentDescription пуст — делаем новое описание, что бы не было пустоты
        if (string.IsNullOrEmpty(currentDescription))
        {
            currentDescription = GetTileDescription(map[playerY, playerX]);
            lastX = playerX;
            lastY = playerY;
        }

        Console.WriteLine("\nОписание локации:");
        Console.WriteLine(currentDescription);
    }

    static void ShowPlayerStatus()
    {
        Console.WriteLine("\nСостояние игрока:");
        Console.WriteLine($"Имя: {playerName}");
        Console.WriteLine($"Здоровье: {playerHealth}");
        Console.WriteLine("Инвентарь: "
            + (inventory.Count > 0 ? string.Join(", ", inventory) : "пусто"));
        Console.WriteLine($"Аптечки: {medkits}");
        Console.WriteLine("Нажмите (H), чтобы использовать аптечку.");
    }

    static void UseMedkit()
    {
        if (medkits > 0 && playerHealth < 100)
        {
            playerHealth += 50;
            if (playerHealth > 100) playerHealth = 100;
            medkits--;
            Console.WriteLine("💊 Вы использовали аптечку (+50 HP).");
        }
        else if (medkits == 0)
        {
            Console.WriteLine("У вас нет аптечек!");
        }
        else
        {
            Console.WriteLine("Ваше здоровье уже на максимуме!");
        }
        Console.ReadKey();
    }

    static void Battle()
    {
        Console.Clear();
        Console.WriteLine("⚔️ Монстр нападает!");

        if (hasSword)
        {
            Console.WriteLine("Вы использовали меч! Монстр повержен, но вы теряете 50 HP!");
            monsterAlive = false;
            map[monsterY, monsterX] = '.';
            playerHealth -= 50;
            if (playerHealth < 0) playerHealth = 0;
        }
        else
        {
            Random rand = new Random();
            int chance = rand.Next(100);
            if (chance < 66)
            {
                Console.WriteLine("Монстр атакует! Вы получаете 20 урона!");
                playerHealth -= 20;
            }
            else
            {
                Console.WriteLine("Вам удалось сбежать от монстра!");
            }
        }

        Console.WriteLine("\nНажмите любую клавишу, чтобы продолжить...");
        Console.ReadKey();
    }
}
