using GalaSoft.MvvmLight;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoutubeCenter.Library.Model
{
    public class Settings : ObservableObject
    {
        public static Settings Instance = new Settings();
        public static readonly string Location;

        private const string ApplicationName = "YoutubeCenter";

        static Settings()
        {
            var applicationPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ApplicationName);

            if (!Directory.Exists(applicationPath))
                Directory.CreateDirectory(applicationPath);

            Location = Path.Combine(applicationPath, "settings.json");

            if (File.Exists(Location))
                Instance = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(Location));
        }

        public static void Safe()
        {
            File.WriteAllText(Location, JsonConvert.SerializeObject(Instance, Formatting.Indented));
        }

        private Settings()
        {
            this.YoutubeApiKey = "";
            this.DownloadPath = "";
            this.DatabaseLocation = "";
        }

        public Settings(string YoutubeApiKey, string DownloadPath, string DatabaseLocation)
        {
            this.YoutubeApiKey = YoutubeApiKey;
            this.DownloadPath = DownloadPath;
            this.DatabaseLocation = DatabaseLocation;
        }

        public string YoutubeApiKey { get; set; }
        public string DownloadPath { get; set; }
        public string DatabaseLocation { get; set; }
    }
}
