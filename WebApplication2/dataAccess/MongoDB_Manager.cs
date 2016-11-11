using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Models;

namespace WebApplication2.dataAccess
{
    public class MongoDB_Manager
    {
        private static IMongoDatabase heroesDB = null;
        private static bool DB_Changed = true;
        private static Hero[] heroes = null;

        /** Get an updated type of heroes array */
        public static async void GetCurrentHeroes()
        {
            /* Get all docs from collection */
            IMongoCollection<BsonDocument> mainCollection = GetCollection("main");
            IAsyncCursor<BsonDocument> task = await mainCollection.FindAsync(Builders<BsonDocument>.Filter.Empty);
            List<BsonDocument> docs = await task.ToListAsync();

            /* Retrieve heroes document and deserialize it into array */
            BsonDocument heroesDocument = docs.FirstOrDefault();
            BsonValue heroesValue = heroesDocument["heroes"];
            List<Hero> heroesObj = BsonSerializer.Deserialize<List<Hero>>(heroesValue.ToJson());
            Hero[] currHeroes = heroesObj.ToArray();

            DB_Changed = false;
            heroes = currHeroes;
        }

        internal static void ConnectToMongoDB()
        {
            MongoClient mongo = new MongoClient(ConfigurationManager.AppSettings["connectionString"]);
            heroesDB = mongo.GetDatabase("heroes");
        }

        /** Updates heroes variable to hold the up-to-date array of heroes as in DB */
        //public static void GetCurrentHeroes()
        //{
        //    List<BsonDocument> docs = GetAllDocsFromCollection("main").Result;
        //    BsonDocument heroesDocument = docs.FirstOrDefault();
        //    BsonValue heroesValue = heroesDocument["heroes"];
        //    List<Hero> heroesObj = BsonSerializer.Deserialize<List<Hero>>(heroesValue.ToJson());
        //    Hero[] currHeroes = heroesObj.ToArray();
        //    //DB_Changed = false;
        //    heroes = currHeroes;
        //}

        /* returns the collection from app's DB by name */
        private static IMongoCollection<BsonDocument> GetCollection(string collectionName)
        {
            return heroesDB.GetCollection<BsonDocument>(collectionName);
        }

        /* returns all documents in a collection (as a list of Bson docs) */
        private static async Task<List<BsonDocument>> GetAllDocsFromCollection(string collectionName)
        {
            IMongoCollection<BsonDocument> mainCollection = GetCollection(collectionName);
            IAsyncCursor<BsonDocument> task = await mainCollection.FindAsync(Builders<BsonDocument>.Filter.Empty);
            List<BsonDocument> docs = await task.ToListAsync();
            return docs;
        }

        /* Getters for static variables */
        public static bool GetDB_Changed()
        {
            return DB_Changed;
        }

        public static Hero[] GetHeroes()
        {
            return heroes;
        }

        public static IMongoDatabase GetDB()
        {
            return heroesDB;
        }

    }
}