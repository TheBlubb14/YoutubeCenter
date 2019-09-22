using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using System;
using System.Collections.ObjectModel;

namespace YoutubeCenter.Library.Model
{
    public class Playlist
    {
        public ObservableCollection<Video> Videos { get; set; }

        public string ID { get; }

        public event EventHandler<YoutubeEventArgs> ErrorOccured;

        private PlaylistItemListResponse latestResponse;
        private readonly YoutubeApi youtubeApi;

        public Playlist(Channel channel, YoutubeApi youtubeApi)
        {
            youtubeApi.NullCheck(nameof(youtubeApi));
            channel.NullCheck(nameof(channel));

            this.youtubeApi = youtubeApi;
            this.ID = channel.UploadsPlaylistId;

            Videos = new ObservableCollection<Video>();

            Load(youtubeApi.CreatePlaylistRequest());
        }

        private async void Load(PlaylistItemsResource.ListRequest request)
        {
            try
            {
                request.PlaylistId = this.ID;

                latestResponse = await youtubeApi.ExecutePlaylistRequest(request);
                AddVideos();
            }
            catch (Exception ex)
            {
                OnErrorOccured(ex);
            }
        }

        private void AddVideos()
        {
            foreach (var video in latestResponse.Items)
                Videos.Add(new Video(video));
        }

        public void LoadMore()
        {
            try
            {
                if (latestResponse == null || string.IsNullOrEmpty(latestResponse.NextPageToken))
                    return;

                var request = youtubeApi.CreatePlaylistRequest();
                request.PageToken = latestResponse.NextPageToken;

                Load(request);
            }
            catch (Exception ex)
            {
                OnErrorOccured(ex);
            }
        }

        private void OnErrorOccured(Exception exception)
        {
            ErrorOccured?.Invoke(this, new YoutubeEventArgs() { Exception = exception });
        }
    }
}
