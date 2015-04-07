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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace THEMusicPlayer
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
	public sealed partial class MainPage : Page
	{
		MusicPlayer musicPlayer;

		public MainPage()
		{
			this.InitializeComponent();
		}
		private void Page_Loaded(object sender, RoutedEventArgs e)
		{
			musicPlayer = new MusicPlayer(mediaElement, this.Dispatcher);
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			const string testfile = @"C:\Users\jysan_000\Music\[2014] The Black Market\04 - The Black Market.mp3";
			musicPlayer.EnqueueTrack(testfile);
			/*
			musicPlayer.PlayMedia();
			await Task.Delay(TimeSpan.FromSeconds(5));
			musicPlayer.PauseMedia();
			await Task.Delay(TimeSpan.FromSeconds(2));
			musicPlayer.PlayMedia();
			await Task.Delay(TimeSpan.FromSeconds(3));
			musicPlayer.StopMedia();
			musicPlayer.PlayMedia();
			await Task.Delay(TimeSpan.FromSeconds(2));
			musicPlayer.StopMedia();
			 */
		}

	}
}
