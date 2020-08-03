using System;
using System.Collections.Generic;
using System.Text;

namespace EndersDungeon
{
    public class Shop
    {
        public static void LoadShop(Player p)
        {
            RunShop(p);
        }

        public static void RunShop(Player p)
        {
            int potionP;
            int armorP;
            int weaponP;
            int difP;

            while (true)
            {
                potionP = 20 + 10 * p.mods;
                armorP = 100 * (p.armorValue + 1);
                weaponP = 100 * p.weaponValue;
                difP = 300 + 100 * p.mods;

                Console.Clear();
                Console.WriteLine("       The Runners' Shop       ");
                Console.WriteLine("===============================");
                Console.WriteLine(" (P)otion:         $" + potionP);
                Console.WriteLine(" (W)eapon:         $" + weaponP);
                Console.WriteLine(" (A)rmor:          $" + armorP);
                Console.WriteLine(" (D)ifficulty Mod: $" + difP);
                Console.WriteLine("===============================");
                Console.WriteLine(" (E)xit Shop");
                Console.WriteLine(" (Q)uit Game\n\n");

                Console.WriteLine(p.name + "'s Stats");
                Console.WriteLine("=========================");
                Console.WriteLine("Current Health: "   + p.health);
                Console.WriteLine("Coins: "            + p.coins);
                Console.WriteLine("Potions: "          + p.weaponValue);
                Console.WriteLine("Weapon Strength: "  + p.weaponValue);
                Console.WriteLine("Armor Toughness: "  + p.weaponValue);
                Console.WriteLine("Difficulty Level: " + p.weaponValue);

                // XP bar that can be used for other things in game, such as leveling a skill like lock picks.
                Console.Write("[");
                Program.ProgressBar("+", " ", ((decimal)p.xp / (decimal)p.GetLevelUpValue()),25);
                Console.WriteLine("]");

                Console.WriteLine("Level: " + p.level);
                Console.WriteLine("=========================");

                // Wait for input
                string input = Console.ReadLine().ToLower();
                if (input == "p" || input == "Potion")
                {
                    Console.Beep();
                    TryBuy("Potion", potionP, p);
                }
                else if (input == "w" || input == "Weapon")
                {
                    Console.Beep();
                    TryBuy("weapon", weaponP, p);
                }
                else if (input == "a" || input == "Armor")
                {
                    Console.Beep();
                    TryBuy("armor", armorP, p);
                }
                else if (input == "d" || input == "difficulty")
                {
                    Console.Beep();
                    TryBuy("difficulty", difP, p);
                }
                else if (input == "e" || input == "exit")
                {
                    Console.Beep();
                    break;
                }
                else if (input == "q" || input == "quit")
                {
                    Console.Beep();
                    Program.Quit();
                }
            }
        }
        static void TryBuy(string item, int cost, Player p)
        {
            if(p.coins >= cost)
            {
                if(item == "Potion")
                {
                    p.potion++;
                }
                else if(item == "weapon")
                {
                    p.weaponValue++;
                }
                else if (item == "armor")
                {
                    p.armorValue++;
                }
                else if (item == "dif")
                {
                    p.mods++;
                }
                p.coins -= cost;
            }
            else
            {
                Console.WriteLine("The shop owner glares a with a mean stink in his eye..");
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("You dont have enough coin for that, make yourself useful and go earn more coin!");
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }
    }
}
