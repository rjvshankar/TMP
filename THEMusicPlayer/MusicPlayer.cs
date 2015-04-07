using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.Media;

namespace THEMusicPlayer
{
    public class MusicPlayer
    {
        private MediaElement mediaElement_;
        private SystemMediaTransportControls mediaControls_;
        private List<string> nowPlayingList = new List<string>();
        private int currentTrackIndex_;
        private CoreDispatcher dispatcher_;

        public MusicPlayer(MediaElement medElement, CoreDispatcher disp)
        {
            this.dispatcher_ = disp;
            this.mediaElement_ = medElement;
            //This should be set in XAML, but just to be sure...
            mediaElement_.AudioCategory = AudioCategory.BackgroundCapableMedia;

            mediaElement_.CurrentStateChanged += mediaElement__CurrentStateChanged;
            mediaElement_.MediaEnded += mediaElement__MediaEnded;

            InitializeTransportControls();
        }

        private void InitializeTransportControls()
        {
            mediaControls_ = SystemMediaTransportControls.GetForCurrentView();
            mediaControls_.ButtonPressed += mediaControls__ButtonPressed;

            mediaControls_.IsPlayEnabled = true;
            mediaControls_.IsPauseEnabled = true;
        }

        private void mediaElement__CurrentStateChanged(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            switch (mediaElement_.CurrentState)
            {

                case MediaElementState.Playing:
                    mediaControls_.PlaybackStatus = MediaPlaybackStatus.Playing;
                    break;
                case MediaElementState.Paused:
                    mediaControls_.PlaybackStatus = MediaPlaybackStatus.Paused;
                    break;
                case MediaElementState.Stopped:
                    mediaControls_.PlaybackStatus = MediaPlaybackStatus.Stopped;
                    break;
                case MediaElementState.Closed:
                    mediaControls_.PlaybackStatus = MediaPlaybackStatus.Closed;
                    break;
                default:
                    break;
            }
        }

        private void mediaControls__ButtonPressed(SystemMediaTransportControls sender, SystemMediaTransportControlsButtonPressedEventArgs args)
        {
            switch (args.Button)
            {
                case SystemMediaTransportControlsButton.Play:
                    PlayMedia();
                    break;
                case SystemMediaTransportControlsButton.Pause:
                    PauseMedia();
                    break;
                default:
                    break;
            }
        }			

        private void mediaElement__MediaEnded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            currentTrackIndex_++;
            if (currentTrackIndex_ < nowPlayingList.Count)
            {
                UpdateMediaSource();
                PlayMedia();
            }
        }

        private async void UpdateMediaSource()
        {
            var file = await Windows.Storage.StorageFile.GetFileFromPathAsync(nowPlayingList[currentTrackIndex_]);
            var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
            mediaElement_.SetSource(stream, "audio/x-mpeg-3");
        }

        public async void PlayMedia()
        {
            await dispatcher_.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    mediaElement_.Play();
                });
        }

        public async void PauseMedia()
        {
            await dispatcher_.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    mediaElement_.Pause();
                });
        }

        public async void StopMedia()
        {
            await dispatcher_.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    mediaElement_.Stop();
                });
        }

        public void EnqueueTrack(string filePath)
        {
            nowPlayingList.Add(filePath);
            if (nowPlayingList.Count == 1)
            {
                currentTrackIndex_ = 0;
                UpdateMediaSource();
            }
            else
            {
                currentTrackIndex_++;
            }
        }

        public void RemoveTrackAtIndex(int index)
        {
            if (index == currentTrackIndex_)
            {
                StopMedia();
                nowPlayingList.RemoveAt(index);
                UpdateMediaSource();
            }
            else if (index < currentTrackIndex_)
            {
                nowPlayingList.RemoveAt(index);
                currentTrackIndex_--;
            }
            else if (index > currentTrackIndex_)
            {
                nowPlayingList.RemoveAt(index);
            }
        }
    }
}
