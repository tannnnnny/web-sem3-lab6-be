namespace Lab6.Models
{
    public class Dropdown
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<string> Items { get; set; } = new List<string>();
    }

    public class DropdownItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
