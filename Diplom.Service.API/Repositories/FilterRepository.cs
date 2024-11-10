using AutoMapper;
using Diplom.Service.API.DBContext;
using HashLib;
using Diplom.Service.API.Models;
using Diplom.Service.API.Models.DTO;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using BloomFilter;
using System.Collections;
using Newtonsoft.Json;
using Microsoft.VisualBasic.Devices;

namespace Diplom.Service.API.Repositories
{
    public class FilterRepository : IFilterRepository
    {
        private readonly ApplicationDBContext _dbContext;
        private IMapper _mapper;
        private IBloomFilter _filter;
        private readonly IHash murmurObj;
        public Analitics analitics { get; set; }


        public FilterRepository(ApplicationDBContext db, IMapper mapper)
        {
            _dbContext = db;
            _mapper = mapper;
            _filter = SD.filter;
            analitics = SD.analitics;
            murmurObj = HashFactory.Hash64.CreateMurmur2();
        }

        public async Task<String> AddElement(MessageDTO messageDTO)
        {
            if (messageDTO != null)
            {
                bool result;
                messageDTO.Date = DateTime.Now.ToLocalTime().ToString();
                var analiticsTime = $"{DateTime.Now.ToString("MM/dd/yyyy")}\n{DateTime.Now.Hour}:00";
                if (analitics.Dates.ContainsKey(analiticsTime))
                {
                    analitics.Dates[analiticsTime]++;
                }
                else
                {
                    analitics.Dates[analiticsTime] = 1;
                }
                switch (messageDTO.TypeInfo)
                {
                    case SD.MessageType.String:
                        if (analitics.CountTypes.ContainsKey(SD.MessageType.String))
                        {
                            analitics.CountTypes[SD.MessageType.String]++;
                        }
                        else
                        {
                            analitics.CountTypes[SD.MessageType.String] = 1;
                        }
                        result = _filter.Add(messageDTO.Info.Trim());
                        if (result)
                        {
                            analitics.AddedCount++;
                        }
                        else
                        {
                            analitics.DuplicateCount++;
                        }
                        return await AddingToDataBaseAsync(result, messageDTO);
                    case SD.MessageType.FilePath:
                        if (analitics.CountTypes.ContainsKey(SD.MessageType.FilePath))
                        {
                            analitics.CountTypes[SD.MessageType.FilePath]++;
                        }
                        else
                        {
                            analitics.CountTypes[SD.MessageType.FilePath] = 1;
                        }
                        result = _filter.Add(await HashFile(messageDTO.Info));
                        if (result)
                        {
                            analitics.AddedCount++;
                        }
                        else
                        {
                            analitics.DuplicateCount++;
                        }
                        return await AddingToDataBaseAsync(result, messageDTO);
                    case SD.MessageType.ByteArray:
                        if (analitics.CountTypes.ContainsKey(SD.MessageType.ByteArray))
                        {
                            analitics.CountTypes[SD.MessageType.ByteArray]++;
                        }
                        else
                        {
                            analitics.CountTypes[SD.MessageType.ByteArray] = 1;
                        }
                        result = _filter.Add(System.Text.Encoding.Default.GetBytes(messageDTO.Info));
                        if (result)
                        {
                            analitics.AddedCount++;
                        }
                        else
                        {
                            analitics.DuplicateCount++;
                        }
                        return await AddingToDataBaseAsync(result, messageDTO);
                    case SD.MessageType.FileStrings:
                        analitics.Dates[analiticsTime] = 0;
                        using (StreamReader f = new StreamReader(messageDTO.Info.Trim(), Encoding.GetEncoding(1251)))
                        {
                            string line;
                            while (!f.EndOfStream)
                            {
                                if (analitics.CountTypes.ContainsKey(SD.MessageType.String))
                                {
                                    analitics.CountTypes[SD.MessageType.String]++;
                                }
                                else
                                {
                                    analitics.CountTypes[SD.MessageType.String] = 1;
                                }

                                line = f.ReadLine();
                                result = _filter.Add(line);
                                messageDTO.Info = line;
                                messageDTO.TypeInfo = SD.MessageType.String;
                                if (result)
                                {
                                    analitics.AddedCount++;
                                }
                                else
                                {
                                    analitics.DuplicateCount++;
                                }
                                await AddingToDataBaseAsync(result, messageDTO);
                                analitics.Dates[analiticsTime]++;
                            }
                        }
                        return "Success";
                }
            }
            return "Error adding";
        }

