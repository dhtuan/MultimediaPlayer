using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.IO;

namespace MiniProject_MusicPlayer
{
    /// <summary>
    /// Interaction logic for MyMusicPage.xaml
    /// </summary>
    public partial class MyMusicPage : UserControl
    {
		public static string selectedPlayListName = null;

        public MyMusicPage()
        {
            InitializeComponent();

            //if (File.Exists("library.dat"))
            //{
            //    string song = "";
            //    ImageSource Cover = null;
            //    string Title = null;
            //    string Artist = null;
            //    string Album = null;

            //    var read = new StreamReader("library.dat");

            //    while ((song = read.ReadLine()) != null)
            //    {
            //        TagLib.File file = TagLib.File.Create(song);

            //        if (file.Tag.Pictures.Length >= 1)
            //        {
            //            TagLib.IPicture pic = file.Tag.Pictures[0];
            //            System.IO.MemoryStream stream = new System.IO.MemoryStream(pic.Data.Data);
            //            BitmapFrame bmp = BitmapFrame.Create(stream);
            //            Cover = bmp;
            //        }
            //        else
            //        {
            //            BitmapImage bi = new BitmapImage();
            //            bi.BeginInit();
            //            bi.UriSource = new Uri("/Icon/disc.png", UriKind.Relative);
            //            bi.EndInit();
            //            Cover = bi;
            //        }

            //        Title = file.Tag.Title;

            //        if (file.Tag.AlbumArtists.Length >= 1)
            //        {
            //            Artist = file.Tag.AlbumArtists[0].ToString();
            //        }
            //        else
            //        {
            //            Artist = "Unknown";
            //        }

            //        Album = file.Tag.Album;

            //        MainWindow._infoList.Add(new Info(Cover, Title, Artist, Album, song));
            //    }

            //    read.Close();

            //    BrowseListView.ItemsSource = MainWindow._infoList;
            //}

            if (MainWindow._infoList.Count == 0)
            {
                browseButton.Visibility = Visibility.Visible;
            }
            else
            {
                browseButton.Visibility = Visibility.Hidden;
                BrowseListView.Visibility = Visibility.Visible;
            }
        }

        void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            ImageSource Cover = null;
            string Title = null;
            string Artist = null;
            string Album = null;

            var screen = new Microsoft.Win32.OpenFileDialog();
            screen.Filter = "Audio File|*.mp3;*.m4a";
            screen.Multiselect = true;

            if (screen.ShowDialog() == true)
            {
                var filenames = screen.FileNames;

                browseButton.Visibility = Visibility.Hidden;
                BrowseListView.Visibility = Visibility.Visible;

                foreach (var filename in filenames)
                {
                    TagLib.File file = TagLib.File.Create(filename);

                    if (file.Tag.Pictures.Length >= 1)
                    {
                        TagLib.IPicture pic = file.Tag.Pictures[0];
                        System.IO.MemoryStream stream = new System.IO.MemoryStream(pic.Data.Data);
                        BitmapFrame bmp = BitmapFrame.Create(stream);
                        Cover = bmp;
                    }
                    else
                    {
                        BitmapImage bi = new BitmapImage();
                        bi.BeginInit();
                        bi.UriSource = new Uri("/Icon/disc.png", UriKind.Relative);
                        bi.EndInit();
                        Cover = bi;
                    }

                    Title = file.Tag.Title;

                    if (file.Tag.AlbumArtists.Length >= 1)
                    {
                        Artist = file.Tag.AlbumArtists[0].ToString();
                    }
                    else
                    {
                        Artist = "Unknown";
                    }

                    Album = file.Tag.Album;

                    MainWindow._infoList.Add(new Info(Cover, Title, Artist, Album, filename));
                }

                BrowseListView.ItemsSource = MainWindow._infoList;
            }
        }

        private void PlayMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var menu = sender as MenuItem;
            var info = menu.DataContext as Info;

			if (MainWindow._timer.IsEnabled == true && info.FileName != MainWindow.currentlyPlayingSong)
			{
				MainWindow._audio.Stop();
				MainWindow._timer.Stop();
			}

			MainWindow.SetNowPlaying(info.FileName);
			MainWindow._audio.Open(new Uri(info.FileName));
			MainWindow._audio.MediaOpened += _audio_MediaOpened;
			MainWindow._audio.MediaEnded += _audio_MediaEnded;

