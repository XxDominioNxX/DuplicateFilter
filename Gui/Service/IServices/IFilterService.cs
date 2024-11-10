using Gui.Models.DTO;
using Gui.Services.IServices;

namespace Gui.Services.IServices
{
    public interface IFilterService : IBaseService
    {
        Task<T> GetAllMessagesAsync<T>(ulong id);
        Task<T> ConsistMessageAsync<T>(MessageDTO meassageDTO);
        Task<T> AddMessageAsync<T>(MessageDTO meassageDTO);
        Task<T> ClearMessagesAsync<T>();
        Task<T> SaveAsync<T>();
        Task<T> LoadAsync<T>();
        Task<T> GetFilesPath<T>(string directoryPath);

    }
}
