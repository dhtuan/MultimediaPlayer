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
using System.Windows.Shapes;

namespace MiniProject_MusicPlayer
{
	/// <summary>
	/// Interaction logic for AddMusicToPlaylist.xaml
	/// </summary>
	public partial class AddMusicToPlaylist
	{
		public AddMusicToPlaylist()
		{
			InitializeComponent();
			AddToPlayListsListView.ItemsSource = MainWindow._playlistList;
		}

		private void AddButton_Click(object sender, RoutedEventArgs e)
		{
			if(AddToPlayListsListView.SelectedItem != null)
			{
				var selected = AddToPlayListsListView.SelectedItem as Playlist;
				MyMusicPage.selectedPlayListName = selected.Name;
				this.DialogResult = true;
			}
		}
	}
}
