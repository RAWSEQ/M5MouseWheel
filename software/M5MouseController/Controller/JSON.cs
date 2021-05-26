using Newtonsoft.Json.Linq;

namespace M5MouseController.Controller
{
    class JSON
    {
        public static dynamic parse(string json)
        {
            if (json.StartsWith("{"))
            {
                return JObject.Parse(json);
            }
            else if (json.StartsWith("["))
            {
                return JObject.Parse(json);
            }
            else
            {
                return json.Trim('"');
            }
            
        }

        public static string stringify(dynamic obj)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        }
    }
}
