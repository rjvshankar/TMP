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

        public async void getSongList()
        {
            StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            StorageFile songsFile = await localFolder.GetFileAsync("songs.json");


        }

        public async void Save()
        {
            StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            StorageFile songsFile = await localFolder.CreateFileAsync("songs.json", CreationCollisionOption.OpenIfExists);

            foreach (SongData songInfo in songList_)
            {
                //Populating jsonObject with songInfo from songList_
                JsonObject jsonObject = new JsonObject();
                jsonObject["Path"] = JsonValue.CreateStringValue(songInfo.getPath());
                jsonObject["Title"] = JsonValue.CreateStringValue(songInfo.getTitle());
                jsonObject["Album"] = JsonValue.CreateStringValue(songInfo.getAlbum());
                jsonObject["Artist"] = JsonValue.CreateStringValue(songInfo.getArtist());
                jsonObject["trackNo"] = JsonValue.CreateNumberValue(songInfo.getTrackNo());
                String jsonString = jsonObject.Stringify();
                String.Concat(jsonString, "\n");

                //Writing to songs.json
                await Windows.Storage.FileIO.WriteTextAsync(songsFile, jsonString);
            }

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
