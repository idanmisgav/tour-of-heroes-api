using System.Threading.Tasks;
using System.Web.Http;
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
            if (!MongoDB_Manager.GetDB_Changed())
            {
                return Ok(MongoDB_Manager.GetHeroes());
            }
            MongoDB_Manager.GetCurrentHeroes();
            Hero[] heroes = MongoDB_Manager.GetHeroes();
            return Ok(heroes);

            //Hero[] heroes = HeroesDB_Manager.GetCurrentHeroes();
            //return Ok(heroes);
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
