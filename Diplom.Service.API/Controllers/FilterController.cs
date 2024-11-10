using Diplom.Service.API.DBContext;
using Diplom.Service.API.Models.DTO;
using Diplom.Service.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.VisualBasic.Devices;

namespace Diplom.Service.API.Controllers
{
    [Route("api/")]
    public class FilterController : ControllerBase
    {
        protected ResponseDTO _response;
        private IFilterRepository _filterRepository;
        public FilterController(IFilterRepository filterRepository)
        {
            _filterRepository = filterRepository;
            _response = new ResponseDTO();
        }

        [HttpPost]
        [Route("add")]
        public async Task<object> AddElement([FromBody] MessageDTO message)
        {
            try
            {
                var resultAdding = await _filterRepository.AddElement(message);
                _response.Result = resultAdding;
                _response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages =
                     new List<string> { ex.ToString() };
            }
            return _response;
        }

        //[HttpPost]
        //[Route("remove")]
        //public async Task<object> RemoveElement([FromBody] MessageDTO message)
        //{
        //    try
        //    {
        //        var resultRemoving = await _filterRepository.RemoveElement(message);
        //        _response.Result = resultRemoving;
        //        _response.IsSuccess = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        _response.IsSuccess = false;
        //        _response.ErrorMessages =
        //             new List<string> { ex.ToString() };
        //    }
        //    return _response;
        //}

        [HttpPost]
        [Route("contain")]
        public async Task<object> ConsistElement([FromBody] MessageDTO message)
        {
            try
            {
                var result = await _filterRepository.ContainElement(message);
                _response.Result =  $"Consist: {result}";
                _response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages =
                     new List<string> { ex.ToString() };
            }
            return _response;
        }

        [HttpGet]
        [Route("clear")]
        public async Task<object> Clear()
        {
            try
            {
                var result = await _filterRepository.Clear();
                _response.Result = result;
                _response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages =
                     new List<string> { ex.ToString() };
            }
            return _response;
        }

        [HttpGet]
        [Route("save")]
        public async Task<object> Save(string path)
        {
            try
            {
                if (path == null) { path = "save.dat"; }
                _response.Result = (await _filterRepository.Save(path)) & (await _filterRepository.SaveAnalitics("analiticsSave.dat"));
                _response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages =
                     new List<string> { ex.ToString() };
            }
            return _response;
        }

        [HttpGet]
        [Route("load")]
        public async Task<object> Load(string path)
        {
            try
            {
                if (path == null) { path = "save.dat"; }
                _response.Result = (await _filterRepository.Load(path)) & (await _filterRepository.LoadAnalitics("analiticsSave.dat"));
                _response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages =
                     new List<string> { ex.ToString() };
            }
            return _response;
        }

        [HttpGet]
        [Route("getMessages/{id}")]
        public async Task<object> GetAll(int id)
        {
            try
            {
                PageMessages res = new PageMessages();
                res.messages = await _filterRepository.GetMessages(id);
                res.countAllMessages = await _filterRepository.GetCountElements();
                _response.Result = res;
                _response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages =
                     new List<string> { ex.ToString() };
            }
            return _response;
        }

        [HttpGet]
        [Route("getFiles")]
        public async Task<object> GetFiles(string directoryPath)
        {
            try
            {
                var result = await _filterRepository.GetFilesPath(directoryPath);
                _response.Result = result;
                _response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages =
                     new List<string> { ex.ToString() };
            }
            return _response;
        }

        [HttpGet]
        [Route("analitics")]
        public async Task<object> GetAnalitics()
        {
            return await Task.Run(() =>
            {
                try
                {
                    _filterRepository.analitics.TotalRAM = ConvertBytesToGB(new ComputerInfo().TotalPhysicalMemory);
                    _filterRepository.analitics.FreeRAM = ConvertBytesToGB(new ComputerInfo().AvailablePhysicalMemory);
                    _filterRepository.analitics.FreeDiskMemory = ConvertBytesToGB(new DriveInfo(Directory.GetCurrentDirectory().Substring(0, 2)).TotalFreeSpace);
                    _filterRepository.analitics.TotalDiskMemory = ConvertBytesToGB(new DriveInfo(Directory.GetCurrentDirectory().Substring(0, 2)).TotalSize);

                    _response.Result = _filterRepository.analitics;
                    _response.IsSuccess = true;
                }
                catch (Exception ex)
                {
                    _response.IsSuccess = false;
                    _response.ErrorMessages =
                         new List<string> { ex.ToString() };
                }
                return _response;
            });
        }

        private double ConvertBytesToGB(object bytes)
        {
            return Math.Round(Convert.ToDouble(bytes) / 1073741824, 3);
        }
    }
}
