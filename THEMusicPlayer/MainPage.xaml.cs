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
            //TODO: Populate the collectionStackPanel
        }

	}
}
