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
using System.Windows.Controls;
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

        //public ObservableCollection<Video> Videos { get; private set; }

        public Playlist Playlist { get; set; }

        private YoutubeApi YoutubeApi;
        private ObservableCollection<Exception> StartupExceptions = new ObservableCollection<Exception>();

        #region Event Commands
        public ICommand NavListBoxSelectionChangedCommand { get; private set; }
        public ICommand KeyDownCommand { get; private set; }
        public ICommand ScrollToBottomCommand { get; private set; }
        public ICommand ItemVisibilityChangedCommand { get; set; }
        #endregion

        #region MenuItem Commands
        public ICommand MenuItemExitCommand { get; private set; }
        public ICommand MenuItemSettingsCommand { get; private set; }
        #endregion

        public ICommand AddChannelKeyDownCommand { get; set; }

        public ISnackbarMessageQueue SnackbarMessageQueue { get; set; }

        private bool IsSettingsDialogOpen;

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
                ScrollToBottomCommand = new RelayCommand(ScrollToBottom);
                AddChannelKeyDownCommand = new RelayCommand<KeyEventArgs>(AddChannelKeyDown);
                ItemVisibilityChangedCommand = new RelayCommand(() => { });
                this.PropertyChanged += this.MainViewModel_PropertyChanged;

                Channels = new ObservableCollection<Channel>();
                //Videos = new ObservableCollection<Video>();
                YoutubeApi = new YoutubeApi(Settings.Instance.YoutubeApiKey);
                LoadChannels();
            }
        }

        private void MainViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(SnackbarMessageQueue):
                    ShowStartupExceptions();
                    break;
            }
        }

        private void LoadChannels()
        {
            //result.Add(await YoutubeApi.GetChannelByNameAsync(channelNames[0]));
            //Database.Instance.SaveChannels(result);


            //var channels = await YoutubeApi.GetChannelsByName(Database.Instance.GetChannels().First().Name);
            try
            {
                foreach (var item in Database.Instance.GetChannels())
                    Channels.Add(item);
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        public async void AddChannelKeyDown(KeyEventArgs e)
        {
            if (Debugger.IsAttached)
                if (e.Key == Key.F8)
                {

                }

            if (e.Key == Key.Escape)
                AddChannelText = "";

            if (e.Key == Key.Enter)
            {
                var channelName = AddChannelText;
                AddChannelText = "";

                var result = await YoutubeApi.GetChannelByNameAsync(channelName);

                if (result.Exception == null)
                {
                    if (result.Result != null)
                    {
                        if (await Database.Instance.SaveChannelAsync(result.Result))
                        {
                            Channels.Add(result.Result);
                            SnackbarMessageQueue.Enqueue($"added {result.Result.Title}");
                        }
                        else
                        {
                            SnackbarMessageQueue.Enqueue($"{result.Result.Title} already added");
                        }
                    }
                    else
                    {
                        SnackbarMessageQueue.Enqueue($"cant find {channelName}");
                    }
                }
                else
                {
                    ShowError(result.Exception);
                }
            }
        }

        private void ShowError(Exception exception)
        {
            if (SnackbarMessageQueue != null)
            {
                SnackbarMessageQueue.Enqueue($"error: {exception.Message}", "show more",
                    () => { SnackbarMessageQueue.Enqueue($"{exception.ToString()}"); });
            }
            else
            {
                // if theres an exception at application startup, 
                // the viewmodel loads before the view and SnackbarMessageQueue is not declared yet
                StartupExceptions.Add(exception);
            }
        }

        private void ShowStartupExceptions()
        {
            foreach (var ex in StartupExceptions)
                ShowError(ex);

            StartupExceptions.Clear();
        }

        private async void AddChannels(List<string> names)
        {
            return;
            names = new List<string>();
            names.Add("docm77");
            names.Add("ethoslab");
            names.Add("summonersinn");
            names.Add("letsreadsmallbooks");

            var result = await YoutubeApi.GetChannelsByNameAsync(names.ToArray());

            if (result.Exception == null)
                Database.Instance.SaveChannels(result.Result);
            else
                ShowError(result.Exception);
        }

        private async void OpenSettings()
        {
            if (!IsSettingsDialogOpen)
                using (var model = SimpleIoc.Default.GetInstance<SettingsViewModel>())
                using (var settings = new SettingsControl() { DataContext = model })
                {
                    await DialogHost.Show(settings,
                        new DialogOpenedEventHandler((obj, args) => IsSettingsDialogOpen = true),
                        new DialogClosingEventHandler((obj, args) =>
                        {
                            IsSettingsDialogOpen = false;

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

        public async void NavListBoxSelectionChanged()
        {
            // Load videos
            //var result = await YoutubeApi.GetVideosByChannelAsync(SelectedChannel, 50);
            Playlist = new Playlist(SelectedChannel, YoutubeApi);

            //if (result.Exception == null)
            //{
            //    //Videos.Clear();

            //}
            //else
            //{
            //    ShowError(result.Exception);
            //}
        }

        private void ScrollToBottom()
        {
            Playlist?.LoadMore();
        }

        public void Dispose()
        {
            Messenger.Default.Unregister<MessageBase>(this);
        }
    }
}