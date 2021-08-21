using Newtonsoft.Json;

namespace Fanex.Athena.PlayerAdmin.Core.Helpers
{
    public static class JsonHelper
    {
        public static string ToJson(this object value)
        {
            return JsonConvert.SerializeObject(value);
        }
    }
}