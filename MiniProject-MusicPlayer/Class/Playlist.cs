using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniProject_MusicPlayer
{
    public class Playlist
    {
        public string _name;
        public BindingList<Info> _song;
        public bool _lastPlayed = false;
        public int _index = -1;

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        public BindingList<Info> Song
        {
            get
            {
                return _song;
            }
            set
            {
                _song = value;
            }
        }

        public bool LastPlayed
        {
            get
            {
                return _lastPlayed;
            }
            set
            {
                _lastPlayed = value;
            }
        }

        public int Index
        {
            get
            {
                return _index;
            }
            set
            {
                _index = value;
            }
        }

        public Playlist(string Name, BindingList<Info> Songx)
        {
            this._name = Name;
            this._song = Song;
        }

        public Playlist(string Name, BindingList<Info> Song, bool Played, int Index)
        {
            this._name = Name;
            this._song = Song;
            this._lastPlayed = Played;
            this._index = Index;
        }

        public void Save()
        {
            var writer = new StreamWriter(Name + ".dat");

            foreach (var song in Song)
            {
                writer.WriteLine(song.FileName);
            }

            writer.WriteLine(":");

            writer.WriteLine(LastPlayed);
            writer.WriteLine(Index);

            writer.Close();
        }

        public void Save(bool Played, int Index)
        {
            var writer = new StreamWriter(Name + ".dat");

            foreach (var song in Song)
            {
                writer.WriteLine(song.FileName);
            }

            writer.WriteLine(":");

            writer.WriteLine(Played);
            writer.WriteLine(Index);

            writer.Close();
        }
    }
}
