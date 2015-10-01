using System;
namespace Core
{
    public abstract class AudioSource
    {
        public int BitsPerSample { get; set; }
        public int Channels { get; set; }
        public long Length { get; set; }
        public string Name { get; set; }
        public long Position { get; protected set; }
        public abstract long ReadSamples(byte[] data, long count);
        public int SampleFrequency { get; set; }
        public AudioMetadata Metadata { get; private set; }
        public static event EventHandler MediaEnded;
        public string DisplayText { get { return String.Format("{0} ● {1} ● {2}", Metadata.Title, Metadata.Artist, Metadata.Album); } }
        public AudioSource()
        {
            Metadata = new AudioMetadata();
        }
        protected void LoadMetadata(string file)
        {
            Metadata = new AudioMetadata(file);
        }
        public AudioSource(string file)
        {
            Metadata = new AudioMetadata(file);
        }
        protected void InformMediaEnded()
        {
            if (MediaEnded != null)
                MediaEnded.Invoke(this, null);
        }

        public static event EventHandler PositionChanged;

        protected void InformPositionChanged()
        {
            if (PositionChanged != null)
                PositionChanged.Invoke(this, null);
        }

        public abstract void SetPosition(long position);
    }
}
