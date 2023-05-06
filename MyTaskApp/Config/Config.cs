using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using server.Models;

namespace server.Config
{
    public class Config
    {
        public static ConfigJson ConfigJson;
        public static void LoadConfigJson()
        {
            var path = Directory.GetCurrentDirectory();
            path = path.Substring(0, path.IndexOf("\\bin"));
            path = Path.Combine(path, "ProgramConfig.json");
            using (StreamReader ProgramConfig = new StreamReader(path))
            {
                var jsonconfig = ProgramConfig.ReadToEnd();
                ConfigJson = JsonConvert.DeserializeObject<ConfigJson>(jsonconfig);
            }
        }
    }

}

