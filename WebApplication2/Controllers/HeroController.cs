using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using WebApplication2.data;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [RoutePrefix("hero")]
    public class HeroController : ApiController
    {
        [HttpGet]
        [Route("getheroes")]
        public IHttpActionResult GetAllHeroes()
        {
            Hero[] heroes = HeroesDB_Manager.GetCurrentHeroes();
            return Ok(heroes);
        }

        [HttpDelete]
        [Route("deleteHero/{id}")]
        public IHttpActionResult DeleteHero(int id)
        {
            HeroesDB_Manager.DeleteHeroFromDB(id);
            Hero[] heroes = HeroesDB_Manager.GetCurrentHeroes();
            return Ok(heroes);
        }

        [HttpPost]
        [Route("addNewHero")]
        public IHttpActionResult AddNewHero([FromBody] Hero hero)
        {
            HeroesDB_Manager.AddHeroToDB(hero.Name);
            Hero[] heroes = HeroesDB_Manager.GetCurrentHeroes();
            return Ok(heroes);
        }

        [HttpPut]
        [Route("updateHero")]
        public IHttpActionResult UpdateHero([FromBody] Hero hero)
        {
            HeroesDB_Manager.UpdateHeroInDB(hero);
            Hero[] heroes = HeroesDB_Manager.GetCurrentHeroes();
            return Ok(heroes);
        }
    }
}
