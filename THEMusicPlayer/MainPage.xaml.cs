using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Diagnostics;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.UI.Xaml.Media.Imaging;

namespace THEMusicPlayer
{
    public sealed partial class MainPage : Page
    {
        MusicPlayer musicPlayer;
        Collection collection;

        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            musicPlayer = new MusicPlayer(mediaElement, this.Dispatcher);
            musicPlayer.SongChanged += musicPlayer_SongChanged;
            musicPlayer.PlaylistChanged += musicPlayer_PlaylistChanged;

            //Populate the collection list.
            collection = await Collection.BuildCollectionAsync();
            foreach (var song in collection.SongList)
            {
                if (song.Title != "")
                {
                    collectionStackPanel.Children.Add(BuildSongBox(song));
                }
            }

            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                collection.Save();
            });
        }

        void musicPlayer_PlaylistChanged(object sender)
        {
            playlistStackPanel.Children.Clear();
            for (int i = 0; i < musicPlayer.NowPlaying.Count; i++)
            {
                int index = i;
                playlistStackPanel.Children.Add(BuildPlaylistBox(musicPlayer.NowPlaying[index], index));
            }
        }

        private async void musicPlayer_SongChanged(object sender, SongData newSong)
        {
            var file = await StorageFile.GetFileFromPathAsync(newSong.Path);
            //using (StorageItemThumbnail thumbnail = await file.GetThumbnailAsync(ThumbnailMode.MusicView, 200))
            //{
            //    if (thumbnail != null && thumbnail.Type == ThumbnailType.Image)
            //    {
            //        var bitmap = new BitmapImage();
            //        bitmap.SetSource(thumbnail);
            //        albumArtworkImage.Source = bitmap;
            //    }
            //    else
            //    {
            //        albumArtworkImage.Source = null;
            //    }
            //}

            titleTextBlock.Text = newSong.Title;
            artistTextBlock.Text = newSong.Artist;
            albumTextBlock.Text = newSong.Album;
            lyricsTextBlock.Text = await Lyrics.Get(newSong.Artist, newSong.Title);
        }

        private TextBlock BuildSongBox(SongData song)
        {
            var block = new TextBlock();
            block.Text = song.Title;
            block.DoubleTapped += (sender, e) =>
            {
                musicPlayer.EnqueueTrack(song);
            };
            return block;
        }

        private TextBlock BuildPlaylistBox(SongData song, int index)
        {
            var block = new TextBlock();
            block.Text = song.Title;
            block.DoubleTapped += (sender, e) =>
            {
                musicPlayer.PlayTrack(index);
            };
            return block;
        }
    }
}