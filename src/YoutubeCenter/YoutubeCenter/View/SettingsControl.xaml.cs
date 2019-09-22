using System;
using System.Windows.Controls;

namespace YoutubeCenter
{
    /// <summary>
    /// Interaktionslogik für SettingsControl.xaml
    /// </summary>
    public partial class SettingsControl : UserControl, IDisposable
    {
        public SettingsControl()
        {
            InitializeComponent();
        }

        public void Dispose()
        {
            if (this.DataContext is IDisposable disposable)
                disposable.Dispose();
        }
    }
}
