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

        public async void getSongList()
        {
            /*
             * Call this in the constructor. And change the name please.
             */
            StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            StorageFile songsFile = await localFolder.GetFileAsync("songs.json");

            songsFile.
        }

        public async void Save()
        {
            StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            StorageFile songsFile = await localFolder.CreateFileAsync("songs.json", CreationCollisionOption.OpenIfExists);

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

                /*
                 * Populate a JSON array with the contents of your song's Artists
                 * array. Assign it to jsonObject["Artist"].
                 */
                jsonObject["Artist"] = JsonValue.CreateStringValue(songInfo.getArtist());
                jsonObject["trackNo"] = JsonValue.CreateNumberValue(songInfo.getTrackNo());

                /*
                 * Add the JSON object to the JSON array here.
                 */

                /*
                 * You'll write the whole array at once so this line is unnecessary.
                 */
                String jsonString = jsonObject.Stringify();

                /*
                 * Your newline hackery is not necessary if you use a JSON array.
                 */
                String.Concat(jsonString, "\n");

                /*
                 * Again, you should write the whole array at once.
                 */
                //Writing to songs.json
                await Windows.Storage.FileIO.WriteTextAsync(songsFile, jsonString);

            }

            /*
             * Write the JSON array to the file here.
             */

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

                    /*
                     * Why are you doing this? Just have an array of strings in your song data.
                     */
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
