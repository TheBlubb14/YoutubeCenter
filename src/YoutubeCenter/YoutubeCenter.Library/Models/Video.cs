using GalaSoft.MvvmLight;
using Google.Apis.YouTube.v3.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoutubeCenter.Library.Model
{
    public class Video : ObservableObject
    {
        public string VideoId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ThumbnailDetails ThumbnailDetails { get; set; }
        public DateTime? PublishedAt { get; set; }

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
        }

        public Video(PlaylistItem playlistItem)
        {
            this.VideoId = playlistItem.ContentDetails?.VideoId;
            this.Title = playlistItem.Snippet?.Title;
            this.Description = playlistItem.Snippet?.Description;
            this.ThumbnailDetails = playlistItem.Snippet?.Thumbnails;
            this.PublishedAt = playlistItem.Snippet?.PublishedAt;
        }
    }
}
