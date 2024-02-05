using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLimit.Torrent;

namespace TrackerLimit.Entitys
{
    [Serializable]
    public class AppConfig
    {
        public TorrentClient? TorrentClient { get; set; }
    }
}
