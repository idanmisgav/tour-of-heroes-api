using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using WebApplication2.Models;

namespace WebApplication2.dataAccess
{
    public class MongoDB_Manager
    {
        //private static IMongoDatabase heroesDB = null;
        internal static bool DB_Changed = false;
        internal static MongoDB_Initializer DB;
        internal static Hero[] heroes;

        /** Get an updated type of heroes array */
        public static async void GetCurrentHeroes()
        {
            try
            {
                /* Get all docs from collection */
                IMongoCollection<BsonDocument> mainCollection = DB.mainCollection;
                using (IAsyncCursor<BsonDocument> task = await mainCollection.FindAsync(Builders<BsonDocument>.Filter.Empty))
                {
                    List<BsonDocument> docs = await task.ToListAsync();                
                    /* Retrieve heroes document and deserialize it into array */
                    BsonDocument heroesDocument = docs.FirstOrDefault();
                    BsonValue heroesValue = heroesDocument["heroes"];
                    List<Hero> heroesObj = BsonSerializer.Deserialize<List<Hero>>(heroesValue.ToJson());
                    Hero[] currHeroes = heroesObj.ToArray();
                    heroes = currHeroes;
                    DB_Changed = false;
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }            
        }

        public static async void DeleteHeroFromDB(Hero hero)
        {
            IMongoCollection<BsonDocument> mainCollection = DB.mainCollection;
            var result = await mainCollection.FindOneAndDeleteAsync(
                         Builders<BsonDocument>.Filter.Eq(hero.SerialNumber.ToString(), hero.Name));
            DB_Changed = true;
        }

        public static void AddHeroToDB(Hero hero)
        {
            List<Hero> heroesAsList = heroes.OfType<Hero>().ToList();

            //Generate a unique serialNumber for the new hero
            hero.SerialNumber = GenerateUniqueSerialNumber(heroesAsList);

            //Build the heroes BsonDocument
            heroesAsList.Add(hero);
            BsonArray bsonHeroesArray = BuildBsonArray(heroesAsList);
            BsonDocument heroesDoc = BuildHeroesBsonDocument(bsonHeroesArray);

            //Delete previous heroes BsonDocument and add the new one
            IMongoCollection<BsonDocument> mainCollection = DB.mainCollection;
            mainCollection.DeleteOne(Builders<BsonDocument>.Filter.Empty);
            mainCollection.InsertOne(heroesDoc);
            DB_Changed = true;
        }

        /* Returns an array of heroes as bson document */
        private static BsonArray BuildBsonArray(List<Hero> lst)
        {
            BsonArray result = new BsonArray();

            foreach(Hero hero in lst)
            {
                BsonDocument bsonHero = new BsonDocument();
                bsonHero.Add("SerialNumber", hero.SerialNumber);
                bsonHero.Add("Name", hero.Name);

                result.Add(bsonHero);
            }
            return result;
        }

        /* Returns a BsonDocument containing the heroes array */
        private static BsonDocument BuildHeroesBsonDocument(BsonArray bsonHeroesArray)
        {
            BsonDocument heroesDoc = new BsonDocument();
            heroesDoc.Add("heroes", bsonHeroesArray);
            return heroesDoc;
        }

        /* Returns a unique serialNumber for the new added hero */
        private static int GenerateUniqueSerialNumber(List<Hero> lstHeroes)
        {
            int result = 0;
            foreach(Hero hero in lstHeroes)
            {
                if (hero.SerialNumber > result)
                    result = hero.SerialNumber;
            }
            return result + 1;
        }
    }
}