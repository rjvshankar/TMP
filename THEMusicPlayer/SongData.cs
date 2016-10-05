using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.FileProperties;

namespace THEMusicPlayer
{
    public struct SongData
    {
        public readonly string Path;
        public readonly string Title;
        public readonly string Album;
        public readonly string Artist;
        public readonly uint TrackNumber;

        public SongData(MusicProperties props, string path)
        {
            Path = path;
            Title = props.Title;
            Album = props.Album;
            Artist = props.Artist;
            TrackNumber = props.TrackNumber;
        }

        public SongData(string path, string title, string album, string artist, uint track)
        {
            Path = path;
            Title = title;
            Album = album;
            Artist = artist;
            TrackNumber = track;
        }
    }
}