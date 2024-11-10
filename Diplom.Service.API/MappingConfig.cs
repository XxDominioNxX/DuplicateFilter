using AutoMapper;
using Diplom.Service.API.Models;
using Diplom.Service.API.Models.DTO;

namespace Diplom.Service.API
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps() 
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<MessageDTO, Message>();
                config.CreateMap<Message, MessageDTO>();
            });

            return mappingConfig;
        }
    }
}
