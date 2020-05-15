using MiniProject_MusicPlayer.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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

namespace MiniProject_MusicPlayer
{
    /// <summary>
    /// Interaction logic for NowPlayingPage.xaml
    /// </summary>
    public partial class NowPlayingPage : UserControl
    {
        public static Check _check = new Check();

        public NowPlayingPage()
        {
            InitializeComponent();
            _check.PropertyChanged += Check_PropertyChanged;

            if (MainWindow.currentlyPlayingSong != null)
			{
                TagLib.File file = TagLib.File.Create(MainWindow.GetNowPlaying());
                if (file.Tag.Pictures.Length >= 1)
                {
                    TagLib.IPicture pic = file.Tag.Pictures[0];
                    System.IO.MemoryStream stream = new System.IO.MemoryStream(pic.Data.Data);
                    BitmapFrame bmp = BitmapFrame.Create(stream);
                    cover.Source = bmp;
                }
                else
                {
                    BitmapImage bi = new BitmapImage();
                    bi.BeginInit();
                    bi.UriSource = new Uri("/Icon/disc.png", UriKind.Relative);
                    bi.EndInit();
                    cover.Source = bi;
                }

                string title = file.Tag.Title;
                Title.Text = title;
                if (file.Tag.AlbumArtists.Length >= 1)
                {
                    string[] artist = file.Tag.AlbumArtists;
                    Artist.Text = "Artist \t" + artist[0].ToString();
                }
                else
                {
                    Artist.Text = "Artist \t" + "Unknown";
                }

                string album = file.Tag.Album;
                Album.Text = "Album \t" + album;
                if (file.Tag.Genres.Length >= 1)
                {
                    string[] genre = file.Tag.Genres;
                    Genre.Text = "Genre \t" + genre[0].ToString();
                }
                else
                {
                    Genre.Text = "Genre \t" + "Unknown";
                }

                if (file.Tag.Year != 0)
                {
                    uint year = file.Tag.Year;
                    Year.Text = "Year \t" + year.ToString();
                }
                else
                {
                    Year.Text = "Year \t" + "Unknown";
                }
            }
			else
			{
				Grid.Visibility = Visibility.Hidden;
			}
        }

        private void Check_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            TagLib.File file = TagLib.File.Create(MainWindow.GetNowPlaying());
            if (file.Tag.Pictures.Length >= 1)
            {
                TagLib.IPicture pic = file.Tag.Pictures[0];
                System.IO.MemoryStream stream = new System.IO.MemoryStream(pic.Data.Data);
                BitmapFrame bmp = BitmapFrame.Create(stream);
                cover.Source = bmp;
            }
            else
            {
                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                bi.UriSource = new Uri("/Icon/disc.png", UriKind.Relative);
                bi.EndInit();
                cover.Source = bi;
            }

            string title = file.Tag.Title;
            Title.Text = title;
            if (file.Tag.AlbumArtists.Length >= 1)
            {
                string[] artist = file.Tag.AlbumArtists;
                Artist.Text = "Artist \t" + artist[0].ToString();
            }
            else
            {
                Artist.Text = "Artist \t" + "Unknown";
            }

            string album = file.Tag.Album;
            Album.Text = "Album \t" + album;
            if (file.Tag.Genres.Length >= 1)
            {
                string[] genre = file.Tag.Genres;
                Genre.Text = "Genre \t" + genre[0].ToString();
            }
            else
            {
                Genre.Text = "Genre \t" + "Unknown";
            }

            if (file.Tag.Year != 0)
            {
                uint year = file.Tag.Year;
                Year.Text = "Year \t" + year.ToString();
            }
            else
            {
                Year.Text = "Year \t" + "Unknown";
            }
        }
    }
}
