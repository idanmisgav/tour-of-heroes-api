using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using WebApplication2.Models;
using WebApplication2.Utils;

namespace WebApplication2.data
{    
    public class HeroesDB_Manager
    {
        public static bool DB_Changed = true;
        public static Hero[] heroes = null;
        private static int maxId = 0;

        public static Hero[] GetCurrentHeroes()
        {
            List<Hero> heroesList = new List<Hero>();
            if(!DB_Changed)
            {
                return heroes;
            }
            int currMax = 0;
            string line;
            // Read the file and display it line by line.
            System.IO.StreamReader file = new System.IO.StreamReader(Constants.DB_PATH);
            while ((line = file.ReadLine()) != null)
            {
                if (String.IsNullOrEmpty(line)) { continue; }
                char[] delimiterChars = { ':' };
                string[] heroDetails = line.Split(delimiterChars);
                int id = Int32.Parse(heroDetails[0]);
                Hero currHero = new Hero { SerialNumber = id, Name = heroDetails[1] };
                if(id > currMax)
                {
                    currMax = id;
                }
                heroesList.Add(currHero);
            }
            file.Close();
            maxId = currMax;
            DB_Changed = false;
            heroes = heroesList.ToArray();
            return heroes;
        }

        public static void DeleteHeroFromDB(int id)
        {
            string line;
            System.IO.StreamReader file = new System.IO.StreamReader(Constants.DB_PATH);
            while ((line = file.ReadLine()) != null)
            {
                char[] delimiterChars = { ':' };
                string[] heroDetails = line.Split(delimiterChars);
                int lineId = Int32.Parse(heroDetails[0]);
                if(lineId == id)
                {
                    file.Close();
                    string fileContetnt = File.ReadAllText(Constants.DB_PATH);
                    string fileContentAfterDeletion = fileContetnt.Replace(line, "");
                    File.WriteAllText(Constants.DB_PATH, fileContentAfterDeletion);
                    FileUtils.CleanEmptyLines(Constants.DB_PATH);
                    break;
                }
            }
            DB_Changed = true;
            file.Close();
        }

        public static void AddHeroToDB(string name)
        {
            int nextAvailableId = findNextId();
            File.AppendAllText(Constants.DB_PATH, "\r\n" + nextAvailableId + ":" + name);
            DB_Changed = true;
        }

        private static int findNextId()
        {
            maxId = maxId + 1;
            return maxId;
        }

        public static void UpdateHeroInDB(Hero hero)
        {
            string composeLine = hero.SerialNumber + ":";
            string composeNewHeroLine = composeLine + hero.Name;           
            string fileContetnt = File.ReadAllText(Constants.DB_PATH);
            int indexOfLine = fileContetnt.IndexOf(composeLine);
            int lengthToEndLine = StringUtils.findLengthUntilChar(fileContetnt, indexOfLine, '\r');
            string oldHero = fileContetnt.Substring(indexOfLine, lengthToEndLine);
            string result = fileContetnt.Replace(oldHero, composeNewHeroLine);
            File.WriteAllText(Constants.DB_PATH, result);
            DB_Changed = true;
        }
    }

    static class Constants
    {
        public const string DB_PATH = "C:/Users/imisgav/Desktop/projects/WebApplication1/heroesDB.txt";
    }
}