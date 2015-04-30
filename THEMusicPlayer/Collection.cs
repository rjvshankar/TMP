using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Data.Json;

namespace THEMusicPlayer
{

    public class Collection
    {
        private List<SongData> songList_ = new List<SongData>();

        /*
         * Wrap songList_ with a public read-only property called
         * SongList.
         */

        /*
         * Where's the constructor?
         * In the constructor you should populate songList_ from songs.json,
         * or run Refresh() if it doesn't exist.
         */
        public Collection()
        {

        }

        public async void retreiveSongList()
        {
            /*
             * Call this in the constructor. And change the name please.
             */
            StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            StorageFile songsFile = await localFolder.GetFileAsync("songs.json");

            var allTheSongs = await Windows.Storage.FileIO.ReadTextAsync(songsFile);
        }

        public async void Save()
        {
            StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            StorageFile songsFile = await localFolder.CreateFileAsync("songs.json", CreationCollisionOption.OpenIfExists);

            JsonArray allTheSongs = new JsonArray();
            /*
             * Add all the objects to a JSON array and write it out to the file.
             * Look up the JsonArray class and add objects to it using its Add method.
             */

            foreach (SongData songInfo in songList_)
            {
                //Populating jsonObject with songInfo from songList_
                JsonObject jsonObject = new JsonObject();
                jsonObject["Path"] = JsonValue.CreateStringValue(songInfo.getPath());
                jsonObject["Title"] = JsonValue.CreateStringValue(songInfo.getTitle());
                jsonObject["Album"] = JsonValue.CreateStringValue(songInfo.getAlbum());
                jsonObject["Artist"] = JsonValue.CreateStringValue(songInfo.getArtists());
                jsonObject["trackNo"] = JsonValue.CreateNumberValue(songInfo.getTrackNo());
                //Adding jsonObject to the JsonArray
                allTheSongs.Add(jsonObject);
            }

            //Writing JsonArray to songs.json
            await Windows.Storage.FileIO.WriteTextAsync(songsFile, allTheSongs.Stringify());

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
                    //recursive call to process sub-folders
                    var subFolder = item as StorageFolder;
                    processFolder(subFolder);
                }

                else
                {
                    //get song details
                    var songFile = item as StorageFile;
                    var songProperties = await songFile.Properties.GetMusicPropertiesAsync();
                    songList_.Add(new SongData(songFile.Path, songProperties.Title, songProperties.AlbumArtist, songProperties.Album, songProperties.TrackNumber));
                }
            }

        }

    }
}
