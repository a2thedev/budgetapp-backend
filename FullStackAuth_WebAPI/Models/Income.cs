using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FullStackAuth_WebAPI.Models
{
    public class Income
    {
        [Key]

        public int Id { get; set; }

        public string Name { get; set; }

        public double Amount { get; set; }

        public DateOnly Date { get; set; }

        [ForeignKey("Budgeter")]

        public string BudgeterId { get; set; }

        public User Budgeter { get; set; }
    }
}
