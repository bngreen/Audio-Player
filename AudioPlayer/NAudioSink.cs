using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core;
using System.Threading;

namespace AudioPlayer
{
    class NAudioSink : IAudioSink
    {
        private NAudio.Wave.IWavePlayer Player { get; set; }
        private NAudioStreamWrapper Wrapper { get; set; }
        public NAudioSink()
        {
           // CreatePlayer();
        }

        void CreatePlayer()
        {
            if (Player != null)
            {
                Player.Stop();
                Player.Dispose();
            }
            Player = new NAudio.Wave.WaveOut();
        }

        public void Play(AudioSource AudioSource)
        {
            Wrapper = new NAudioStreamWrapper(AudioSource as NAudioSource);
            CreatePlayer();
            Player.Init(Wrapper);
            Player.Play();
        }

        public void Pause()
        {
            Player.Pause();
        }

        public void Stop()
        {
            //if (Player.PlaybackState != NAudio.Wave.PlaybackState.Stopped)
                Player.Stop();
        }

        public void Continue()
        {
            Player.Play();
        }
    }
}
