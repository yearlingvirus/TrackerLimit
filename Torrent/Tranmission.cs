using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transmission.API.RPC.Entity;
using Transmission.API.RPC;
using TrackerLimit.Entitys;
using System.Diagnostics;
using TrackerLimit.Utils;
using YamlDotNet.Serialization;
using Transmission.API.RPC.Arguments;

namespace TrackerLimit.Torrent
{
    public class Tranmission : TorrentClient
    {
        private Client _client;

        public Client Client
        {
            get
            {
                _client ??= new Client(Url, UserName, Password);
                return _client;
            }
            private set
            {
                _client = value;
            }
        }

        private ISerializer _yamlSerializer = new SerializerBuilder().Build();


        public Tranmission() : base() => Initialize();

        public Tranmission(string url, string userName, string password) : base(url, userName, password) => Initialize();

        public override void Limit()
        {
            var fields = new string[] {
                "id",
                "name",
                "status",
                "totalSize",
                "trackers",
                "uploadLimit",
                "uploadLimited"
            };


            var torrentMaps = GetMatchedTorrentMaps();

            if (torrentMaps == null || torrentMaps.Count <= 0 || !torrentMaps.Any(x => x.Value.Count > 0))
            {
                ToolUtils.PrintTextWithTopDash("No matched seed data found, please adjust the configuration and continue");
                return;
            }

            ToolUtils.PrintTextWithTopDash("Please confirm if you wish to continue? (y/n)");

            var input = ToolUtils.ReadConsoleLine();

            if (input != null && input.Equals("y", StringComparison.OrdinalIgnoreCase))
            {
                // 分析每个种子的Tracker信息，并根据配置应用限速规则
                foreach (var torrentMap in torrentMaps)
                {
                    var tracker = Trackers.FirstOrDefault(x => x.SiteName == torrentMap.Key);
                    if (tracker == null)
                    {
                        ToolUtils.PrintTextWithTopDash($"No Tracker configuration found for SiteName:{torrentMap.Key}, please check");
                    }
                    else
                    {

                        var ids = torrentMap.Value.Select(x => (object)x.ID).ToArray();

                        Client.TorrentSet(new TorrentSettings { IDs = ids, UploadLimit = tracker.UploadLimit, UploadLimited = tracker.UploadLimited });

                        ToolUtils.PrintConsoleLine($"SiteName:{torrentMap.Key}, number of seeds: {ids.Length}, settings applied");
                    }
                }
            }
        }

        private Dictionary<string, List<TorrentInfo>> GetMatchedTorrentMaps()
        {
            var fields = new string[] {
                "id",
                "name",
                "status",
                "totalSize",
                "trackers",
                "uploadLimit",
                "uploadLimited"
            };

            // 获取所有种子信息
            var torrents = Client.TorrentGet(fields).Torrents;

            ToolUtils.PrintTextWithTopDash($"Number of torrents: {torrents.Length}");

            ToolUtils.PrintTextWithTopDash(() =>
            {
                ToolUtils.PrintConsoleLine("Current configuration is:");

                foreach (var tracker in Trackers)
                {
                    ToolUtils.PrintConsoleLine(_yamlSerializer.Serialize(tracker));
                }

            });

            // 创建字典来存储匹配到的种子，按siteName分组
            var torrentMaps = new Dictionary<string, List<TorrentInfo>>();

            // 遍历所有种子
            foreach (var torrent in torrents)
            {
                // 检查种子的每个追踪器是否匹配配置中的任一SiteName或Host
                foreach (var tracker in torrent.Trackers)
                {
                    if (string.IsNullOrWhiteSpace(tracker.announce))
                    {
                        continue;
                    }

                    var siteName = ToolUtils.ExtractSiteName(tracker.announce);

                    bool isMatched = Trackers.Any(x => x.SiteName.Equals(siteName, StringComparison.OrdinalIgnoreCase)) ||
                                     Trackers.Any(x => tracker.announce.Contains(x.Host));

                    if (isMatched)
                    {
                        // 如果匹配，检查字典中是否已存在该siteName的键
                        if (!torrentMaps.TryGetValue(siteName, out List<TorrentInfo>? value))
                        {
                            value = [];
                            torrentMaps[siteName] = value;
                        }

                        value.Add(torrent);
                        break; // 一个种子匹配到即可，无需检查其他追踪器
                    }
                }
            }

            ToolUtils.PrintTextWithTopDash(() =>
            {
                ToolUtils.PrintConsoleLine("Matching seed data\r\n");

                foreach (var torrentMap in torrentMaps)
                {
                    ToolUtils.PrintConsoleLine($"SiteName: {torrentMap.Key}, count: {torrentMap.Value.Count}");
                }
            });

            return torrentMaps;
        }

        private void Initialize()
        {
            ClientType = Enums.TorrentClientType.Tranmission;
        }
    }
}
