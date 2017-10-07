using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeCenter.Library.Model;

namespace YoutubeCenter.Library
{
    public static class SettingsHelper
    {
        private const string ApplicationName = "YoutubeCenter";

        public static void Safe(Settings settings, string path = null)
        {
            File.WriteAllText(CheckPath(path), JsonConvert.SerializeObject(settings, Formatting.Indented));
        }

        public static Settings Load(string path = null)
        {
            path = CheckPath(path);
            if (File.Exists(path))
                return JsonConvert.DeserializeObject<Settings>(File.ReadAllText(path));
            else
                return null;
        }

        private static string CheckPath(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                var applicationPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ApplicationName);

                if (!Directory.Exists(applicationPath))
                    Directory.CreateDirectory(applicationPath);

                return Path.Combine(applicationPath, "settings.json");
            }
            else
            {
                return path;
            }
        }
    }
}
