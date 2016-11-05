using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.Cors;
using WebApplication2.Models;

namespace WebApplication2
{
    public static class WebApiConfig
    {
        private static IMongoDatabase heroesDB = null;

        public static async void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Attribute routing.
            config.MapHttpAttributeRoutes();

            // Web API routes (convention routing).
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            //enable cors globally
            config.EnableCors(new EnableCorsAttribute("*", "*", "*"));

            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            MongoClient mongo = new MongoClient(ConfigurationManager.AppSettings["connectionString"]);
            heroesDB = mongo.GetDatabase("heroes");

            //            var document = new BsonDocument
            //{
            //    { "address" , new BsonDocument
            //        {
            //            { "street", "2 Avenue" },
            //            { "zipcode", "10075" },
            //            { "building", "1480" },
            //            { "coord", new BsonArray { 73.9557413, 40.7720266 } }
            //        }
            //    },
            //    { "borough", "Manhattan" },
            //    { "cuisine", "Italian" },
            //    { "grades", new BsonArray
            //        {
            //            new BsonDocument
            //            {
            //                { "date", new DateTime(2014, 10, 1, 0, 0, 0, DateTimeKind.Utc) },
            //                { "grade", "A" },
            //                { "score", 11 }
            //            },
            //            new BsonDocument
            //            {
            //                { "date", new DateTime(2014, 1, 6, 0, 0, 0, DateTimeKind.Utc) },
            //                { "grade", "B" },
            //                { "score", 17 }
            //            }
            //        }
            //    },
            //    { "name", "Vella" },
            //    { "restaurant_id", "41704620" }
            //};

            //            var collection = heroesDB.GetCollection<BsonDocument>("restaurants");
            //            await collection.InsertOneAsync(document);

            IMongoCollection<BsonDocument> main = heroesDB.GetCollection<BsonDocument>("main");
            IAsyncCursor<BsonDocument> task = await main.FindAsync(Builders<BsonDocument>.Filter.Empty);
            List<BsonDocument> list = await task.ToListAsync();
            BsonDocument result = list.FirstOrDefault();
            BsonValue heroesObj = result["heroes"];
            List<HeroNew> d = BsonSerializer.Deserialize<List<HeroNew>>(heroesObj.ToJson());
            HeroNew[] heroes = d.ToArray();


        }

        internal static IMongoDatabase GetDataBase()
        {
            return heroesDB;
        }
    }
}
