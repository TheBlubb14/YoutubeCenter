using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<IList<Model.Channel>> GetChannelsByName(params string[] Names)
        {
            var result = new List<Model.Channel>();
            foreach (var name in Names)
                result.Add(await GetChannelByNameAsync(name));

            return result;
        }

        public async Task<Model.Channel> GetChannelByNameAsync(string Name)
        {
            var search = _service.Channels.List("brandingSettings,snippet");
            search.ForUsername = Name;

            var result = await search.ExecuteAsync().ConfigureAwait(false);

            if (result?.Items.Count < 1)
                return null;

            var item = result.Items[0];

            return new Model.Channel()
            {
                Id = item?.Id,
                Title = item?.Snippet?.Title,
                Name = Name,
                Description = item?.Snippet?.Description,
                BackgroundImageUrl = item?.BrandingSettings?.Image?.BackgroundImageUrl?.Default__
            };
        }
    }
}
