using Dapper;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YoutubeCenter.Library.Model;
using SQLitePCL;

namespace YoutubeCenter.Library.Database
{
    public sealed class Database : IDisposable
    {
        public static readonly Database Instance = new Database();

        public bool IsDisposed { get; private set; }

        private SqliteConnection Connection;

        static Database()
        {
            Batteries.Init();
        }

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

            // Create Table
            Connection.Execute("CREATE TABLE IF NOT EXISTS \"Channel\" ( `SysNumber` INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, `ID` TEXT, `Title` TEXT, `Description` TEXT, `BackgroundImageUrl` TEXT, `UploadsPlaylistId` TEXT )");
        }

        public void Dispose()
        {
            Connection.Dispose();
            IsDisposed = true;
        }

        public IEnumerable<Channel> GetChannels()
        {
            return Connection?.Query<Channel>("SELECT * FROM Channel") ?? Enumerable.Empty<Channel>();
        }

        public Channel GetChannelByID(string id)
        {
            return Connection?.QueryFirstOrDefault<Channel>("SELECT * FROM Channel WHERE ID = @ID",
                new { ID = id });
        }

        public async Task<bool> SaveChannelAsync(Channel channel)
        {
            // Channel already saved
            if (GetChannelByID(channel.Id) != null)
                return false;

            await Connection.ExecuteAsync("INSERT INTO Channel " +
                     "(ID, Title, Description, BackgroundImageUrl, UploadsPlaylistId) VALUES " +
                     "(@ID, @Title, @Description, @BackgroundImageUrl, @UploadsPlaylistId) " +
                     "EXCEPT SELECT ID, Title, Description, BackgroundImageUrl, UploadsPlaylistId FROM Channel",
                     new
                     {
                         ID = channel.Id ?? "",
                         Title = channel.Title ?? "",
                         Description = channel.Description ?? "",
                         BackgroundImageUrl = channel.BackgroundImageUrl ?? "",
                         UploadsPlaylistId = channel.UploadsPlaylistId ?? ""
                     }).ConfigureAwait(false);

            return true;
        }

        public async void SaveChannels(ICollection<Channel> channels)
        {
            foreach (var channel in channels)
            {
                if (channel == null)
                    continue;

                await SaveChannelAsync(channel).ConfigureAwait(false);
            }
        }
    }
}
