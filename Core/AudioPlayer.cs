using System;
namespace Core
{
    public class AudioPlayer
    {
        public AudioPlayer()
        {
            Shuffle = false;
            Playlist = new AudioSourceCollection();
            AudioSource.MediaEnded += new EventHandler(AudioSource_MediaEnded);
        }

        public int Position
        {
            get
            {
                if (PlayingMedia == null)
                    return 0;
                return (int)(100 * (double)PlayingMedia.Position / PlayingMedia.Length); }
            set
            {
                if (PlayingMedia == null)
                    return;
                //For Re-buffering purposes i must stop the playback.
                Stop();
                PlayingMedia.SetPosition((value * PlayingMedia.Length) / 100);
                Play(PlayingMedia);
            }
        }

        void AudioSource_MediaEnded(object sender, EventArgs e)
        {
            Next();
        }
        private Random random = new Random();
        int sourceindex = 0;
        private AudioSource GetNextSource()
        {
            if (Playlist.Count == 0)
                return null;
            if (Shuffle)
            {
                return Playlist[random.Next(Playlist.Count - 1)];
            }
            else
            {
                return Playlist[(sourceindex++) % Playlist.Count];
            }
        }
        private AudioSource GetPreviousSource()
        {
            if (Playlist.Count == 0)
                return null;
            if (Shuffle)
            {
                return Playlist[random.Next(Playlist.Count - 1)];
            }
            else
            {
                sourceindex--;
                if (sourceindex < 0)
                    sourceindex = Playlist.Count - 1;
                return Playlist[sourceindex];
            }
        }

        public AudioSource PlayingMedia { get; protected set; }

        protected virtual void Play(AudioSource source)
        {
            if (source == null)
                return;
            PlayingMedia = source;
            AudioSink.Play(source);
        }

        public void Next()
        {
            if (Playlist.Count < 2)
                return;
            Play(GetNextSource());
        }
        public void Pause()
        {
            AudioSink.Pause();
        }
        public void Play()
        {
            Play(GetNextSource());
        }
        public AudioSourceCollection Playlist { get; protected set; }
        public void Previous()
        {
            if (Playlist.Count < 2)
                return;
            Play(GetPreviousSource());
        }
        public void Stop()
        {
            AudioSink.Stop();
        }
        public virtual double Volume { get; set; }

        protected IAudioSink AudioSink { get; set; }

        public bool Shuffle { get; set; }

        public void Continue()
        {
            AudioSink.Continue();
        }
    }
}
