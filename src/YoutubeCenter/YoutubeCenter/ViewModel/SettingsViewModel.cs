using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MaterialDesignThemes.Wpf;
using Newtonsoft.Json;
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
        public ICommand DatabaseBrowseFileCommand { get; set; }
        public ICommand DownloadPathBrowseFolderCommand { get; set; }

        public ICommand LoadedCommand { get; set; }

        public Settings Settings { get; private set; }

        public SettingsViewModel()
        {
            if (IsInDesignMode)
            {
            }
            else
            {
                DownloadPathBrowseFolderCommand = new RelayCommand(DownloadPathBrowseFolder);
                DatabaseBrowseFileCommand = new RelayCommand(DatabaseBrowseFile);
                LoadedCommand = new RelayCommand(Loaded);
            }
        }

        private void Loaded()
        {
            this.Settings = JsonConvert.DeserializeObject<Settings>(JsonConvert.SerializeObject(Settings.Instance));
        }

        private void DatabaseBrowseFile()
        {
            var dialog = new SaveFileDialog
            {
                AddExtension = true,
                AutoUpgradeEnabled = true,
                DefaultExt = ".sqlite",
                FileName = "YoutubeCenterData.sqlite",
                Filter = "sqlite|*.sqlite|all files|*",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            };

            var result = dialog.ShowDialog(System.Windows.Application.Current.MainWindow.GetIWin32Window());

            if (result == DialogResult.OK)
                this.Settings.DatabaseLocation = dialog.FileName;
        }

        private void DownloadPathBrowseFolder()
        {
            var dialog = new FolderBrowserDialog();
            dialog.RootFolder = Environment.SpecialFolder.MyComputer;

            var result = dialog.ShowDialog(System.Windows.Application.Current.MainWindow.GetIWin32Window());

            if (result == DialogResult.OK)
                this.Settings.DownloadPath = dialog.SelectedPath;
        }

        public void Dispose()
        {
            this.Settings = null;
        }
    }
}
