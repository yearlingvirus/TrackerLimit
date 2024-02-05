using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLimit.Entitys;
using TrackerLimit.Enums;

namespace TrackerLimit.Torrent
{
    public abstract class TorrentClient : ITrackerLimit
    {
        public TorrentClient()
        {
            ClientType = TorrentClientType.Tranmission;
            Url = string.Empty;
            UserName = string.Empty;
            Password = string.Empty;
            Trackers = [];
        }

        public TorrentClient(string url, string userName, string password)
        {
            Url = url;
            UserName = userName;
            Password = password;
            Trackers = [];
        }

        /// <summary>
        /// ClientType
        /// </summary>
        public TorrentClientType ClientType { get; set; }

        /// <summary>
        /// Url
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// UserName
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Trackers
        /// </summary>
        public List<Tracker> Trackers { get; set; }

        public abstract void Limit();
    }
}
