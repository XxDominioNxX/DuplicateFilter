using BloomFilter;
using Diplom.Service.API.Models;


namespace Diplom.Service.API
{
    public static class SD
    {

        public static IBloomFilter filter = FilterBuilder.Build(100000000, 0.001);

        //public static IFilter filter = new SharpQuotFilt();
        public static Analitics analitics = new Analitics();
        public enum MessageType
        {
            String,
            FilePath,
            ByteArray,
            FileStrings
        }
    }
}
