using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebApplication2.Models;
using static MongoDB.Driver.WriteConcern;

namespace WebApplication2.dataAccess
{
    public class MongoDB_Manager
    {
        private static IMongoDatabase heroesDB = WebApiConfig.GetDataBase();

        /** returns the collection from app's DB by name */
        private static IMongoCollection<BsonDocument> GetCollection(string collectionName)
        {
            return heroesDB.GetCollection<BsonDocument>(collectionName);
        }

        /** returns all documents in a collection (as a list of Bson docs) */
        private static async Task<List<BsonDocument>> GetAllDocsInCollection(IMongoCollection<BsonDocument> collection)
        {
            IAsyncCursor<BsonDocument> task = await collection.FindAsync(Builders<BsonDocument>.Filter.Empty);
            List<BsonDocument> list = await task.ToListAsync();
            return list;
        }

        public static async Task<Hero[]> GetCurrentHeroes()
        {
            var main = heroesDB.GetCollection<BsonDocument>("main");
            //var filter = Builders<BsonDocument>.Filter.Eq(< field >, < value >);
            IAsyncCursor<BsonDocument> task = await main.FindAsync(Builders<BsonDocument>.Filter.Empty);
            return null;
        }
    }
}