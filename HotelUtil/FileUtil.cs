using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace HotelUtil
{
    public class FileUtil<T>
    {
        private static FileUtil<T>? instance;
        public FileUtil()
        {
            if (instance == null)
                instance = new FileUtil<T>();
        }

        public static List<string> GetAccount()
        {
            var json = File.ReadAllText("appsettings.json");
            var appSettings = JsonObject.Parse(json);
            List<string> account = new List<string>();
            account.Add(appSettings["DefaultAccount"]["Email"].ToString());
            account.Add(appSettings["DefaultAccount"]["Pwd"].ToString());
            return account;
        }

        public static void WriteFile(List<T> t, string path, string type)
        {
            var fullpath = path + type;
            switch (type)
            {
                case ".json":
                    JsonSerializerOptions options = new JsonSerializerOptions()
                    {
                        WriteIndented = true,
                        ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles
                    };
                    string json = JsonSerializer.Serialize(t, options);
                    File.WriteAllText(fullpath, json);
                    break;
                case ".xml":
                    XmlSerializer xml = new XmlSerializer(typeof(List<T>));
                    using (StreamWriter writer = new StreamWriter(fullpath))
                        xml.Serialize(writer, t);
                    break;
            }
        }

        public static List<T>? ReadFile(string path, string type)
        {
            var fullpath = path + type;
            switch (type)
            {
                case ".json":
                    var json = File.ReadAllText(fullpath);
                    return JsonSerializer.Deserialize<List<T>>(json);
                case ".xml":
                    XmlSerializer xml = new XmlSerializer(typeof(List<T>));
                    using (StreamReader reader = new StreamReader(fullpath))
                        return xml.Deserialize(reader) as List<T>;
                default:
                    return null;
            }
        }
    }
}
