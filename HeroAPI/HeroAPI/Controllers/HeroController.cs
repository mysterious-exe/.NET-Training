using HeroAPI.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HeroAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HeroController : ControllerBase
    {
        private readonly DataContext _context;

        public HeroController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Hero>>> Get()
        {
            return Ok(await _context.Heroes.ToListAsync());
        }

        

        [HttpGet("{id}")]
        public async Task<ActionResult<List<Hero>>> Get(int id)
        {
            var hero = await _context.Heroes.FindAsync(id);
            if(hero == null)
            {
                return BadRequest("Hero not found");
            }
            return Ok(hero);
        }



        [HttpPost]
        public async Task<ActionResult<List<Hero>>> AddHero(Hero hero)
        {
            _context.Heroes.Add(hero);
            await _context.SaveChangesAsync();
            return Ok(await _context.Heroes.ToListAsync());
        }

        [HttpPut]
        public async Task<ActionResult<List<Hero>>> UpdateHero(Hero request)
        {
            var dbHero = await _context.Heroes.FindAsync(request.Id);
            if (dbHero == null)
            {
                return BadRequest("Hero not found");
            }
            dbHero.Name= request.Name; 
            dbHero.FirstName= request.FirstName;
            dbHero.LastName= request.LastName;
            dbHero.Place= request.Place;

            await _context.SaveChangesAsync();

            return Ok(await _context.Heroes.ToListAsync());
        }

        [HttpDelete]
        public async Task<ActionResult<List<Hero>>> DeleteHero(int id)
        {
            var dbHero = await _context.Heroes.FindAsync(id);
            if (dbHero == null)
            {
                return BadRequest("Hero not found");
            }
            _context.Heroes.Remove(dbHero);
            await _context.SaveChangesAsync();
            return Ok(await _context.Heroes.ToListAsync());
        }

    }
}
