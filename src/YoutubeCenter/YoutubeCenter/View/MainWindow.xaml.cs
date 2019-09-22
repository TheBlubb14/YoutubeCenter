using System;
using System.Windows;
using System.Windows.Controls;
using YoutubeCenter.Library.Model;
using YoutubeCenter.ViewModel;

namespace YoutubeCenter
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            ((MainViewModel)DataContext).SnackbarMessageQueue = MainSnackbar.MessageQueue;
        }

        public void Dispose()
        {
            if (this.DataContext is IDisposable disposable)
                disposable.Dispose();
        }

        private void ItemsControl_CleanUpVirtualizedItem(object sender, CleanUpVirtualizedItemEventArgs e)
        {
            (e.Value as Video)?.Dispose();
        }
    }
}
