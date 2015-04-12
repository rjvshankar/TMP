using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace THEMusicPlayer
{
    public class SongData
    {
        /*
         * You need an array of strings to represent your artists.
         */

        public String path;
        public String title;
        public String album;
        public String artist;
        public uint trackNo;

        public SongData()
        {
            /*
             * You can get rid of this constructor. All these are default values.
             */ 

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

        /*
         * Please get rid of these horrible Java-style getters and replace them with
         * read-only properties.
         */

        public String getPath()
        {
            return path;
        }

        public String getTitle()
        {
            return title;
        }

        public String getAlbum()
        {
            return album;
        }

        public String getArtist()
        {
            return artist;
        }

        public uint getTrackNo()
        {
            return trackNo;
        }
    }
}
