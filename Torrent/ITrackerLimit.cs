using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerLimit.Torrent
{
    /// <summary>
    /// ITrackerLimit
    /// </summary>
    public interface ITrackerLimit
    {
        /// <summary>
        /// Limit
        /// </summary>
        void Limit();
    }
}
