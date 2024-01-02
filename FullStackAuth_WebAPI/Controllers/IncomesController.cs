using FullStackAuth_WebAPI.Data;
using FullStackAuth_WebAPI.DataTransferObjects;
using FullStackAuth_WebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

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
        public IActionResult  GetAllIncomes()
        {
            try
            {
                var incomes = _context.Incomes.Select(i => new IncomeWithUserDto
                {
                    Id = i.Id,
                    Name = i.Name,
                    Amount = i.Amount,
                    Date = i.Date,
                    Budgeter = new UserForDisplayDto
                    {
                        Id=i.Budgeter.Id,
                        FirstName=i.Budgeter.FirstName,
                        LastName=i.Budgeter.LastName,
                        UserName=i.Budgeter.UserName,
                    }
                }).ToList();
                return StatusCode(200,incomes);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("MyIncomes"),Authorize]
        public IActionResult GetUsersIncomes()
        {
            try
            {
                string userId = User.FindFirstValue("id");
                var incomes = _context.Incomes.Where(i => i.BudgeterId.Equals(userId));
                return StatusCode(200, incomes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
           

        
        // GET api/<IncomesController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var income = _context.Incomes.Include(i => i.Budgeter).FirstOrDefault(i => i.Id == id);
                if (income == null)
                {
                    return NotFound();
                }
                return StatusCode(200, income);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // POST api/<IncomesController>
        [HttpPost, Authorize]
        public IActionResult Post([FromBody] Income income)
        {
            try
            {
                string userId = User.FindFirstValue("id");
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized();
                }

                income.BudgeterId = userId;

                _context.Incomes.Add(income);
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                _context.SaveChanges();
                return StatusCode(201, income);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // PUT api/<IncomesController>/5
        [HttpPut("{id}"), Authorize]
        public IActionResult Put(int id, [FromBody] Income income)
        {
            try
            {


                Income existingIncome = _context.Incomes.Include(i => i.Budgeter).FirstOrDefault(i => i.Id == id);

                if (existingIncome == null)
                {
                    return NotFound();
                }

                var userId = User.FindFirstValue("id");
                if (string.IsNullOrEmpty(userId) || income.BudgeterId != userId)
                {
                    return Unauthorized();
                }

                existingIncome.BudgeterId = userId;
                existingIncome.Budgeter = _context.Users.Find(userId);
                existingIncome.Name = income.Name;
                existingIncome.Amount = income.Amount;
                existingIncome.Date = income.Date;
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                _context.SaveChanges();
                return StatusCode(200, income);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
            
        }

        // DELETE api/<IncomesController>/5
        [HttpDelete("{id}"), Authorize]
        public IActionResult Delete(int id)
        {
            try
            {

                Income income = _context.Incomes.FirstOrDefault(i => i.Id == id);
                if (income == null)
                {
                    return NotFound();
                }

                var userId = User.FindFirstValue("id");
                if (string.IsNullOrEmpty(userId) || income.BudgeterId != userId)
                {
                    return Unauthorized();
                }
                _context.Incomes.Remove(income);
                _context.SaveChanges();
                return StatusCode(204);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
