using System.ComponentModel.DataAnnotations;
using static Diplom.Service.API.SD;

namespace Diplom.Service.API.Models.DTO
{
    public class MessageDTO
    {
        public string Date { get; set; }
        public string Info { get; set; }
        public MessageType TypeInfo { get; set; } = MessageType.String;
    }
}
