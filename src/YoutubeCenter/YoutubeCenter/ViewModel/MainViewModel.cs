using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using YoutubeCenter.Library;
using YoutubeCenter.Library.Model;

namespace YoutubeCenter.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        public Settings Settings { get; set; }

        public Channel SelectedChannel { get; set; }

        public ObservableCollection<Channel> Channels { get; private set; }

        public ICommand LoadChannelsCommand { get; private set; }

        #region Event Commands
        public ICommand NavListBoxSelectionChangedCommand { get; private set; }
        public ICommand KeyDownCommand { get; private set; }
        #endregion

        #region MenuItem Commands
        public ICommand MenuItemExitCommand { get; private set; }
        public ICommand MenuItemSettingsCommand { get; private set; }
        #endregion

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            if (IsInDesignMode)
            {
                Channels = Mockup.GetDummyChannels();
            }
            else
            {
                LoadChannelsCommand = new RelayCommand(LoadChannels);
                NavListBoxSelectionChangedCommand = new RelayCommand(NavListBoxSelectionChanged);
                MenuItemExitCommand = new RelayCommand(ShutdownService.RequestShutdown);
                KeyDownCommand = new RelayCommand(KeyDown);
            }
        }

        private void KeyDown()
        {
            throw new NotImplementedException();
        }

        public void Exit()
        {
            Environment.Exit(0);
        }

        public void NavListBoxSelectionChanged()
        {
            // Load Channel?
        }

        public void LoadChannels()
        {
            Channels = Mockup.GetDummyChannels();
            // youtube api load channels
        }
    }
}