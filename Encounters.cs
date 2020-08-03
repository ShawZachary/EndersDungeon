using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Runtime.ExceptionServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace EndersDungeon
{
    public class Encounters
    {
        static Random rand = new Random();
        // Encounter Generic

        // Encounters
        public static void FirstEncounter()
        {
            Print("You sneak up on the drunk, grabbing the empty wine bottle off the table..");
            Print("You raise the bottle above your head, but then the gaurd awakes and turns to face you..\n");
            Console.WriteLine("Press enter to continue");
            Console.ReadKey();
            Combat(false, "Drunken gaurd", 1, 4);
        }
        public static void BasicFightEncounter()
        {
            Console.Clear();
            Print("As you contine on your path, you see a shadowey figure approach..");
            Console.ReadKey();
            Combat(true, "", 0, 0);
        }
        public static void DarkWizardEncounter()
        {
            Console.Clear();
            Print("As you proceed on your journey, You see a black shadow contrasting in the moonlight. A figure in long robes and a");
            Print(" crooked hat. As you step closer, the figure looks up, exposing their");
            Console.ForegroundColor = ConsoleColor.Red;
            Print("glowing red eyes..", 20);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.ReadKey();
            Combat(false, "Dark Wizard", 4, 6);
        }

        // Encounter Tools
        public static void RandomEncounter()
        {
            switch(rand.Next(0, 2))
            {
                case 0:
                    BasicFightEncounter();
                    break;
                case 1:
                    DarkWizardEncounter();
                    break;
            }
        }
        public static void Combat(bool random, string name, int power, int health)
        {
            string n = "";
            int p = 0;
            int h = 0;

            if (random)
            {
                n = GetName();
                p = Program.currentPlayer.GetPower();
                h = Program.currentPlayer.GetHealth();
            }
            else
            {
                n = name;
                p = power;
                h = health;
            }

            while(h > 0)
            {
                Console.Clear();
                Console.WriteLine(n);
                Console.WriteLine("Power " + p + "/" + "Health " + h);
                Console.WriteLine("=====================");
                Console.WriteLine("| (A)ttack (D)efend |");
                Console.WriteLine("|                   |");
                Console.WriteLine("| (R)un    (H)eal   |");
                Console.WriteLine("=====================");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Potions: " + Program.currentPlayer.potion);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Health: " + Program.currentPlayer.health);
                Console.ForegroundColor = ConsoleColor.Gray;
                string input = Console.ReadLine();
                if (input.ToLower() == "a" || input.ToLower() == "attack")
                {
                    // Attack
                    Console.Beep();
                    Print("With haste you swing your weapon at " + n +"! As you draw back the " + n + " counters with an attack of their own!\n");

                    int damage = p - Program.currentPlayer.armorValue;
                    if (damage < 0)
                    {
                        damage = 0;
                    }
                    int attack = rand.Next(0, Program.currentPlayer.weaponValue) + rand.Next(1, 4) + ((Program.currentPlayer.currentClass == Player.PlayerClass.Warrior)? + 3:0); // damage will be between 1 and 3.

                    Print("You have lost " + damage + " Health and deal " + attack + " damge to " + n + "");
                    Console.WriteLine("Press any key to Continue");
                    Program.currentPlayer.health -= damage;
                    h -= attack;
                }
                else if (input.ToLower() == "d" || input.ToLower() == "defend")
                {
                    // Defend
                    Console.Beep();
                    Print("As the " + n + " prepares to attack, You ready yourself into a defensive stance!\n");

                    int damage = (p / 4) - Program.currentPlayer.armorValue;
                    if (damage <0)
                    {
                        damage = 0;
                    }
                    int attack = rand.Next(0, Program.currentPlayer.weaponValue)/2; // so that you are doing less damage, considering you are in a defense stance.

                    Print("You have lost " + damage + " Health and deal " + attack + " damge to " + n + "!\n");
                    Console.WriteLine("Press any key to Continue");
                    Program.currentPlayer.health -= damage;
                    h -= attack;
                }
                else if (input.ToLower() == "r" || input.ToLower() == "run")
                {
                    // Run
                    Console.Beep();
                    if (Program.currentPlayer.currentClass != Player.PlayerClass.Ranger && rand.Next(0, 2) == 0)
                    {
                        Print("As you attempt to run from " + n + ", they strike you in the back! Sending you spawrling onto the ground.\n");

                        int damage = p - Program.currentPlayer.armorValue;
                        if (damage < 0)
                        {
                            damage = 0;
                        }
                        Print("You lose " + damage + " health and cannot escape.\n");
                        Console.WriteLine("Press any key to Continue");
                        Console.ReadKey();
                    }
                    else
                    {
                        // Go to Town
                        Print("You manage to escape from the "+ n +"! You successfully escape and run to the nearest shop!\n");
                        Console.ReadKey();
                        Shop.LoadShop(Program.currentPlayer);
                    }
                    Console.WriteLine("");
                }
                else if (input.ToLower() == "h" || input.ToLower() == "heal")
                {
                    // Heal
                    Console.Beep();
                    if (Program.currentPlayer.potion == 0)
                    {
                        Print("You quickly reach for a potion but realize you have none left!\n");
                        Console.WriteLine("Press any key to Continue");
                        int damage = p - Program.currentPlayer.armorValue;
                        if (damage < 0)
                        {
                            damage = 0;
                        }
                        Print("As you fumbled around for a potion, the " + n + " attacks you! You lose " + damage + "health.\n");
                        Console.WriteLine("Press any key to Continue");
                    }
                    else
                    {
                        Print("You reach into the bag on your waist, and pull out a restore potion, You throw it back and get ready for the fight.\n");
                        Console.WriteLine("Press any key to Continue");
                        int potionValue = 5 + ((Program.currentPlayer.currentClass == Player.PlayerClass.Mage)? + 4:0); // Class check can be used anywhere, such as in puzzles.
                        Print("You have restored " + potionValue + " Health.\n");
                        Program.currentPlayer.health += potionValue;
                        Print("As you drank your potion, the " + n + " advanced and went for an attack!\n");
                        int damage = (p / 2) - Program.currentPlayer.armorValue;
                        if (damage < 0)
                        {
                            damage = 0;
                        }
                        Print("You lose " + damage + " health.\n");
                        Console.WriteLine("Press any key to Continue");
                    }
                    Console.ReadKey();
                }
                if (Program.currentPlayer.health <= 0)
                {
                    // Death Code
                    Print("As the " + n + " grins their evil grin, You realize you have taken your last breath.. \n You have died.", 70);
                    Console.ReadKey();
                    System.Environment.Exit(0);
                }
                Console.ReadKey();
            }
            int c = Program.currentPlayer.GetCoins();
            int x = Program.currentPlayer.GetXP();
            Print("As you stand victorious over " + n + ", it's body dissolves into the ground.. \n Leaveing a sack of " + c + " gold coins.\n You have gained " + x + "XP!");
            Program.currentPlayer.coins += c;
            Program.currentPlayer.xp += x;

            if (Program.currentPlayer.CanLevelUp())
            {
                Program.currentPlayer.LevelUp();
            }
            Console.ReadKey();
        }

        public static string GetName()
        {
            switch (rand.Next(0, 5))
            {
                case 0:
                    return "Skeleton";
                case 1:
                    return "Thug";
                case 2:
                    return "Lesser Demon";
                case 3:
                    return "Dark Wizard";
                case 4:
                    return "Witch";
            }
            return "Evil Captor";
        }
    public static void Print(string text, int speed = 40)
    {
        foreach (char c in text)
        {
            Console.Write(c);
            System.Threading.Thread.Sleep(speed);
        }
        Console.WriteLine();
    }
    }
}
