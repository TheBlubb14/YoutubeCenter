using System;

namespace YoutubeCenter.Library.Model
{
    public class YoutubeEventArgs : EventArgs
    {
        public Exception Exception { get; set; }

        public YoutubeEventArgs()
        {

        }
    }
}
