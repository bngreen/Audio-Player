using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core
{
    public interface IAudioSink
    {
        void Play(AudioSource AudioSource);

        void Pause();

        void Stop();

        void Continue();
    }
}
