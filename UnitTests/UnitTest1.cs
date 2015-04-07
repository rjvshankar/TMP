using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework.AppContainer;
using Windows.UI.Core;
using THEMusicPlayer;

namespace UnitTests
{
    [TestClass]
    public class UnitTest1
    {
		/// <summary>
		/// This tests whether the Music Player works or not. It is limited to
		/// just testing whether the methods work/don't throw exceptions unexpectedly,
		/// since audio needs to be manually tested.
		/// </summary>
		[UITestMethod]
		public void PlaybackTest()
		{
			const string testfile = @"C:\Users\jysan_000\Music\[2014] The Black Market\04 - The Black Market.mp3";
			var mediaElement = new Windows.UI.Xaml.Controls.MediaElement();
			var dispatcher = CoreWindow.GetForCurrentThread().Dispatcher;
			var musicPlayer = new THEMusicPlayer.MusicPlayer(mediaElement, dispatcher);
			musicPlayer.EnqueueTrack(testfile);
			musicPlayer.PlayMedia();
			Task.Delay(TimeSpan.FromSeconds(5)).Wait();
			musicPlayer.PauseMedia();
			Task.Delay(TimeSpan.FromSeconds(2)).Wait();
			musicPlayer.PlayMedia();
			Task.Delay(TimeSpan.FromSeconds(3)).Wait();
			musicPlayer.StopMedia();
			musicPlayer.PlayMedia();
			Task.Delay(TimeSpan.FromSeconds(2)).Wait();
			musicPlayer.StopMedia();
		}
    }
}
