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
        public static ObservableCollection<Channel> GetDummyChannels()
        {
            return new ObservableCollection<Channel>()
            {
                new Channel {Name = "Channel 1"},
                new Channel {Name = "Channel 2"},
                new Channel {Name = "Channel 3"},
                new Channel {Name = "Channel 4"},
                new Channel {Name = "Channel 5"},
            };
        }
    }
}
