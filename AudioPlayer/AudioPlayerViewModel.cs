using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.ComponentModel;
using Microsoft.Expression.Interactivity.Core;
using Microsoft.Win32;
using System.Threading;
using System.Windows.Threading;
using System.Windows;

namespace AudioPlayer
{
    public class AudioPlayerViewModel : NAudioPlayer, INotifyPropertyChanged
    {
        public AudioPlayerViewModel()
            : base()
        {
            Playlist = new ObservablePlaylist();
            Volume = 100;
            Core.AudioSource.PositionChanged += new EventHandler(AudioSource_PositionChanged);
        }
        protected override void Play(Core.AudioSource source)
        {
            base.Play(source);
            NotifyPropertyChanged("PlayingMedia");
        }

        void AudioSource_PositionChanged(object sender, EventArgs e)
        {
            NotifyPropertyChanged("Position");
        }
        public ICommand PlayCommand { get { return new ActionCommand(() => Play()); } }
        public ICommand StopCommand { get { return new ActionCommand(() => Stop()); } }
        public ICommand NextCommand { get { return new ActionCommand(() => Next()); } }
        public ICommand PreviousCommand { get { return new ActionCommand(() => Previous()); } }
        public ICommand OpenEqualizer { get { return new ActionCommand(() => new EqualizerWindow(this).Show()); } }
        public ICommand AddToPlaylist
        {
            get
            {
                return new ActionCommand(() =>
                    {
                        OpenFileDialog ofd = new OpenFileDialog();
                        ofd.Filter = "Mp3 Files|*.mp3";
                        ofd.Multiselect = true;
                        ofd.ShowDialog();
                        var filenames = ofd.FileNames;
                        foreach (var filename in filenames)
                            Playlist.Add(new NAudioSource(filename));
                    }
                );
            }
        }
        private void NotifyPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
