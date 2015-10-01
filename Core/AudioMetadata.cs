using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MediaInfoLib;
using System.Text.RegularExpressions;

namespace Core
{
    public class AudioMetadata
    {
        public string Title { get; private set; }
        public string Album { get; private set; }
        public string Artist { get; private set; }
        public string Genre { get; private set; }
        public string RecordDate { get; private set; }
        public TimeSpan Duration { get; private set; }
        public string DurationString { get { return String.Format("{0}:{1}:{2}", Duration.Hours.ToString().PadLeft(2, '0'), Duration.Minutes.ToString().PadLeft(2, '0'), Duration.Seconds.ToString().PadLeft(2, '0')); } }
        public AudioMetadata()
        {
            Duration = new TimeSpan();
        }
        public AudioMetadata(string Filename)
        {
            var MI = new MediaInfo();
            MI.Open(Filename);
            Title = MI.Get(StreamKind.General, 0, "Title");
            Album = MI.Get(StreamKind.General, 0, "Album");
            Artist = MI.Get(StreamKind.General, 0, "Performer");
            Genre = MI.Get(StreamKind.General, 0, "Genre");
            RecordDate = MI.Get(StreamKind.General, 0, "Recorded_Date");
            var duration = MI.Get(StreamKind.General, 0, "Duration");
            Duration = new TimeSpan(Convert.ToInt64(duration) * 10000);
        }
    }
}
