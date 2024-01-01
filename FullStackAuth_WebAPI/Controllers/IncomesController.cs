using FullStackAuth_WebAPI.Data;
using FullStackAuth_WebAPI.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FullStackAuth_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IncomesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public IncomesController(ApplicationDbContext context)
        {
            _context = context;
        }



        // GET: api/<IncomesController>
        [HttpGet]
        public IActionResult  Get()
        {
            var incomes = _context.Incomes.ToList();
            return Ok(incomes);
        }

        // GET api/<IncomesController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var income = _context.Incomes.Find(id);
            if (income == null)
            {
                return NotFound();
            }
            return Ok(income);
        }

        // POST api/<IncomesController>
        [HttpPost]
        public IActionResult Post([FromBody] Income income)
        {
            _context.Incomes.Add(income);
            _context.SaveChanges();
            return StatusCode(201, income);
        }

        // PUT api/<IncomesController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Income income)
        {
            var existingIncome = _context.Expenses.FirstOrDefault(i => i.Id == id);
            if(existingIncome == null)
            {
                return NotFound(id);
            }
            else
            {
                existingIncome.Name = income.Name;
                existingIncome.Amount = income.Amount;
                existingIncome.Date = income.Date;
                _context.SaveChanges();
                return StatusCode(200, income);
            }
        }

        // DELETE api/<IncomesController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var income = _context.Incomes.Find(id);
            if (income == null)
            {
                return NotFound();
            }
            _context.Incomes.Remove(income);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
