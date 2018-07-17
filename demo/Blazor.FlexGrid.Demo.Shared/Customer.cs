namespace Blazor.FlexGrid.Demo.Shared
{
    public class Customer
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }

        public string SurName { get; set; }

        public string Country { get; set; }

        public string Address { get; set; }

        public override string ToString()
            => $"Id: {Id}, Email: {Email}";

    }
}
