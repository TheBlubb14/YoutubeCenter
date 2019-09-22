using Google;
using Google.Apis.Requests;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace YoutubeCenter.Library
{
    public class YoutubeApi
    {
        public string ApiKey { get; }

        private readonly YouTubeService _service;

        public YoutubeApi(string ApiKey)
        {
            this.ApiKey = ApiKey;

            _service = new YouTubeService(new Google.Apis.Services.BaseClientService.Initializer()
            {
                ApplicationName = "YoutubeCenter",
                ApiKey = this.ApiKey
            });
        }

        public async Task<(ICollection<Model.Channel> Result, Exception Exception)> GetChannelsByNameAsync(params string[] Names)
        {
            var result = new List<Model.Channel>();

            try
            {
                foreach (var name in Names)
                {
                    var channel = await GetChannelByNameAsync(name);
                    result.Add(channel.Result);

                    // stops after getting an exception
                    if (channel.Exception != null)
                        return (result, channel.Exception);
                }

                return (result, null);
            }
            catch (GoogleApiException ex) when (ex.Error is RequestError)
            {
                // TODO: handel this error better
                return (null, ex);
            }
            catch (Exception ex)
            {
                return (null, ex);
            }
        }

        public async Task<(Model.Channel Result, Exception Exception)> GetChannelByNameAsync(string Name)
        {
            try
            {
                var search = _service.Channels.List("snippet, contentDetails");
                search.ForUsername = Name;

                var result = await search.ExecuteAsync().ConfigureAwait(false);

                if (result?.Items.Count < 1)
                    return (null, null);

                var item = result.Items[0];

                return (new Model.Channel()
                {
                    Id = item?.Id,
                    Title = item?.Snippet?.Title,
                    Description = item?.Snippet?.Description,
                    BackgroundImageUrl = item?.Snippet?.Thumbnails?.Default__?.Url,
                    UploadsPlaylistId = item?.ContentDetails?.RelatedPlaylists?.Uploads
                }, null);
            }
            catch (GoogleApiException ex) when (ex.Error is RequestError)
            {
                // TODO: handel this error better
                return (null, ex);
            }
            catch (Exception ex)
            {
                return (null, ex);
            }
        }

        public async Task<(VideoListResponse Result, Exception Exception)> GetVideoByVideoIdAsync(string videoId)
        {
            try
            {
                var search = _service.Videos.List("snippet");
                search.Id = videoId;

                var result = await search.ExecuteAsync().ConfigureAwait(false);

                return (result, null);
            }
            catch (Exception ex)
            {
                return (null, ex);
            }
        }

        public async Task<(PlaylistItemListResponse Result, Exception Exception)> GetVideosByChannelAsync(Model.Channel channel, int count = 1)
        {
            try
            {
                if (channel == null)
                    return (null, new NullReferenceException(nameof(channel)));

                var request = CreatePlaylistRequest(count);
                request.PlaylistId = channel.UploadsPlaylistId;

                return (await ExecutePlaylistRequest(request), null);
            }
            catch (Exception ex)
            {
                return (null, ex);
            }
        }

        internal PlaylistItemsResource.ListRequest CreatePlaylistRequest(int itemCount = 50)
        {
            itemCount = itemCount.Clamp(1, 50);

            var request = _service.PlaylistItems.List("snippet, contentDetails");
            request.MaxResults = itemCount;

            return request;
        }

        internal async Task<PlaylistItemListResponse> ExecutePlaylistRequest(PlaylistItemsResource.ListRequest request)
        {
            return await request.ExecuteAsync().ConfigureAwait(false);
        }

        private async Task<(PlaylistItemListResponse Result, Exception Exception)> GetPlaylist(string pageToken, int count = 50)
        {
            if (string.IsNullOrEmpty(pageToken))
                return (null, new NullReferenceException(nameof(pageToken)));

            try
            {
                var request = CreatePlaylistRequest(count);
                request.PageToken = pageToken;

                return (await ExecutePlaylistRequest(request), null);
            }
            catch (Exception ex)
            {
                return (null, ex);
            }
        }

        public async Task<(PlaylistItemListResponse Result, Exception Exception)> GetNextPlaylist(PlaylistItemListResponse playlistItemListResponse, int count = 50)
        {
            return await GetPlaylist(playlistItemListResponse.NextPageToken, count);
        }

        public async Task<(PlaylistItemListResponse Result, Exception Exception)> GetPreviousPlaylist(PlaylistItemListResponse playlistItemListResponse, int count = 50)
        {
            return await GetPlaylist(playlistItemListResponse.PrevPageToken, count);
        }
    }
}
