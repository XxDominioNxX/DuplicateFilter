using System.ComponentModel.DataAnnotations;
using static Gui.SD;

namespace Gui.Models
{
    public class Message
    {
        [Key]
        public int MessageId { get; set; }
        [Required]
        public string Date { get; set; }
        [Required]
        public string Info { get; set; }
        [Required]
        public MessageType TypeInfo { get; set; } = MessageType.String;
    }
}
