using Diplom.Service.API.Models;
using Diplom.Service.API.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Diplom.Service.API.Repositories
{
    public interface IFilterRepository
    {
        Analitics analitics { get; set; }
        Task<IEnumerable<MessageDTO>> GetMessages(int id);
        Task<string> ContainElement(MessageDTO message);
        Task<String> AddElement(MessageDTO message);
        //Task<String> RemoveElement(MessageDTO message);
        Task<bool> Clear();
        Task<bool> Save(string path);
        Task<bool> Load(string path);
        Task<bool> LoadAnalitics(string path);
        Task<bool> SaveAnalitics(string path);
        Task<IEnumerable<string>> GetFilesPath(string directoryPath);
        Task<ulong> GetCountElements();
    }
}
