using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gui.Models
{
    public class Analitics
    {
        public Analitics()
        {
            Dates = new Dictionary<string, ulong>();
            CountTypes = new Dictionary<object, ulong>();
        }
        /// <summary>
        /// Количество элементов, которые добавленны в базу данных (уникальные значения)
        /// </summary>
        public ulong AddedCount { get; set; }

        /// <summary>
        /// Количество дублирующихся элементов
        /// </summary>
        public ulong DuplicateCount { get; set; }

        /// <summary>
        /// Даты последних добавленных сообщений
        /// </summary>
        public Dictionary<string, ulong> Dates { get; set; }

        /// <summary>
        /// Количество сообщений по типам
        /// </summary>
        public Dictionary<object, ulong> CountTypes { get; set; }

        /// <summary>
        /// Свободное дисковое пространство
        /// </summary>
        public double FreeDiskMemory { get; set; }

        /// <summary>
        /// Занятое дисковое пространство
        /// </summaryЮ
        public double TotalDiskMemory { get; set; }

        /// <summary>
        /// Свободная ОЗУ
        /// </summaryЮ
        public double FreeRAM { get; set; }

        /// <summary>
        /// Всего ОЗУ
        /// </summaryЮ
        public double TotalRAM { get; set; }

    }
}