        public async Task<bool> Clear()
        {
            try
            {
                foreach (var entity in _dbContext.Messages)
                    _dbContext.Messages.Remove(entity);
                await _dbContext.SaveChangesAsync();
                _filter.Clear();
                return true;
            }
            catch (Exception) { return false; }
        }

        public async Task<string> ContainElement(MessageDTO messageDTO)
        {
            return await Task.Run(async () =>
            {
                if (messageDTO != null)
                {
                    bool result;
                    switch (messageDTO.TypeInfo)
                    {
                        case SD.MessageType.String:
                            result = _filter.Contains(messageDTO.Info);
                            return result.ToString();
                        case SD.MessageType.FilePath:
                            result = _filter.Contains(await HashFile(messageDTO.Info));
                            return result.ToString();
                        case SD.MessageType.ByteArray:
                            result = _filter.Contains(System.Text.Encoding.Default.GetBytes(messageDTO.Info));
                            return result.ToString();
                    }
                }
                return "Error checking";
            });

        }

        //public async Task<string> RemoveElement(MessageDTO messageDTO)
        //{
        //    if (messageDTO != null)
        //    {
        //        bool result;
        //        switch (messageDTO.TypeInfo)
        //        {
        //            case SD.MessageType.String:
        //                result = _filter.Delete(messageDTO.Info, false);
        //                return await DeletingToDataBaseAsync(result, messageDTO);
        //            case SD.MessageType.FilePath:
        //                result = _filter.Delete(messageDTO.Info, true);
        //                return await DeletingToDataBaseAsync(result, messageDTO);
        //            case SD.MessageType.ByteArray:
        //                result = _filter.Delete(System.Text.Encoding.Default.GetBytes(messageDTO.Info));
        //                return await DeletingToDataBaseAsync(result, messageDTO);
        //        }
        //    }
        //    return "Error removing";
        //}

        public async Task<IEnumerable<MessageDTO>> GetMessages(int id)
        {
            int pageSize = 100;
            if (id > 0)
            {
                int beginSkip = (id - 1) * pageSize;
                int endSkip = _dbContext.Messages.Count() - beginSkip > pageSize ? _dbContext.Messages.Count() - beginSkip - pageSize : 0;
                var allMessages = await _dbContext.Messages.ToListAsync();
                allMessages = allMessages.Skip(beginSkip).SkipLast(endSkip).ToList();
                return _mapper.Map<List<MessageDTO>>(allMessages);
            }
            if (id == 0)
            {
                var allMessages = _dbContext.Messages.ToList();
                return _mapper.Map<List<MessageDTO>>(allMessages);
            }
            var Messages = new List<Message>();
            return _mapper.Map<List<MessageDTO>>(Messages);
        }

        public async Task<ulong> GetCountElements()
        {
            return await Task.Run(() =>
            {
                return (ulong)_dbContext.Messages.Count();
            });
        }

        public async Task<bool> Save(string path)
        {
            return await Task.Run(() =>
            {
                if (File.Exists(path)) File.Delete(path);

                using (BinaryWriter writer2 = new BinaryWriter(File.Open(path, FileMode.OpenOrCreate)))
                {
                    writer2.Write(ToByteArray(_filter.GetBitArray()));
                }
                using (StreamWriter writer = new StreamWriter(path, true))
                {
                    writer.WriteLine();
                    writer.WriteLine(_filter.GetBitArray().Count.ToString());
                }
                return true;
            });
        }

