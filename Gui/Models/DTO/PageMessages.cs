namespace Gui.Models.DTO
{
    public class PageMessages
    {
        public IEnumerable<MessageDTO> messages { get; set; }
        public ulong countAllMessages { get; set; }
    }
}
