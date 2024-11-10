using static Gui.SD;

namespace Gui.Models
{
    public class ApiRequest
    {
        public ApiType ApiType { get; set; } = ApiType.GET;
        public string ApiUrl { get; set; }
        public object ApiData { get; set; }
    }
}
