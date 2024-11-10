namespace Gui.Models
{
    public class MenuElement
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? IsActive { get; set; }
        public string Link { get; set; }
        public List<string> svg { get; set; }
    }
}
