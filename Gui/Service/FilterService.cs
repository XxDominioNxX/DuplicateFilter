using Gui.Models.DTO;
using Gui.Services.IServices;
using Gui.Services;

namespace Gui.Services
{
    public class FilterService : BaseService, IFilterService
    {
        private readonly HttpClient _clientFactory;
        public FilterService(HttpClient clientFactory) : base(clientFactory) 
        {
            _clientFactory = clientFactory;
        }

        public async Task<T> AddMessageAsync<T>(MessageDTO meassageDTO)
        {
            return await this.SendAsync<T>(new Models.ApiRequest()
            {
                ApiType = SD.ApiType.POST,
                ApiData = meassageDTO,
                ApiUrl = SD.FilterAPIBase + "api/" + "add"
            });
        }

        public async Task<T> ClearMessagesAsync<T>()
        {
            return await this.SendAsync<T>(new Models.ApiRequest()
            {
                ApiType = SD.ApiType.GET,
                ApiUrl = SD.FilterAPIBase + "api/" + "clear"
            });
        }

        public async Task<T> ConsistMessageAsync<T>(MessageDTO meassageDTO)
        {
            return await this.SendAsync<T>(new Models.ApiRequest()
            {
                ApiType = SD.ApiType.POST,
                ApiData = meassageDTO,
                ApiUrl = SD.FilterAPIBase + "api/" + "consist"
            });
        }

        public async Task<T> GetAllMessagesAsync<T>(ulong id)
        {
            return await this.SendAsync<T>(new Models.ApiRequest()
            {
                ApiType = SD.ApiType.GET,
                ApiUrl = SD.FilterAPIBase + "api/" + "getMessages/" + id
            });
        }

        public async Task<T> SaveAsync<T>()
        {
            return await this.SendAsync<T>(new Models.ApiRequest()
            {
                ApiType = SD.ApiType.GET,
                ApiUrl = SD.FilterAPIBase + "api/" + "save"
            });
        }

        public async Task<T> LoadAsync<T>()
        {
            return await this.SendAsync<T>(new Models.ApiRequest()
            {
                ApiType = SD.ApiType.GET,
                ApiUrl = SD.FilterAPIBase + "api/" + "load"
            });
        }

        public async Task<T> GetFilesPath<T>(string directoryPath)
        {
            return await this.SendAsync<T>(new Models.ApiRequest()
            {
                ApiType= SD.ApiType.GET,
                ApiUrl = SD.FilterAPIBase + "api/" + "getFiles?directoryPath=" + directoryPath
            });
        }

        public async Task<T> GetAnalitics<T>()
        {
            return await this.SendAsync<T>(new Models.ApiRequest()
            {
                ApiType = SD.ApiType.GET,
                ApiUrl = SD.FilterAPIBase + "api/" + "analitics"
            });
        }
    }
}
