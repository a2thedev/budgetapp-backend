namespace FullStackAuth_WebAPI.DataTransferObjects
{
    public class IncomeWithUserDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public double Amount { get; set; }

        public DateTime Date { get; set; }
        public UserForDisplayDto Budgeter { get; set; }
    }
}
