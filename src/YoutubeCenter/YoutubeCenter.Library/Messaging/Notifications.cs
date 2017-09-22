using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoutubeCenter.Library.Messaging
{
    public static class Notifications
    {
        public static readonly string ConfirmShutdown = Guid.NewGuid().ToString();

        public static readonly string NotifyShutdown = Guid.NewGuid().ToString();
    }
}
