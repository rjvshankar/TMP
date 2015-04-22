using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace THEMusicPlayer
{
    public class SongData
    {
        public String path;
        public String title;
        public String album;
        public String artist;
        public uint trackNo;

        public SongData()
        {
            path = null;
            title = null;
            album = null;
            artist = null;
            trackNo = 0;
        }

        public SongData(String Path, String Title, String Album, String Artist, uint track)
        {
            path = Path;
            title = Title;
            album = Album;
            artist = Artist;
            trackNo = track;
        }

    }
}
