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

        public List<SongData> SongList
        {
            get { return songList_; }
        }

        public static async Task<Collection> BuildCollectionAsync()
        {
            Collection ret = new Collection();
            var localFolder = ApplicationData.Current.LocalFolder;
            var item = await localFolder.TryGetItemAsync("songs.json");

            if (item == null)
            {
                await ret.Refresh();
            }
            else
            {
                ret.songList_ = await RetrieveSongList();
            }

            return ret;
        }

        private static async Task<List<SongData>> RetrieveSongList()
        {
            var localFolder = ApplicationData.Current.LocalFolder;
            var songsFile = await localFolder.GetFileAsync("songs.json");

            var jsonValues = JsonArray.Parse(await FileIO.ReadTextAsync(songsFile));

            var ret = new List<SongData>();
            foreach (var jsonValue in jsonValues)
            {
                var jsonObject = jsonValue.GetObject();

                ret.Add(new SongData(
                    jsonObject.GetNamedString("Path"),
                    jsonObject.GetNamedString("Title"),
                    jsonObject.GetNamedString("Album"),
                    jsonObject.GetNamedString("Artist"),
                    (uint)jsonObject.GetNamedNumber("TrackNumber")
                    ));
            }

            return ret;
        }

        public async void Save()
        {
            StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            StorageFile songsFile = await localFolder.CreateFileAsync("songs.json", CreationCollisionOption.OpenIfExists);

            JsonArray mainArray = new JsonArray();
            foreach (SongData songInfo in SongList)
            {
                JsonObject jsonObject = new JsonObject();
                jsonObject["Path"] = JsonValue.CreateStringValue(songInfo.Path);
                jsonObject["Title"] = JsonValue.CreateStringValue(songInfo.Title);
                jsonObject["Album"] = JsonValue.CreateStringValue(songInfo.Album);
                jsonObject["Artist"] = JsonValue.CreateStringValue(songInfo.Artist);
                jsonObject["TrackNumber"] = JsonValue.CreateNumberValue(songInfo.TrackNumber);

                mainArray.Add(jsonObject);
            }

            await FileIO.WriteTextAsync(songsFile, mainArray.Stringify());
        }

        public async Task Refresh()
        {
            StorageFolder rootDirectory = KnownFolders.MusicLibrary;
            await processFolder(rootDirectory);
        }

        private async Task processFolder(StorageFolder folder)
        {
            var items = await folder.GetItemsAsync();

            foreach (var item in items)
            {
                if (item is StorageFolder)
                {
                    //recursive call to process sub-folders
                    var subFolder = item as StorageFolder;
                    await processFolder(subFolder);
                }
                else
                {
                    //get song details
                    var songFile = item as StorageFile;

                    var musicProp = await songFile.Properties.GetMusicPropertiesAsync();

                    if (musicProp == null)
                    {
                        continue;
                    }

                    songList_.Add(new SongData(musicProp, songFile.Path));
                }
            }
        }
    }
}