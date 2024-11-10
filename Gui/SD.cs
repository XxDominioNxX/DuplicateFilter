using Gui.Services;

namespace Gui
{
    public static class SD
    {
        
        public static string FilterAPIBase { get; set; }
        public enum ApiType
        {
            GET,
            POST,
            PUT,
            DELETE
        }
        public enum MessageType
        {
            String,
            FilePath,
            ByteArray,
            FileStrings
        }
    }
}
