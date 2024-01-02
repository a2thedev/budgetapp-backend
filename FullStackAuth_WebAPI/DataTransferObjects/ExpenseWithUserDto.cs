namespace FullStackAuth_WebAPI.DataTransferObjects
{
    public class ExpenseWithUserDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public double Amount { get; set; }

        public DateTime Date { get; set; }

        public int Rating { get; set; }

        public bool IsPaid { get; set; }

        public UserForDisplayDto Budgeter { get; set; }
    }
}
