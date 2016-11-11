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
using WebApplication2.dataAccess;
using WebApplication2.Models;

namespace WebApplication2
{
    public static class WebApiConfig
    {

        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            /* Attribute routing. */
            config.MapHttpAttributeRoutes();

            /* Web API routes (convention routing). */
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            /* Enable cors globally */
            config.EnableCors(new EnableCorsAttribute("*", "*", "*"));

            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            /* Connect to Mongo DB */
            MongoDB_Manager.ConnectToMongoDB();

            //IMongoDatabase heroesDB = MongoDB_Manager.GetDB();
            //IMongoCollection<BsonDocument> main = heroesDB.GetCollection<BsonDocument>("main");
            //IAsyncCursor<BsonDocument> task = await main.FindAsync(Builders<BsonDocument>.Filter.Empty);
            //List<BsonDocument> list = await task.ToListAsync();
            //BsonDocument result = list.FirstOrDefault();
            //BsonValue heroesObj = result["heroes"];
            //List<Hero> d = BsonSerializer.Deserialize<List<Hero>>(heroesObj.ToJson());
            //Hero[] heroes = d.ToArray();
        }
    }
}
