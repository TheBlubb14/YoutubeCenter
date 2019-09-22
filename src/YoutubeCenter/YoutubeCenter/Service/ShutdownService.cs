using GalaSoft.MvvmLight.Messaging;
using System.Windows;
using YoutubeCenter.Library.Messaging;

namespace YoutubeCenter.Service
{
    public static class ShutdownService
    {
        public static void RequestShutdown()
        {
            var shouldAbortShutdown = false;

            Messenger.Default.Send(new NotificationMessageAction<bool>(
                Notifications.ConfirmShutdown, null,
                shouldAbort => shouldAbortShutdown |= shouldAbort));

            if (!shouldAbortShutdown)
            {
                Messenger.Default.Send(new MessageBase(Notifications.NotifyShutdown));
                Application.Current.Shutdown();
            }
        }
    }
}
