using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeCenter.Library.Messaging;

namespace YoutubeCenter.ViewModel
{
    class SettingsViewModel : ViewModelBase
    {
        public SettingsViewModel()
        {
            if (IsInDesignMode)
            {
            }
            else
            {
                Messenger.Default.Register<MessageBase>(this, x =>
                {
                    if (x.Sender.ToString() == Notifications.NotifyShutdown)
                    {
                        // Save settings
                    }
                });
            }
        }
    }
}
