using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Diplom.Service.API.SD;

namespace Diplom.Service.API.Models
{
    public class Message
    {
        [Key]
        [HiddenInput(DisplayValue = false)]
        public int MessageId { get; set; }
        [Required]
        public string Date { get; set; }
        [Required]
        public string Info { get; set; }
        [Required]
        public MessageType TypeInfo { get; set; } = MessageType.String;
    }
}
