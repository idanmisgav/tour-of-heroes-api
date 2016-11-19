using System.Threading.Tasks;
using System.Web.Http;
using WebApplication2;
using WebApplication2.data;
using WebApplication2.dataAccess;
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
            if (!MongoDB_Manager.DB_Changed)
            {
                return Ok(MongoDB_Manager.heroes);
            }
            MongoDB_Manager.GetCurrentHeroes();
            return Ok(MongoDB_Manager.heroes);
        }

        [HttpDelete]
        [Route("deleteHero/{hero}")]
        public IHttpActionResult DeleteHero([FromBody] Hero hero)
        {
            MongoDB_Manager.DeleteHeroFromDB(hero);
            Hero[] heroes = HeroesDB_Manager.GetCurrentHeroes();
            return Ok(heroes);
        }

        [HttpPost]
        [Route("addNewHero")]
        public IHttpActionResult AddNewHero([FromBody] Hero hero)
        {
            MongoDB_Manager.AddHeroToDB(hero);
            MongoDB_Manager.GetCurrentHeroes();
            return Ok(MongoDB_Manager.heroes);
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
