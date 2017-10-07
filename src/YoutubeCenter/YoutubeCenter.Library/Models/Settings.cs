using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoutubeCenter.Library.Model
{
    public class Settings : ObservableObject
    {
        public Settings()
        {
            this.YoutubeApiKey = "";
            this.DownloadPath = "";
        }

        public Settings(string YoutubeApiKey, string DownloadPath)
        {
            this.YoutubeApiKey = YoutubeApiKey;
            this.DownloadPath = DownloadPath;
        }

        public string YoutubeApiKey { get; set; }
        public string DownloadPath { get; set; }
    }
}
