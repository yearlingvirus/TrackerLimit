using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerLimit.Entitys
{
    [Serializable]
    public class Tracker
    {
        public Tracker()
        {
            SiteName = string.Empty;
            Host = string.Empty;
            UploadLimit = 1000;
        }

        /// <summary>
        /// TrackerName
        /// </summary>
        public string SiteName { get; set; }

        /// <summary>
        /// Host
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// maximum upload speed (KBps)
        /// </summary>
        public int UploadLimit { get; set; }

        /// <summary>
        /// true if uploadLimit is honored
        /// </summary>
        public bool UploadLimited { get; set; }
    }
}
