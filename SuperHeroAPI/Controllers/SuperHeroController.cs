using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuperHeroAPI.Data;
using SuperHeroAPI.Entities;

namespace SuperHeroAPI.Controllers
{
    // NOTE: those surrounded by "[]" are attributes (or think of it as metadata that provides declarative information to your code
    [Route("api/[controller]")]
    [ApiController]

    public class SuperHeroController : ControllerBase
    {

        /*
        // === simple example WITHOUT DATABASE use === 
        
        [HttpGet]   // indicates the request method 
        public async Task<ActionResult<List<SuperHero>>> GetAllHeroes()
        {
            var heroes = new List<SuperHero>
                {
                    new SuperHero
                    {
                        Id = 1,
                        Name = "Spiderman",
                        FirstName = "Peter",
                        LastName = "Parker",
                        Place = "New York City"
                    }
                };

            return Ok(heroes);
        }
        */


        private readonly DataContext _context;
        public SuperHeroController(DataContext context)
        {
            _context = context;
        }


        // READ: GET all with DATABASE USE
        [HttpGet]   
        public async Task<ActionResult<List<SuperHero>>> GetAllHeroes()
        {
            var heroes = await _context.SuperHeroes.ToListAsync();

            return Ok(heroes);
        }


        // READ: GET one
        [HttpGet("{id}")]   // append the additional route 
        public async Task<ActionResult<SuperHero>> GetHero(int id)
        {
            var hero = await _context.SuperHeroes.FindAsync(id);
            if (hero == null)
                return NotFound("Hero not found.");

            return Ok(hero);
        }


        // CREATE: POST one
        [HttpPost]
        public async Task<ActionResult<List<SuperHero>>> AddHero(SuperHero hero)
        {
            _context.SuperHeroes.Add(hero); // NOTE: "Add" will not save the change (only tracks the entity)
            await _context.SaveChangesAsync();  // this does the saving

            return Ok(await _context.SuperHeroes.ToListAsync());
        }


        // UPDATE: PUT (modify existing data) one
        [HttpPut]
        public async Task<ActionResult<List<SuperHero>>> UpdateHero(SuperHero updatedHero)
        {
            var dbHero = await _context.SuperHeroes.FindAsync(updatedHero.Id);
            if (dbHero == null)
                return NotFound("Hero not found.");

            dbHero.Name = updatedHero.Name;
            dbHero.FirstName = updatedHero.FirstName;
            dbHero.LastName = updatedHero.LastName;
            dbHero.Place = updatedHero.Place;

            await _context.SaveChangesAsync();

            return Ok(await _context.SuperHeroes.ToListAsync());
        }


        // DELETE: DELETE one
        [HttpDelete]
        public async Task<ActionResult<List<SuperHero>>> DeleteHero(int id)
        {
            var dbHero = await _context.SuperHeroes.FindAsync(id);
            if (dbHero == null)
                return NotFound("Hero not found.");

            _context.SuperHeroes.Remove(dbHero);
            await _context.SaveChangesAsync();

            return Ok(await _context.SuperHeroes.ToListAsync());
        }
    }
}
