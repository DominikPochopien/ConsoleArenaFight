using System;
using System.IO;

namespace KonsoloweRPG
{
    class Program
    {
        static Random rand = new Random();

        static void Main(string[] args)
        {
            bool exitGame = false;
            //Podstawowe opcje gry 
            while (!exitGame)
            {
                RunGame();

                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\nCo chcesz zrobić?");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("1. Zakończ grę");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("2. Zacznij nową grę");
                Console.WriteLine("");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("Wybierz opcję od 1 do 2: ");
                Console.ResetColor();

                string choice = Console.ReadLine();
                if (choice == "1")
                {
                    exitGame = true;
                    Console.WriteLine("Do zobaczenia!");
                }
                else if (choice == "2")
                {
                    Console.Clear();
                    continue;
                }
                else
                {
                    Console.WriteLine("Nieprawidłowa opcja. Kończę grę.");
                    exitGame = true;
                }
            }
        }
        // główne menu gry
        static void RunGame()
        {
            int playerHP = 200;
            int playerMaxHP = 200;
            int gold = 0;
            int potions1 = 3;
            int potions2 = 2;
            int enemiesDefeated = 0;
            int keys = 0;
            bool hasSword = false;
            bool hasArmor = false;
            int sharpnessLevel = 0;
            int luckLevel = 0;

            string[] enemyNames = { "Ork", "Goblin", "Troll", "Wilk", "Bandyta", "Zombi", "Szkielet", "Czarownik", "Nekromanta", "Smok",
                                    "Wampir", "Demon", "Ogre", "Pająk", "Harpi", "Minotaur", "Golem", "Czarnoksiężnik", "Wiedźma", "Kraken", "Dragon" };

            bool running = true;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("=== Walki na Arenie 1.0 ===");
            Console.ResetColor();

            while (running)
            {
                if (playerHP <= 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nTwoje HP spadło do 0. Zostałeś pokonany!");
                    Console.ResetColor();
                    break;
                }

                Console.WriteLine("\nWybierz opcję:");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("1. Atakuj przeciwnika");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("2. Otwórz skrzynkę ze złotem");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("3. Użyj mikstury leczniczej");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("4. Pokaż status gracza");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("5. Sklep");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("6. Zakończ grę");
                Console.WriteLine("");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("Wybierz opcję od 1 do 6: ");
                Console.ResetColor();

                string choice = Console.ReadLine();
                Console.Clear();

                switch (choice)
                {
                    case "1":
                        (playerHP, gold, potions1, potions2, enemiesDefeated, keys) = AttackEnemy(playerHP, playerMaxHP, gold, potions1, potions2, enemiesDefeated, keys, enemyNames, hasSword, sharpnessLevel, luckLevel);
                        break;
                    case "2":
                        (gold, keys, hasSword) = OpenChestExtended(gold, keys, hasSword);
                        break;
                    case "3":
                        (playerHP, potions1, potions2) = UsePotion(playerHP, playerMaxHP, potions1, potions2);
                        break;
                    case "4":
                        ShowStatus(playerHP, playerMaxHP, gold, potions1, potions2, enemiesDefeated, keys, hasSword, hasArmor, sharpnessLevel, luckLevel);
                        break;
                    case "5":
                        (gold, potions1, potions2, playerMaxHP, hasArmor, sharpnessLevel, luckLevel) = Shop(gold, potions1, potions2, playerMaxHP, hasArmor, hasSword, sharpnessLevel, luckLevel);
                        break;
                    case "6":
                        running = false;
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Nieprawidłowa opcja!");
                        Console.ResetColor();
                        break;
                }
            }
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\nGra zakończona!");
            Console.WriteLine("");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Pokonałeś {enemiesDefeated} przeciwników i zebrałeś {gold} złota.");
            Console.WriteLine("Wynik zapisano w pliku 'wynik.txt'.");
            Console.ResetColor();

            SaveResults(playerHP, playerMaxHP, gold, potions1, potions2, enemiesDefeated, keys, hasSword, hasArmor, sharpnessLevel, luckLevel);
        }
        // funkcja odpowiadająca za atak przeciwników i gracza
        static (int, int, int, int, int, int) AttackEnemy(int playerHP, int playerMaxHP, int gold, int potions1, int potions2, int enemiesDefeated, int keys, string[] enemyNames, bool hasSword, int sharpnessLevel, int luckLevel)
        {
            string enemy = enemyNames[rand.Next(enemyNames.Length)];
            int enemyHP = rand.Next(50, 90);
            int enemyDMG = rand.Next(10, 31);

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\nPojawił się {enemy} HP: {enemyHP}, DMG: {enemyDMG}!");
            Console.WriteLine(" ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Twoje HP: {Math.Max(playerHP, 0)}/{playerMaxHP}");
            Console.ResetColor();

            while (enemyHP > 0 && playerHP > 0)
            {
                int playerDMG = rand.Next(20, 35);

                if (hasSword && rand.Next(1, 5) == 1)
                    playerDMG += 20;

                playerDMG += sharpnessLevel * 5;
                enemyHP -= playerDMG;

                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"Zadałeś {playerDMG} obrażeń {enemy}.");
                Console.WriteLine(" ");
                Console.ResetColor();

                if (enemyHP <= 0)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine($"Pokonałeś {enemy}!");
                    Console.WriteLine(" ");
                    enemiesDefeated++;

                    int goldFound = rand.Next(30, 55);
                    goldFound += goldFound * luckLevel / 10;
                    gold += goldFound;

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"Zdobywasz {goldFound} złota!");
                    Console.WriteLine(" ");

