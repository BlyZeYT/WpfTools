namespace WpfTools.Internals;

using Newtonsoft.Json;
using System.IO;

public static class Json
{
    public static T? Read<T>(string path) where T : class
    {
        using (var sr = new StreamReader(path))
        {
            return JsonConvert.DeserializeObject<T>(sr.ReadToEnd());
        }
    }

    public static void Write<T>(string path, T obj) where T : class
    {
        using (var sw = new StreamWriter(path))
        {
            sw.Write(JsonConvert.SerializeObject(obj));
        }
    }
}