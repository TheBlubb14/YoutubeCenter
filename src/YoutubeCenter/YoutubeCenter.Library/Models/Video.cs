using GalaSoft.MvvmLight;
using Google.Apis.YouTube.v3.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace YoutubeCenter.Library.Model
{
    public class Video : ObservableObject
    {
        public string VideoId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ThumbnailDetails ThumbnailDetails { get; set; }
        public DateTime? PublishedAt { get; set; }
        public BitmapImage Image { get; set; }



        public Video()
        {

        }

        public Video(Google.Apis.YouTube.v3.Data.Video youtubeVideo)
        {
            this.VideoId = youtubeVideo.Id;
            this.Title = youtubeVideo.Snippet?.Title;
            this.Description = youtubeVideo.Snippet?.Description;
            this.ThumbnailDetails = youtubeVideo.Snippet?.Thumbnails;
            this.PublishedAt = youtubeVideo.Snippet?.PublishedAt;

            DownloadImage(youtubeVideo?.Snippet?.Thumbnails?.Maxres?.Url ?? youtubeVideo?.Snippet?.Thumbnails?.Default__?.Url);
        }

        public Video(PlaylistItem playlistItem)
        {
            this.VideoId = playlistItem.ContentDetails?.VideoId;
            this.Title = playlistItem.Snippet?.Title;
            this.Description = playlistItem.Snippet?.Description;
            this.ThumbnailDetails = playlistItem.Snippet?.Thumbnails;
            this.PublishedAt = playlistItem.Snippet?.PublishedAt;

            DownloadImage(playlistItem?.Snippet?.Thumbnails?.Maxres?.Url ?? playlistItem?.Snippet?.Thumbnails?.Default__?.Url);
        }

        private async void DownloadImage(string url)
        {
            if (string.IsNullOrEmpty(url))
                return;

            using (var client = new WebClient())
            {
                var bytes = await client.DownloadDataTaskAsync(url);
                this.Image = ConvertToBitmapImage(bytes);
            }
        }

        private BitmapImage ConvertToBitmapImage(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0)
                return null;

            var image = new BitmapImage();
            using (var stream = new MemoryStream(imageData))
            {
                stream.Position = 0;
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = stream;
                image.EndInit();
            }

            image.Freeze();
            return image;
        }
    }
}
