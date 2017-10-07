using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using YoutubeCenter.Library;
using YoutubeCenter.Library.Messaging;
using YoutubeCenter.Library.Model;

namespace YoutubeCenter.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// </summary>
    public class MainViewModel : ViewModelBase, IDisposable
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
                MenuItemSettingsCommand = new RelayCommand(OpenSettings);
                MenuItemExitCommand = new RelayCommand(ShutdownService.RequestShutdown);
                KeyDownCommand = new RelayCommand<KeyEventArgs>(KeyDown);
                Settings = SettingsHelper.Load();
            }
        }

        private void OpenSettings()
        {
            using (var model = SimpleIoc.Default.GetInstance<SettingsViewModel>())
            using (var settings = new SettingsControl() { DataContext = model })
            {
                model.Settings = Settings;
                DialogHost.Show(settings, new DialogClosingEventHandler((obj, args) =>
                {
                    if (args.Parameter.ToString() == "1")
                        SettingsHelper.Safe(model.Settings);
                }));
                Settings = model.Settings;
            }
        }

        private void KeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.E && IsControl)
                MenuItemExitCommand.Execute(null);
            else if (e.Key == Key.X && IsControl)
                MenuItemSettingsCommand.Execute(null);
        }

        private bool IsControl => (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control;

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

        public void Dispose()
        {
            Messenger.Default.Unregister<MessageBase>(this);
        }
    }
}