        public async Task<bool> Load(string path)
        {
            return await Task.Run(() =>
            {
                if (File.Exists(path))
                {
                    List<byte> buf = new List<byte>();

                    using (BinaryReader reader = new BinaryReader(File.Open(path, FileMode.OpenOrCreate)))
                    {
                        while (reader.PeekChar() > -1)
                        {
                            buf.Add(reader.ReadByte());
                        }
                    }
                    var byteArray = buf.Select(x => ReverseBits(x)).ToArray();
                    BitArray bit = new BitArray(byteArray);
                    var len = File.ReadLines(path).ToList();
                    bit.Length = Convert.ToInt32(len[len.Count() - 1]);
                    _filter.SetBitArray(bit);
                    return true;
                }
                else
                {
                    return false;
                }  
            });
        }

        public async Task<bool> SaveAnalitics(string path)
        {
            return await Task.Run(() =>
            {
                if (File.Exists(path)) File.Delete(path);
                string json = JsonConvert.SerializeObject(SD.analitics);
                File.WriteAllText(path, json);
                return true;
            });
        }

        public async Task<bool> LoadAnalitics(string path)
        {
            return await Task.Run(() =>
            {
                if (File.Exists(path))
                {
                    string json = File.ReadAllText(path);
                    SD.analitics = JsonConvert.DeserializeObject<Analitics>(json);
                    return true;
                }
                else { return false; }
            });
        }

        public async Task<IEnumerable<string>> GetFilesPath(string directoryPath)
        {
            return await Task.Run(() =>
            {
                if (Directory.Exists(directoryPath))
                {
                    return Directory.GetFiles(directoryPath).ToList();
                }
                else
                {
                    return new List<string>();
                }
            });
        }

        //-----------------Вспомогательные функции----------------

        private async Task<string> AddingToDataBaseAsync(bool result, MessageDTO messageDTO)
        {
            if (result)
            {
                var message = _mapper.Map<Message>(messageDTO);
                var IsSuccess = await _dbContext.Messages.AddAsync(message);
                _dbContext.SaveChanges();
                return IsSuccess.State.ToString();
            }
            else
            {
                return "Элемент уже содержится в Базе данных";
            }
        }

        private async Task<string> DeletingToDataBaseAsync(bool result, MessageDTO messageDTO)
        {
            return await Task.Run(() =>
            {
                if (result)
                {
                    var message = _mapper.Map<Message>(messageDTO);
                    var IsSuccess = _dbContext.Messages.Remove(message);
                    _dbContext.SaveChanges();
                    return IsSuccess.State.ToString();
                }
                else
                {
                    return "Элемент не содержится в Базе данных";
                }
            });
        }

        private async Task<string> HashFile(string filePath)
        {
            return await Task.Run(() =>
            {
                if (filePath != null && filePath != "")
                {
                    if (File.Exists(filePath))
                    {
                        return murmurObj.ComputeFile(filePath).ToString();
                    }
                    return "Error";
                }
                return "Error";
            });
        }

        private byte[] ToByteArray(BitArray bits)
        {
            int numBytes = bits.Count / 8;
            if (bits.Count % 8 != 0) numBytes++;

            byte[] bytes = new byte[numBytes];
            int byteIndex = 0, bitIndex = 0;

            for (int i = 0; i < bits.Count; i++)
            {
                if (bits[i])
                    bytes[byteIndex] |= (byte)(1 << (7 - bitIndex));

                bitIndex++;
                if (bitIndex == 8)
                {
                    bitIndex = 0;
                    byteIndex++;
                }
            }
            return bytes;
        }

        private byte ReverseBits(byte number)
        {
            number = (byte)((number & 0x55) << 1 | (number & 0xAA) >> 1);
            number = (byte)((number & 0x33) << 2 | (number & 0xCC) >> 2);
            number = (byte)((number & 0x0F) << 4 | (number & 0xF0) >> 4);

            return number;
        }
    }
}
