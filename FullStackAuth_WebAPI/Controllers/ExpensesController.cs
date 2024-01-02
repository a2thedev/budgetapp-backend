using FullStackAuth_WebAPI.Data;
using FullStackAuth_WebAPI.DataTransferObjects;
using FullStackAuth_WebAPI.Models;
using Microsoft.AspNetCore.Mvc;


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
            

        // GET api/<ExpenseController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var expense = _context.Expenses.Find(id);
            if (expense == null)
            {
                return NotFound();
            }
            return Ok(expense);
        }

        // POST api/<ExpenseController>
        [HttpPost]
        public IActionResult Post([FromBody] Expense expense)
        {
            _context.Expenses.Add(expense);
            _context.SaveChanges();
            return StatusCode(201, expense);
        }

        // PUT api/<ExpenseController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Expense expense)
        {
            var existingExpense = _context.Expenses.FirstOrDefault(e => e.Id == id);
            if (existingExpense == null)
            {
                return NotFound(id);
            }
            else
            {
                existingExpense.Name = expense.Name;
                existingExpense.Amount = expense.Amount;
                existingExpense.Date = expense.Date;
                existingExpense.Rating = expense.Rating;
                existingExpense.IsPaid = expense.IsPaid;

                _context.SaveChanges();
                return StatusCode(200, expense);
            }
        }

        // DELETE api/<ExpenseController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var expense = _context.Expenses.Find(id);
            if (expense == null)
            {
                return NotFound();
            }
            _context.Expenses.Remove(expense);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
