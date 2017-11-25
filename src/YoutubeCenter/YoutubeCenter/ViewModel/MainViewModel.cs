using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using YoutubeCenter.Library;
using YoutubeCenter.Library.Database;
using YoutubeCenter.Library.Messaging;
using YoutubeCenter.Library.Model;
using YoutubeCenter.Service;

namespace YoutubeCenter.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// </summary>
    public class MainViewModel : ViewModelBase, IDisposable
    {
        public Channel SelectedChannel { get; set; }

        public string AddChannelText { get; set; }

        public ObservableCollection<Channel> Channels { get; private set; }

        private YoutubeApi YoutubeApi;

        #region Event Commands
        public ICommand NavListBoxSelectionChangedCommand { get; private set; }
        public ICommand KeyDownCommand { get; private set; }
        #endregion

        #region MenuItem Commands
        public ICommand MenuItemExitCommand { get; private set; }
        public ICommand MenuItemSettingsCommand { get; private set; }
        #endregion

        public ICommand AddChannelKeyDownCommand { get; set; }

        public ISnackbarMessageQueue SnackbarMessageQueue { get; set; }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            if (IsInDesignMode)
            {
                //Channels = Mockup.GetDummyChannels();
            }
            else
            {
                NavListBoxSelectionChangedCommand = new RelayCommand(NavListBoxSelectionChanged);
                MenuItemSettingsCommand = new RelayCommand(OpenSettings);
                MenuItemExitCommand = new RelayCommand(ShutdownService.RequestShutdown);
                KeyDownCommand = new RelayCommand<KeyEventArgs>(KeyDown);
                AddChannelKeyDownCommand = new RelayCommand<KeyEventArgs>(AddChannelKeyDown);

                Channels = new ObservableCollection<Channel>();
                YoutubeApi = new YoutubeApi(Settings.Instance.YoutubeApiKey);
                LoadChannels();
            }
        }

        private void LoadChannels()
        {
            //result.Add(await YoutubeApi.GetChannelByNameAsync(channelNames[0]));
            //Database.Instance.SaveChannels(result);


            //var channels = await YoutubeApi.GetChannelsByName(Database.Instance.GetChannels().First().Name);

            foreach (var item in Database.Instance.GetChannels())
                Channels.Add(item);
        }

        public async void AddChannelKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                AddChannelText = "";

            if (e.Key == Key.Enter)
            {
                var channelName = AddChannelText;
                AddChannelText = "";

                var channel = await YoutubeApi.GetChannelByNameAsync(channelName);
                if (channel != null)
                {
                    if (await Database.Instance.SaveChannelAsync(channel))
                    {
                        Channels.Add(channel);
                        SnackbarMessageQueue.Enqueue($"added {channel.Title}");
                    }
                    else
                    {
                        SnackbarMessageQueue.Enqueue($"{channel.Title} already added");
                    }
                }
                else
                {
                    SnackbarMessageQueue.Enqueue($"cant find {channelName}");
                }
            }
        }

        private async void AddChannels(List<string> names)
        {
            return;
            names = new List<string>();
            names.Add("docm77");
            names.Add("ethoslab");
            names.Add("summonersinn");
            names.Add("letsreadsmallbooks");

            var channels = await YoutubeApi.GetChannelsByNameAsync(names.ToArray());
            Database.Instance.SaveChannels(channels);
        }

        private async void OpenSettings()
        {
            // TODO: prevent multiple opening
            using (var model = SimpleIoc.Default.GetInstance<SettingsViewModel>())
            using (var settings = new SettingsControl() { DataContext = model })
            {
                await DialogHost.Show(settings, new DialogClosingEventHandler((obj, args) =>
                {
                    if (args.Parameter.ToString() == "1")
                    {
                        Settings.Instance = model.Settings;
                        Settings.Safe();
                    }
                }));
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

        public void NavListBoxSelectionChanged()
        {
            // Load Channel?
            var a = SelectedChannel;
        }

        public void Dispose()
        {
            Messenger.Default.Unregister<MessageBase>(this);
        }
    }
}