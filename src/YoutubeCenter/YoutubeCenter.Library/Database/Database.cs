using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeCenter.Library.Model;

namespace YoutubeCenter.Library.Database
{
    public class Database :IDisposable
    {
        private Settings Settings;
        private SQLiteConnection Connection;

        public bool IsDisposed { get; private set; }

        public Database(Settings settings)
        {
            this.Settings = settings;
            Settings.PropertyChanged += this.Settings_PropertyChanged;

            Load();
        }

        private void Settings_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Settings.DatabaseLocation))
                Load();
        }

        private void Load()
        {
            if (String.IsNullOrEmpty(Settings.DatabaseLocation))
                return;

            if (!File.Exists(Settings.DatabaseLocation))
                SQLiteConnection.CreateFile(Settings.DatabaseLocation);

            Connection = new SQLiteConnection($"Data Source={Settings.DatabaseLocation};Version=3;");
        }

        public void Dispose()
        {
            Connection.Dispose();
            IsDisposed = true;
        }
    }
}
