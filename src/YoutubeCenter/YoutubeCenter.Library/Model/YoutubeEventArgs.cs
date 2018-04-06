using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
