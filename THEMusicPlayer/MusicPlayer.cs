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
		private List<Uri> nowPlayingList = new List<Uri>();
		private int currentTrackIndex;
		private MediaElement mediaElement;

		public MusicPlayer(MediaElement medElement)
		{
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
			currentTrackIndex++;
			if (currentTrackIndex < nowPlayingList.Count)
			{
				UpdateMediaSource();
				PlayMedia();
			}
		}

		private void UpdateMediaSource()
		{
			mediaElement_.Source = nowPlayingList[currentTrackIndex];
		}

		public async void PlayMedia()
		{
			await CoreWindow.GetForCurrentThread().Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
				{
					mediaElement_.Play();
				});
		}

		public async void PauseMedia()
		{
			await CoreWindow.GetForCurrentThread().Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
				{
					mediaElement_.Play();
				});
		}

		public async void StopMedia()
		{
			await CoreWindow.GetForCurrentThread().Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
				{
					mediaElement_.Stop();
				});
		}

		public void EnqueueTrack(Uri uri)
		{
			nowPlayingList.Add(uri);
			if (nowPlayingList.Count == 1)
			{
				currentTrackIndex = 0;
				UpdateMediaSource();
			}
			else
			{
				currentTrackIndex++;
			}
		}

		public void RemoveTrackAtIndex(int index)
		{
			if (index == currentTrackIndex)
			{
				StopMedia();
				nowPlayingList.RemoveAt(index);
				UpdateMediaSource();
			}
			else if (index < currentTrackIndex)
			{
				nowPlayingList.RemoveAt(index);
				currentTrackIndex--;
			}
			else if (index > currentTrackIndex)
			{
				nowPlayingList.RemoveAt(index);
			}
		}
	}
}