                    if (rand.NextDouble() < 0.35) 
                    {
                        potions1++;
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Przeciwnik upuścił miksturę leczniczą poziom 1!");
                        Console.WriteLine(" ");

                    }
                    if (rand.NextDouble() < 0.25) 
                    {
                        potions2++;
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Przeciwnik upuścił miksturę leczniczą poziom 2!");
                        Console.WriteLine(" ");
                    }
                    if (rand.NextDouble() < 0.30)
                    {
                        keys++;
                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                        Console.WriteLine("Przeciwnik upuścił klucz do skrzynki!");
                        Console.WriteLine(" ");
                    }
                    Console.ResetColor();
                    break;
                }

                playerHP -= enemyDMG;
                
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write($"{enemy} HP: {Math.Max(enemyHP, 0)} ");
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("zadał Ci ");
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(enemyDMG);
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(" obrażeń!");
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Twoje HP: {Math.Max(playerHP, 0)}/{playerMaxHP}");
                Console.ResetColor();
            }

            if (playerHP <= 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Zostałeś pokonany...");
                Console.ResetColor();
            }

            return (playerHP, gold, potions1, potions2, enemiesDefeated, keys);
        }
        // funkcja odpowiadająca za otwieranie skrzynki w grze
        static (int, int, bool) OpenChestExtended(int gold, int keys, bool hasSword)
        {
            if (keys <= 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nNie masz klucza do skrzynki!");
                Console.ResetColor();
                return (gold, keys, hasSword);
            }

            keys--;
            int drop = rand.Next(1, 5);

            if (drop == 1)
            {
                gold += 50;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Znalazłeś 50 złota!");
            }
            else if (drop == 2)
            {
                keys++;
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Znalazłeś klucz!");
            }
            else
            {
                hasSword = true;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Znalazłeś miecz!");
            }

            Console.ResetColor();
            return (gold, keys, hasSword);
        }
        //funkcja odpowiadająca za używanie przedmiotów w grze
        static (int, int, int) UsePotion(int playerHP, int playerMaxHP, int potions1, int potions2)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\nWybierz miksturę do użycia:");
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"1. Mikstura poziom 1 (+20 HP), masz: {potions1}");
            Console.WriteLine($"2. Mikstura poziom 2 (+50 HP), masz: {potions2}");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"3. Wyjściue z magazynu!");
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Wybierz opcję od 1 do 3: ");
            Console.ResetColor();
            string choice = Console.ReadLine();

            if (choice == "1" && potions1 > 0)
            {
                playerHP += 20;
                if (playerHP > playerMaxHP) playerHP = playerMaxHP;
                potions1--;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Użyłeś mikstury poziom 1. HP: {playerHP}/{playerMaxHP}");
                Console.WriteLine("");
                Console.ResetColor();
            }
            else if (choice == "2" && potions2 > 0)
            {
                playerHP += 50;
                if (playerHP > playerMaxHP) playerHP = playerMaxHP;
                potions2--;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Użyłeś mikstury poziom 2. HP: {playerHP}/{playerMaxHP}");
                Console.WriteLine("");
                Console.ResetColor();
            }
            else if (choice == "3")
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Anulowano użycie mikstury.");
                Console.WriteLine("");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Nie masz tej mikstury lub wybrałeś błędną opcję!");
                Console.ResetColor();
            }

            return (playerHP, potions1, potions2);
        }
        //funkcja odpowiadająca za pokazywanie statusu gracza w menu
        static void ShowStatus(int playerHP, int playerMaxHP, int gold, int potions1, int potions2, int enemiesDefeated, int keys, bool hasSword, bool hasArmor,int sharpnessLevel, int luckLevel)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"\n=== Status gracza ===");
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"HP: {playerHP}/{playerMaxHP}");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Złoto: {gold}$");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"Klucze: {keys}");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Mikstury poziom 1: {potions1}");
            Console.WriteLine($"Mikstury poziom 2: {potions2}");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Pokonani przeciwnicy: {enemiesDefeated}");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"Miecz: {(hasSword ? "TAK" : "NIE")}");
            Console.WriteLine($"Zbroja: {(hasArmor ? "TAK" : "NIE")}");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"Poziom ostrości: {sharpnessLevel}");
            Console.WriteLine($"Poziom szczęścia: {luckLevel}");
            Console.WriteLine("");
            Console.ResetColor();
        }
        //funkcja odpowiadająca za sklep w grze

        static (int gold, int potions1, int potions2, int playerMaxHP, bool hasArmor, int sharpnessLevel, int luckLevel)
    Shop(int gold, int potions1, int potions2, int playerMaxHP, bool hasArmor, bool hasSword, int sharpnessLevel, int luckLevel)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"\n========= SKLEP | Złoto: {gold}$ =========\n");

            
            Console.ForegroundColor = gold >= 25 ? ConsoleColor.Green : ConsoleColor.DarkGray;
            Console.WriteLine("1. Mikstura poziom 1 (+20 HP) - 25$");

            Console.ForegroundColor = gold >= 50 ? ConsoleColor.Green : ConsoleColor.DarkGray;
            Console.WriteLine("2. Mikstura poziom 2 (+50 HP) - 50$");

            
            Console.ForegroundColor =
                hasArmor ? ConsoleColor.DarkGray :
                gold >= 350 ? ConsoleColor.Green : ConsoleColor.DarkGray;

            Console.WriteLine("3. Zbroja zwiększa maksymalne zdrowie (+100 HP) - 350$");

           
            if (!hasSword || gold < 100 || sharpnessLevel >= 5)
                Console.ForegroundColor = ConsoleColor.DarkGray;
            else
                Console.ForegroundColor = ConsoleColor.Magenta;

            Console.WriteLine($"4. Zaklęcie Ostrość miecza (Poziom {sharpnessLevel}/5) - 100$");

         
            bool luckUnlocked = luckLevel < 3 && luckLevel <= sharpnessLevel;

            if (!hasSword || gold < 150 || !luckUnlocked)
                Console.ForegroundColor = ConsoleColor.DarkGray;
            else
                Console.ForegroundColor = ConsoleColor.Yellow;

            Console.WriteLine($"5. Zaklęcie Szczęście miecza (Poziom {luckLevel}/3) - 150$");

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("6. Wyjdź ze sklepu");

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("\nWybierz opcję (1–6): ");
            Console.ResetColor();

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    if (gold >= 25)
                    {
                        gold -= 25;
                        potions1++;
                        Console.WriteLine("Kupiono miksturę poziom 1.");
                    }
                    else Console.WriteLine("Nie masz wystarczająco złota.");
                    break;

                case "2":
                    if (gold >= 50)
                    {
                        gold -= 50;
                        potions2++;
                        Console.WriteLine("Kupiono miksturę poziom 2.");
                    }
                    else Console.WriteLine("Nie masz wystarczająco złota.");
                    break;

                case "3":
                    if (hasArmor)
                        Console.WriteLine("Masz już zbroję.");
                    else if (gold >= 350)
                    {
                        gold -= 350;
                        playerMaxHP += 100;
                        hasArmor = true;
                        Console.WriteLine("Kupiono zbroję! Maksymalne HP +100.");
                    }
                    else Console.WriteLine("Nie masz wystarczająco złota.");
                    break;

                case "4":
                    if (!hasSword)
                        Console.WriteLine("Nie masz miecza!");
                    else if (sharpnessLevel >= 5)
                        Console.WriteLine("Ostrość jest już na maksymalnym poziomie.");
                    else if (gold >= 100)
                    {
                        gold -= 100;
                        sharpnessLevel++;
                        Console.WriteLine($"Ostrość zwiększona do poziomu {sharpnessLevel}!");
                    }
                    else Console.WriteLine("Nie masz wystarczająco złota.");
                    break;

                case "5":
                    if (!hasSword)
                        Console.WriteLine("Nie masz miecza!");
                    else if (luckLevel >= 3)
                        Console.WriteLine("Szczęście jest już na maksymalnym poziomie.");
                    else if (gold >= 150)
                    {
                        gold -= 150;
                        luckLevel++;
                        Console.WriteLine($"Szczęście zwiększone do poziomu {luckLevel}!");
                    }
                    else Console.WriteLine("Nie masz wystarczająco złota.");
                    break;

                default:
                    Console.WriteLine("Wychodzisz ze sklepu...");
                    break;
            }

            Console.ResetColor();
            return (gold, potions1, potions2, playerMaxHP, hasArmor, sharpnessLevel, luckLevel);
        }
        //funkcja odpowiadająca za zapis gty w pliku
        static void SaveResults(int playerHP, int playerMaxHP, int gold, int potions1, int potions2, int enemiesDefeated, int keys, bool hasSword, bool hasArmor, int sharpnessLevel, int lucklevel)
        {
            string result = "=== Wynik gry ===\n" +
            $"Pokonałeś {enemiesDefeated} przeciwników.\n" +
            $"Zebrałeś {gold} złota.\n" +
            $"Pozostało HP: {Math.Max(playerHP, 0)}/{playerMaxHP}.\n" +
            $"Klucze: {keys}\n" +
            $"Mikstury poziom 1: {potions1}\n" +
            $"Mikstury poziom 2: {potions2}\n" +
            $"Miecz: {(hasSword ? "TAK" : "NIE")}\n" +
            $"Zbroja: {(hasArmor ? "TAK" : "NIE")}\n" +
            $"Poziom ostrza miecza: {(sharpnessLevel)}\n" +
            $"Poziom strzęścia miecza: {(lucklevel)}\n" +

            "-----------------------------\n";

            File.AppendAllText("wynik1.txt", result);
        }

    }

}