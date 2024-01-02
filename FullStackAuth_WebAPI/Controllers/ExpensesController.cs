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
    public class ExpensesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ExpensesController(ApplicationDbContext context)
        {
            _context = context;
        }


        // GET: api/<ExpenseController>
        [HttpGet]
        public IActionResult GetAllExpenses()
        {

            try
            {
                
                var expenses = _context.Expenses.Select(e => new ExpenseWithUserDto
                {
                    Id = e.Id,
                    Name = e.Name,
                    Amount = e.Amount,
                    Date = e.Date,
                    Budgeter = new UserForDisplayDto
                    {
                        Id = e.Budgeter.Id,
                        FirstName = e.Budgeter.FirstName,
                        LastName = e.Budgeter.LastName,
                        UserName = e.Budgeter.UserName,
                    }
                }).ToList();
                return StatusCode(200, expenses);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpGet("myExpenses"), Authorize]
        public IActionResult GetUserExpenses()
        {
            try
            {
                string userId = User.FindFirstValue("id");
                var expenses = _context.Expenses.Where(e => e.BudgeterId.Equals(userId));
                return StatusCode(200, expenses);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET api/<ExpenseController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {

                var expense = _context.Expenses.Include(e => e.Budgeter).FirstOrDefault(e => e.Id == id);

                if (expense == null)
                {
                    return NotFound();
                }
                return StatusCode(200, expense);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // POST api/<ExpenseController>
        [HttpPost]
        public IActionResult Post([FromBody] Expense expense)
        {
            try
            {
                string userId = User.FindFirstValue("id");
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized();
                }

                expense.BudgeterId = userId;
                _context.Expenses.Add(expense);
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                _context.SaveChanges();
                return StatusCode(201, expense);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // PUT api/<ExpenseController>/5
        [HttpPut("{id}"), Authorize]
        public IActionResult Put(int id, [FromBody] Expense expense)
        {
            try
            {

                Expense existingExpense = _context.Expenses.Include(e=>e.Budgeter).FirstOrDefault(e => e.Id == id);

                if (existingExpense == null)
                {
                    return NotFound();
                }
                var userId = User.FindFirstValue("id");
                if(string.IsNullOrEmpty(userId) || expense.BudgeterId != userId)
                {
                    return Unauthorized();
                }
                existingExpense.BudgeterId = userId;
                existingExpense.Budgeter = _context.Users.Find(userId);
                    existingExpense.Name = expense.Name;
                    existingExpense.Amount = expense.Amount;
                    existingExpense.Date = expense.Date;
                    existingExpense.Rating = expense.Rating;
                    existingExpense.IsPaid = expense.IsPaid;
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                 _context.SaveChanges();
                 return StatusCode(200, expense);
                
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // DELETE api/<ExpenseController>/5
        [HttpDelete("{id}"), Authorize]
        public IActionResult Delete(int id)
        {
            try
            {


                Expense expense = _context.Expenses.FirstOrDefault(e => e.Id == id);
                if (expense == null)
                {
                    return NotFound();
                }

                var userId = User.FindFirstValue("id");
                if (string.IsNullOrEmpty(userId) || expense.BudgeterId != userId)
                {
                    return Unauthorized();
                }
                _context.Expenses.Remove(expense);
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
