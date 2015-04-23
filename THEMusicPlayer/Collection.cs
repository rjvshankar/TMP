using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace THEMusicPlayer
{

    public class Collection
    {
        private List<SongData> songList_ = new List<SongData>();

        public void getSongList()
        {
        
        }

        public async void Save()
        {
            StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            StorageFile sampleFile = await localFolder.CreateFileAsync("songs.json", CreationCollisionOption.OpenIfExists);


        }

        public void Refresh()
        {
            StorageFolder rootDirectory = KnownFolders.MusicLibrary;
            processFolder(rootDirectory);
        }

        //recursive function that gets all files in the folder
        private async void processFolder(StorageFolder folder)
        {
            var items = await folder.GetItemsAsync();

            foreach (var item in items)
            {
                if (item is StorageFolder)
                {
                    var subFolder = item as StorageFolder;
                    processFolder(subFolder);
                }

                else
                {
                    var songFile = item as StorageFile;
                    TagLib.File file = TagLib.File.Create(songFile.Path);
                    var Artists = file.Tag.AlbumArtists;
                    StringBuilder builder = new StringBuilder();
                    
                    foreach (var artist in Artists)
                    {
                        builder.Append(artist);
                        builder.Append(", ");
                    }

                    String Artist = builder.ToString();

                    songList_.Add(new SongData(songFile.Path, file.Tag.Title, Artist, file.Tag.Album, file.Tag.Track));
                }
            }

        }

    }
}
