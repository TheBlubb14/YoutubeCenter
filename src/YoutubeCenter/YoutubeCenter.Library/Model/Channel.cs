using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

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
