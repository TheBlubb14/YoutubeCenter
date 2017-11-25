using Dapper;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using YoutubeCenter.Library.Model;


namespace YoutubeCenter.Library.Database
{
    public class Database : IDisposable
    {
        public static readonly Database Instance = new Database();

        public bool IsDisposed { get; private set; }

        private SqliteConnection Connection;

        private Database()
        {
            Settings.Instance.PropertyChanged += this.Settings_PropertyChanged;
            Load();
        }

        private void Settings_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Settings.DatabaseLocation))
                Load();
        }

        private void Load()
        {
            if (String.IsNullOrEmpty(Settings.Instance.DatabaseLocation))
                return;

            Connection?.Dispose();

            var builder = new SqliteConnectionStringBuilder
            {
                DataSource = Settings.Instance.DatabaseLocation
            };
            Connection = new SqliteConnection(builder.ConnectionString);
        }

        public void Dispose()
        {
            Connection.Dispose();
            IsDisposed = true;
        }

        public IEnumerable<Channel> GetChannels()
        {
            return Connection?.Query<Channel>("SELECT ID, Title, Description, BackgroundImageUrl FROM Channel");
        }

        public Channel GetChannelByID(string id)
        {
            return Connection?.QueryFirst<Channel>("SELECT ID, Title, Description, BackgroundImageUrl FROM Channel WHERE ID = @ID",
                new { ID = id });
        }

        public async Task<bool> SaveChannelAsync(Channel channel)
        {
            // Channel already saved
            if (GetChannelByID(channel.Id) != null)
                return false;

            await Connection.ExecuteAsync("INSERT INTO Channel " +
                     "(ID, Title, Description, BackgroundImageUrl) VALUES " +
                     "(@ID, @Title, @Description, @BackgroundImageUrl) " +
                     "EXCEPT SELECT ID, Title, Description, BackgroundImageUrl FROM Channel",
                     new
                     {
                         ID = channel.Id ?? "",
                         Title = channel.Title ?? "",
                         Description = channel.Description ?? "",
                         BackgroundImageUrl = channel.BackgroundImageUrl ?? ""
                     });

            return true;
        }

        public async void SaveChannels(ICollection<Channel> channels)
        {
            foreach (var channel in channels)
            {
                if (channel == null)
                    continue;

                await SaveChannelAsync(channel);
            }
        }
    }
}
