using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DSP;
using System.Threading;

namespace AudioPlayer
{
    public class DSPAudioPlayer : Core.AudioPlayer
    {
        private ShortFilterSet filterset { get; set; }
        private EqualizerFilter Equalizer { get; set; }
        private VolumeFilter VolumeFilter { get; set; }
        private void ProcessAudio(short[] input, short[] output)
        {
            filterset.PerformFilter(input, output, Config.BlockSize);
        }
        private void PostProcessAudio(byte[] data, long length)
        {
            //apply volume
            for (long i = 0; i < length; i += 2)
            {
                short dt = BitConverter.ToInt16(data, (int)i);
                dt = (short)(((double)dt) * exponentialVolume);// / 100);
                var bt = BitConverter.GetBytes(dt);
                data[i] = bt[0];
                data[i + 1] = bt[1];
            }
        }
        public DSPAudioPlayer()
            : base()
        {
            NAudioSource.ProcessSamples = ProcessAudio;
            NAudioSource.PostProcessAudio = PostProcessAudio;
            filterset = new ShortFilterSet(Config.BlockSize);
            Equalizer = new EqualizerFilter(Config.BlockSize, 44100, 18);
            VolumeFilter = new VolumeFilter(Config.BlockSize);
            //VolumeFilter.Volume = 0.15f;
            //filterset.Filters.Add(VolumeFilter);
            filterset.Filters.Add(Equalizer);
            foreach (var level in Levels)
                level.Value = -12;
        }
        private double exponentialVolume = 0;
        private double volume = 0;
        public override double Volume { get { return volume; } set { volume = value; exponentialVolume = Math.Pow(10, 45 * ((value - 100) / 100) / 20); } }//get { return VolumeFilter.Volume * 100; } set { VolumeFilter.Volume = (float)value / 100; } }
        public Level[] Levels { get { return Equalizer.Levels; } }
    }
}
