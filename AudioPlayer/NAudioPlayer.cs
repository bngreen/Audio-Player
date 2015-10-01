using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AudioPlayer
{
    public class NAudioPlayer : DSPAudioPlayer
    {
        public NAudioPlayer()
            : base()
        {
            AudioSink = new NAudioSink();
        }
    }
}
