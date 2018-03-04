using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeCenter.Library.Model;

namespace YoutubeCenter.Library
{
    public static class Mockup
    {
        public static ObservableCollection<Channel> GetDummyChannels(string ApiKey)
        {
            return null;
            //return new ObservableCollection<Channel>()
            //{
            //    new Channel {Name = "Channel 1"},
            //    new Channel {Name = "Channel 2"},
            //    new Channel {Name = "Channel 3"},
            //    new Channel {Name = "Channel 4"},
            //    new Channel {Name = "Channel 5"},
            //};

            //var api = new YoutubeApi(ApiKey);
            ////var result = new ObservableCollection<Channel>();
            ////result.Add(api.GetChannel().Result);
            ////return result;

            //var channelNames = new string[] { "EthosLab", "docm77", "greatscottlab", "Letsreadsmallbooks", "schmoyoho", "TeslaMotors", "marquesbrownlee", "RiotGamesInc", "corycotton", "msadaghd", "xIVERTiiGOIx" };

            //int counter = 0;
            //foreach (var channelName in channelNames)
            //{
            //    counter++;
            //    var channels = api.GetChannelsByNameAsync(channelName).Result;
            //    var a = 1;
            //    //using (var client = new WebClient())
            //    //{
            //    //    var bytes = client.DownloadData(channels.First().Snippet.Thumbnails.Default__.Url);
            //    //    worker.ReportProgress(100 / channelNames.Length * counter, (channels.First().BrandingSettings.Channel.Title, Extension.LoadImage(bytes), channels.First()));
            //    //}
            //}

            //return null;
        }
    }
}
