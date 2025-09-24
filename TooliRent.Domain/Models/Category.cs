namespace TooliRent.Domain.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Tool> Tools { get; set; } = new List<Tool>();
    }
}