            var writer = new StreamWriter("lastplayed.dat");
            writer.WriteLine(MainWindow.GetNowPlaying());
            writer.Close();

            foreach (var items in MainWindow._playlistList)
            {
                items.Save(false, -1);
            }

            MainWindow._currentlyPlayingPlayList.Clear();
        }

		private void _audio_MediaEnded(object sender, EventArgs e)
		{
			MainWindow._isPlaying = false;
			MainWindow._timer.Stop();
			MainWindow._audio.Close();
            MainWindow._checkEnd.ChangeEnd = true;

            MainWindow.currentlyPlayingSong = null;
        }

        private void _audio_MediaOpened(object sender, EventArgs e)
		{
			MainWindow._audio.Play();
			MainWindow._isPlaying = true;
			MainWindow._timer.Start();
            MainWindow._checkOpen.ChangeOpen = true;
        }

        private void RemoveMenuItem_Click(object sender, RoutedEventArgs e)
		{
			var menu = sender as MenuItem;
			var info = menu.DataContext as Info;

			MainWindow._infoList.Remove(info);
		}

		private void NewPlaylistMenuItem_Click(object sender, RoutedEventArgs e)
		{
			var newplaylist = new AddPlaylistWindow();

			var menu = sender as MenuItem;
			var info = menu.DataContext as Info;

			if (newplaylist.ShowDialog() == true)
			{
				Playlist newPlaylist = new Playlist(newplaylist.ListName, new BindingList<Info>(), false, -1);
				MainWindow._playlistList.Add(newPlaylist);
                MainWindow._check.ChangePlaylist = true;
				newPlaylist.Song.Add(info);
			}
		}

		private void AddToPlaylistMenuItem_Click(object sender, RoutedEventArgs e)
		{
			var screen = new AddMusicToPlaylist();
			var menu = sender as MenuItem;
			var info = menu.DataContext as Info;

			if (screen.ShowDialog() == true)
			{
				foreach(var playlist in MainWindow._playlistList)
				{
					if(playlist.Name == selectedPlayListName)
					{
						playlist.Song.Add(info);
					}
				}
			}
		}

        private void BrowseMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ImageSource Cover = null;
            string Title = null;
            string Artist = null;
            string Album = null;

            var screen = new Microsoft.Win32.OpenFileDialog();
            screen.Filter = "Audio File|*.mp3;*.m4a";
            screen.Multiselect = true;

            if (screen.ShowDialog() == true)
            {
                var filenames = screen.FileNames;

                browseButton.Visibility = Visibility.Hidden;
                BrowseListView.Visibility = Visibility.Visible;

                foreach (var filename in filenames)
                {
                    TagLib.File file = TagLib.File.Create(filename);

                    if (file.Tag.Pictures.Length >= 1)
                    {
                        TagLib.IPicture pic = file.Tag.Pictures[0];
                        System.IO.MemoryStream stream = new System.IO.MemoryStream(pic.Data.Data);
                        BitmapFrame bmp = BitmapFrame.Create(stream);
                        Cover = bmp;
                    }
                    else
                    {
                        BitmapImage bi = new BitmapImage();
                        bi.BeginInit();
                        bi.UriSource = new Uri("/Icon/disc.png", UriKind.Relative);
                        bi.EndInit();
                        Cover = bi;
                    }

                    Title = file.Tag.Title;

                    if (file.Tag.AlbumArtists.Length >= 1)
                    {
                        Artist = file.Tag.AlbumArtists[0].ToString();
                    }
                    else
                    {
                        Artist = "Unknown";
                    }

                    Album = file.Tag.Album;

                    MainWindow._infoList.Add(new Info(Cover, Title, Artist, Album, filename));
                }

                BrowseListView.ItemsSource = MainWindow._infoList;
            }
        }

        private void RemoveAllMenuItem_Click(object sender, RoutedEventArgs e)
        {
            while (MainWindow._infoList.Count != 0)
            {
                MainWindow._infoList.Remove(MainWindow._infoList[0]);
            }

            BrowseListView.Visibility = Visibility.Hidden;
            browseButton.Visibility = Visibility.Visible;
        }
    }
}
