namespace FullStackAuth_WebAPI.DataTransferObjects
{
    public class IncomeWithUserDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public double Amount { get; set; }

        public DateOnly Date { get; set; }
        public UserForDisplayDto Budgeter { get; set; }
    }
}
