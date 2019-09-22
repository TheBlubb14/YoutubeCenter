using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
