namespace Razor.Components.Models
{
    public class RoleAttributes
    {
        public int Id { get; set; }
        public required string RoleId { get; set; }
        public required string Rule { get; set; }
        public int Value { get; set; }

    }
}
