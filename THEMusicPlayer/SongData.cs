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

        public SongData()
        {
            path = null;
            title = null;
            album = null;
            artist = null;
        }

        public SongData(String a, String b, String c, String d)
        {
            path = a;
            title = b;
            album = c;
            artist = d;
        }

    }
}
