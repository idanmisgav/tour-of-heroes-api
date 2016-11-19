using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebApplication2.Models;

namespace WebApplication2.dataAccess
{
    public class MongoDB_Initializer
    {
        internal MongoClient mongo { get; }
        internal IMongoDatabase heroesDB { get; }
        internal IMongoCollection<BsonDocument> mainCollection { get; }
        internal Hero[] heroes { get; set; }

        public MongoDB_Initializer()
        {
            this.mongo = new MongoClient(ConfigurationManager.AppSettings["connectionString"]);
            this.heroesDB = mongo.GetDatabase("heroes");
            this.mainCollection = heroesDB.GetCollection<BsonDocument>("main");
        }

        public async Task<Hero[]> GetHeroes()
        {
            IAsyncCursor<BsonDocument> task = await mainCollection.FindAsync(Builders<BsonDocument>.Filter.Empty);
            List<BsonDocument> docs = await task.ToListAsync();
            BsonDocument heroesDocument = docs.FirstOrDefault();
            BsonValue heroesValue = heroesDocument["heroes"];
            List<Hero> heroesObj = BsonSerializer.Deserialize<List<Hero>>(heroesValue.ToJson());
            Hero[] currHeroes = heroesObj.ToArray();
            this.heroes = currHeroes;
            return currHeroes;
        } 
    }
}