using GalaSoft.MvvmLight;

namespace YoutubeCenter.Library.Model
{
    public class Channel : ObservableObject
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string BackgroundImageUrl { get; set; }
        public string UploadsPlaylistId { get; set; }
    }
}
