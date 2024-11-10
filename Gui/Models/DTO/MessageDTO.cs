using System.ComponentModel.DataAnnotations;
using static Gui.SD;

namespace Gui.Models.DTO
{
    public class MessageDTO
    {
        public string Date { get; set; }
        public string Info { get; set; }
        public MessageType TypeInfo { get; set; } = MessageType.String;
    }
}
