using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using YoutubeCenter.Library;
using YoutubeCenter.Library.Messaging;
using YoutubeCenter.Library.Model;

namespace YoutubeCenter.ViewModel
{
    public class SettingsViewModel : ViewModelBase, IDisposable
    {
        public ICommand BrowseFolderCommand { get; set; }

        public Settings Settings { get; set; }

        public SettingsViewModel()
        {
            if (IsInDesignMode)
            {
            }
            else
            {
                BrowseFolderCommand = new RelayCommand(BrowseFolder);

                Messenger.Default.Register<MessageBase>(this, x =>
                {
                    if (x.Sender is Notifications.NotifyShutdown)
                        SettingsHelper.Safe(Settings);
                });
            }
        }

        private void BrowseFolder()
        {
            var dialog = new FolderBrowserDialog();
            var result = dialog.ShowDialog(System.Windows.Application.Current.MainWindow.GetIWin32Window());

            if (result == DialogResult.OK)
                Settings.DownloadPath = dialog.SelectedPath;
        }

        public void Dispose()
        {
            Messenger.Default.Unregister<MessageBase>(this);
        }
    }
}
