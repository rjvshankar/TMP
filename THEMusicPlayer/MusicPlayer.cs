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
using Windows.Storage;

namespace THEMusicPlayer
{
    public class MusicPlayer
    {
        private MediaElement mediaElement_;
        private SystemMediaTransportControls mediaControls_;
        private List<SongData> nowPlayingList_ = new List<SongData>();
        private int currentTrackIndex_;
        private CoreDispatcher dispatcher_;

        public List<SongData> NowPlaying
        {
            get
            {
                return nowPlayingList_;
            }
        }

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
            mediaControls_.IsNextEnabled = true;
            mediaControls_.IsPreviousEnabled = true;
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
                case SystemMediaTransportControlsButton.Next:
                    IncrementTrack();
                    break;
                case SystemMediaTransportControlsButton.Previous:
                    DecrementTrack();
                    break;
                default:
                    break;
            }
        }

        private void mediaElement__MediaEnded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            IncrementTrack();
        }

        private void DecrementTrack()
        {
            currentTrackIndex_--;
            if (currentTrackIndex_ >= 0)
            {
                UpdateMediaSource();
                PlayMedia();
            }
            else
            {
                currentTrackIndex_ = 0;
                UpdateMediaSource();
                PlayMedia();
            }
        }

        public void PlayTrack(int index)
        {
            if (index < 0)
            {
                currentTrackIndex_ = 0;
            }
            else if (index >= nowPlayingList_.Count)
            {
                currentTrackIndex_ = nowPlayingList_.Count - 1;
            }
            else
            {
                currentTrackIndex_ = index;
            }

            UpdateMediaSource();
            PlayMedia();
        }

        private void IncrementTrack()
        {
            currentTrackIndex_++;
            if (currentTrackIndex_ < nowPlayingList_.Count)
            {
                UpdateMediaSource();
                PlayMedia();
            }
            else
            {
                currentTrackIndex_ = nowPlayingList_.Count - 1;
                UpdateMediaSource();
                PlayMedia();
            }
        }

        public delegate void SongChangedEventHandler(object sender, SongData newSong);
        public event SongChangedEventHandler SongChanged;

        private async void UpdateMediaSource()
        {
            StopMedia();
            var file = await StorageFile.GetFileFromPathAsync(
                nowPlayingList_[currentTrackIndex_].Path);
            var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
            await dispatcher_.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                mediaElement_.SetSource(stream, file.ContentType);
            });
            await mediaControls_.DisplayUpdater.CopyFromFileAsync(MediaPlaybackType.Music, file);
            mediaControls_.DisplayUpdater.Update();
            await dispatcher_.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                SongChanged(this, nowPlayingList_[currentTrackIndex_]);
            });
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

        public delegate void PlaylistChangedEventHandler(object sender);
        public event PlaylistChangedEventHandler PlaylistChanged;

        public void EnqueueTrack(SongData song)
        {
            nowPlayingList_.Add(song);
            if (nowPlayingList_.Count == 1)
            {
                currentTrackIndex_ = 0;
                UpdateMediaSource();
            }
            PlaylistChanged(this);
        }

        public void RemoveTrackAtIndex(int index)
        {
            if (index == currentTrackIndex_)
            {
                StopMedia();
                nowPlayingList_.RemoveAt(index);
                UpdateMediaSource();
            }
            else if (index < currentTrackIndex_)
            {
                nowPlayingList_.RemoveAt(index);
                currentTrackIndex_--;
            }
            else if (index > currentTrackIndex_)
            {
                nowPlayingList_.RemoveAt(index);
            }
            PlaylistChanged(this);
        }
    }
}