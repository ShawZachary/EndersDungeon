using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Media;

namespace EndersDungeon
{
    public class Program
    {
        public static Player currentPlayer = new Player();
        public static bool mainLoop = true;
        public static Random rand = new Random();

        static void Main(string[] args)
        {
            if (!Directory.Exists("saves"))
            {
                Directory.CreateDirectory("saves");
            }

            currentPlayer = Load(out bool newP);
            if (newP)
                Encounters.FirstEncounter();
            
            while (mainLoop)
            {
                Encounters.RandomEncounter();
            }
        }

        static Player NewStart(int i)
        {
            Console.Clear();
            Player currentPlayer = new Player();
            Console.WriteLine("Welcome to Ender's Dungeon.");
            Print("\nWhat is your Name Hero?");
            currentPlayer.name = Console.ReadLine();
            Print("Class Options: Mage | Ranger | Warrior");
            bool flag = false;
            while (flag == false)
            {
                flag = true;
                string input = Console.ReadLine().ToLower();
                if(input == "mage")
                {
                    currentPlayer.currentClass = Player.PlayerClass.Mage;
                }
                else if (input == "ranger")
                {
                    currentPlayer.currentClass = Player.PlayerClass.Ranger;
                }
                else if (input == "warrior")
                {
                    currentPlayer.currentClass = Player.PlayerClass.Warrior;
                }
                else
                {
                    Console.WriteLine("Please choose an exsisting Class");
                    flag = false;
                }
            }

            currentPlayer.id = i;
            Console.Clear();
            Print("You have awoken in a cold and damp Prison cell.");
            Print("Your head Aches with a throbbing pain.");
            if (currentPlayer.name == "")
            {
                Print("You can't even remmeber your own name...", 80);
            }
            else
            {
                Print("All you can remember is your name, " + currentPlayer.name);
            }
            Console.ReadKey();
            Console.Clear();
            Print("You search your surroundings, looking for someway to escape.");
            Print("You reach through the bars to your cell and manage to grab the handle.");
            Print("You shake the handle, knowing that it would be locked.\n");
            Print("Press enter to continue");
            Console.ReadKey();
            Console.Clear();
            Print("But just as you were going to give up, you hear a click.. The lock broke! I suppose luck is on your side,");
            Print("Seeing as how your captors threw you in a cell with a rusty handle.");
            Print("You slowly open the cell door, as to not risk any sound to escape from the old hinges.\n");
            Print("Press enter to continue");
            Console.ReadKey();
            Console.Clear();
            Print("as you ease out of the cell, you see a drunken gaurd asleep at his post with his back to you.");
            Print("Press enter to continue");
            Console.ReadKey();
            Console.Clear();
            return currentPlayer;
        }

        public static void Quit()
        {
            Save();
            Environment.Exit(0);
        }


        public static void Save()
        {
            BinaryFormatter binForm = new BinaryFormatter();
            string path = "saves/" + currentPlayer.id.ToString() + ".level";
            FileStream file = File.Open(path, FileMode.OpenOrCreate);
            binForm.Serialize(file, currentPlayer);
            file.Close();
        }

        public static Player Load(out bool newP)
        {
            newP = false;
            Console.Clear();
            string[] paths = Directory.GetDirectories("saves");
            List<Player> players = new List<Player>();
            int idCount = 0;

            BinaryFormatter binForm = new BinaryFormatter();
            foreach (string  p in paths)
            {
                FileStream file = File.Open(p, FileMode.Open);
                Player player = (Player)binForm.Deserialize(file); // problem
                file.Close();
                players.Add(player);
            }

            idCount = players.Count;

            while (true)
            {
                Console.Clear();
                Print("Choose your Saved Player: ");

                foreach (Player p in players)
                {
                    Console.WriteLine(p.id + ": " + p.name);
                }

                Print("Please input player name or ID (ID:# or playername) Additionally. 'create' will start a new save!");
                string[] data = Console.ReadLine().Split(':');

                try
                {
                    if(data[0] == "ID")
                    {
                        if(int.TryParse(data[1], out int id))
                        {
                            foreach (Player player in players)
                            {
                                if(player.id == id)
                                {
                                    return player;
                                }
                            }
                            Console.WriteLine("There is no player with that ID!");
                            Console.ReadKey();
                        }
                        else
                        {
                            Console.WriteLine("Your ID needs to be a number! Press any key to try again.");
                            Console.ReadKey();
                        }
                    }
                    else if (data[0] == "create")
                    {
                        Player newPlayer = NewStart(idCount);
                        newP = true;
                        return newPlayer;
                    }
                    else
                    {
                        foreach (Player player in players)
                        {
                            if(player.name == data[0])
                            {
                                return player;
                            }
                        }
                        Console.WriteLine("There is no player with that name!");
                        Console.ReadKey();
                    }
                }
                catch(IndexOutOfRangeException)
                {
                    Console.WriteLine("Your ID needs to be a number! Press any key to try again.");
                    Console.ReadKey();
                }

            }
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

        public static void ProgressBar(string fillerChar, string backgroundChar, decimal value, int size)
        {
            int dif = (int)(value * size);
            for(int i = 0; i < 100; i++)
            {
                if(i < dif)
                {
                    Console.Write(fillerChar);
                }
                else
                {
                    Console.Write(backgroundChar);
                }
            }
        }
    }
}

