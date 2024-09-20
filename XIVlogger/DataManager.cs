
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace XIVstats
{
    internal class DataManager
    {
        public Dictionary<string,Location> dataManager()

        {
            //C:\Users\Zacharias\Source\Repos\SamplePlugin\SamplePlugin\Data\Cities.json
            var filepath = Path.Combine(Plugin.PluginInterface.AssemblyLocation.Directory?.FullName!, "Data\\Cities.json");
            string Json = File.ReadAllText(filepath);

            var locations = JsonConvert.DeserializeObject<Dictionary<string, Location>>(Json);
            if (locations == null) throw new FileNotFoundException();
            return locations;
        }

        public void WriteToJson(object Location)
        {
            var filepath = Path.Combine(Plugin.PluginInterface.AssemblyLocation.Directory?.FullName!, "Data\\Cities.json");
            string jsonString = JsonConvert.SerializeObject(Location);
            File.WriteAllText(filepath, jsonString);
        }
    }

    public class Location
    {
        public string Name { get; set; } = "";
        public int Count { get; set; }
    }
    
}
