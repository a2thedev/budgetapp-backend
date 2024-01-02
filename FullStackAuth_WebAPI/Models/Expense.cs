using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FullStackAuth_WebAPI.Models
{
    public class Expense
    {
        [Key]

        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public double Amount { get; set; }
        [Required]
        public DateOnly Date { get; set; }
        [Required]
        public int Rating { get; set; }
        [Required]
        public bool IsPaid { get; set; }

        [ForeignKey("Budgeter")]

        public string BudgeterId { get; set; }

        public User Budgeter { get; set; }
    }
}
