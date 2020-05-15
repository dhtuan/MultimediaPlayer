using MiniProject_MusicPlayer.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for PlaylistPage.xaml
    /// </summary>
    public partial class PlaylistPage : UserControl
    {
        public static BindingList<Info> _Playlist = new BindingList<Info>();
		public static List<int> indexes = new List<int>();
		public static int startIndex;

        public PlaylistPage(BindingList<Info> temp)
        {
            InitializeComponent();
            _Playlist = temp;
            this.DataContext = this;
            PlayListListViewPage.ItemsSource = _Playlist;
		}


        private void RemoveMenuItem_Click(object sender, RoutedEventArgs e)
		{
			if (PlayListListViewPage.SelectedItem != null)
			{
				_Playlist.RemoveAt(PlayListListViewPage.SelectedIndex);
			}
			
		}

		public static void refillIndexesList()
		{
			int i = 0;

			foreach(var info in _Playlist)
			{
				indexes.Add(i);
				i++;
			}
		}

		public static int findSongIndex(string FileName)
		{
			foreach(var info in _Playlist)
			{
				if(info.FileName == FileName)
				{
					return _Playlist.IndexOf(info);
				}
			}
			return -1;
		}

		private void PlayMenuItem_Click(object sender, RoutedEventArgs e)
		{
			var song = PlayListListViewPage.SelectedItem as Info;
            MainWindow._audio.Pause();
            MainWindow._audio.Close();
            MainWindow._timer.Stop();

            refillIndexesList();
			startIndex = findSongIndex(song.FileName);
			MainWindow._currentlyPlayingPlayList = _Playlist.ToList();
			MainWindow.SetNowPlaying(song.FileName);
            MainWindow._audio.MediaOpened += _audio_MediaOpened;
            MainWindow._audio.MediaEnded += _audio_MediaEnded;
        }

        public void _audio_MediaEnded(object sender, EventArgs e)
		{			
			MainWindow._isHavingAPlayListRunning = false;
			MainWindow._isPlaying = false;
			MainWindow._timer.Stop();
			Info nextSong = null;

			if(MainWindow._isRepeat == true && MainWindow._isShuffle == true)
			{
				Random rnd = new Random();

				if(indexes.Count == 0)
				{
					refillIndexesList();
				}

				int position = rnd.Next(indexes.Count);

				nextSong = _Playlist[indexes[position]];

				indexes.RemoveAt(position);
			}
			else if(MainWindow._isRepeat == true && MainWindow._isShuffle == false)
			{
				for (int i = 0; i < _Playlist.Count; i++)
				{
					if (MainWindow.currentlyPlayingSong == _Playlist[i].FileName)
					{
						if (i + 1 < _Playlist.Count)
						{
							nextSong = _Playlist[i + 1];
						}
						else if (i + 1 == _Playlist.Count)
						{
							nextSong = _Playlist[0];
						}
					}
				}
			}
			else if(MainWindow._isRepeat == false && MainWindow._isShuffle == true)
			{
				Random rnd = new Random();

				if(indexes.Count != 0)
				{
					int position = rnd.Next(indexes.Count);

					nextSong = _Playlist[indexes[position]];

					indexes.RemoveAt(position);
				}
			}
			else if(MainWindow._isRepeat == false && MainWindow._isShuffle == false)
			{
				for (int i = 0; i < _Playlist.Count; i++)
				{
					if (MainWindow.currentlyPlayingSong == _Playlist[i].FileName)
					{
						if (i + 1 < _Playlist.Count && startIndex != i + 1)
						{
							nextSong = _Playlist[i + 1];
						}
						else if(i + 1 == _Playlist.Count && startIndex != 0)
						{
							nextSong = _Playlist[0];
						}
					}
				}
			}

			if(nextSong != null)
			{
				MainWindow._audio.Close();
				MainWindow.SetNowPlaying(nextSong.FileName);
			}
            else
            {
                MainWindow._checkEnd.ChangeEnd = true;
            }
        }

		public void _audio_MediaOpened(object sender, EventArgs e)
		{
			MainWindow._isHavingAPlayListRunning = true;
			MainWindow._audio.Play();
			MainWindow._isPlaying = true;
			MainWindow._timer.Start();
            MainWindow._checkOpen.ChangeOpen = true;

            foreach (var info in _Playlist)
			{
				if(info.FileName == MainWindow.currentlyPlayingSong)
				{
					indexes.Remove(_Playlist.IndexOf(info));
				}
			}

            for (int i = 0; i < _Playlist.Count; i++)
            {
                if (MainWindow.currentlyPlayingSong == _Playlist[i].FileName)
                {
                    MainWindow._playlistList[MainWindow._globalindex].Save(true, i);
                }
            }
        }
	}
}
