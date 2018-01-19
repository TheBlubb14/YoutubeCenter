using Google;
using Google.Apis.Requests;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace YoutubeCenter.Library
{
    public class YoutubeApi
    {
        public string ApiKey { get; private set; }

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

        public async Task<(Model.Video Result, Exception Exception)> GetVideoByVideoIdAsync(string videoId)
        {
            try
            {
                var search = _service.Videos.List("snippet");
                search.Id = videoId;

                var result = await search.ExecuteAsync().ConfigureAwait(false);

                if (result?.Items.Count < 1)
                    return (null, null);

                var item = result.Items[0];

                return (new Model.Video(item), null);
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


        public async Task<(List<Model.Video> Result, Exception Exception)> GetVideosByChannelAsync(Model.Channel channel, int count = 1)
        {
            try
            {
                var search = _service.PlaylistItems.List("snippet, contentDetails");
                search.MaxResults = count;
                search.PlaylistId = channel.UploadsPlaylistId;

                var result = await search.ExecuteAsync().ConfigureAwait(false);

                if (result?.Items.Count < 1)
                    return (null, null);

                var item = result.Items[0];
                return (new List<Model.Video>(result.Items.Select(x => new Model.Video(x))), null);
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
    }
}
