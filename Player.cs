using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;

namespace EndersDungeon
{
    [Serializable]
    public class Player
    {
        public string name;
        public int id;
        public int coins = 0;
        public int level = 1;
        public int xp = 0;
        public int health = 10;
        public int damage = 1;
        public int armorValue = 0;
        public int potion = 5;
        public int weaponValue = 1;

        public int mods = 0;

        public enum PlayerClass { Mage, Ranger, Warrior };
        public PlayerClass currentClass = PlayerClass.Warrior;

        public int GetHealth()
        {
            int upper = (2 * mods + 5);
            int lower = (mods + 2);
            return Program.rand.Next(lower, upper);
        }
        public int GetPower()
        {
            int upper = (2 * mods + 2);
            int lower = (mods + 1);
            return Program.rand.Next(lower, upper);
        }
        public int GetCoins()
        {
            int upper = (10 * mods + 50);
            int lower = 5 * (mods + 10);
            return Program.rand.Next(lower, upper);
        }

        public int GetXP()
        {
            int upper = (20 * mods + 50);
            int lower = (15 * mods + 10);
            return Program.rand.Next(lower, upper);
        }

        public int GetLevelUpValue()
        {
            return 100 * level + 400;
        }

        public bool CanLevelUp()
        {
            if (xp >= GetLevelUpValue())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void LevelUp()
        {
            while (CanLevelUp())
            {
                xp -= GetLevelUpValue();
                level++;
            }
            Console.ForegroundColor = ConsoleColor.Cyan;
            Program.Print("Conrgratulations! You are now " + level + "!");
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
}
