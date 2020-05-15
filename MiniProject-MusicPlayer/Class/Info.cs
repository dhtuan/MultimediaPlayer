using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MiniProject_MusicPlayer
{
	public class Info
	{
		public ImageSource _cover;
		public string _title;
		public string _artist;
        public string _album;
        public string _filename;

		public ImageSource Cover
		{
			get
			{
				return _cover;
			}
			set
			{
				_cover = value;
			}
		}

		public string Title
		{
			get
			{
				return _title;
			}
			set
			{
				_title = value;
			}
		}

		public string Artist
		{
			get
			{
				return _artist;
			}
			set
			{
				_artist = value;
			}
		}

        public string Album
        {
            get
            {
                return _album;
            }
            set
            {
                _album = value;
            }
        }

        public string FileName
		{
			get
			{
				return _filename;
			}
			set
			{
				_filename = value;
			}
		}

		public Info(ImageSource Cover, string Title, string Artist, string Album, string FileName)
		{
			this.Cover = Cover;
			this.Title = Title;
			this.Artist = Artist;
            this.Album = Album;
            this.FileName = FileName;
		}
	}
}
