using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json;
using System;
using System.Windows.Forms;
using System.Windows.Input;
using YoutubeCenter.Library.Model;

namespace YoutubeCenter.ViewModel
{
    public sealed class SettingsViewModel : ViewModelBase, IDisposable
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
            using var dialog = new SaveFileDialog
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
            using var dialog = new FolderBrowserDialog
            {
                RootFolder = Environment.SpecialFolder.MyComputer
            };

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
