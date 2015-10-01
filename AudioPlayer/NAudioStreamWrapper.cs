using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AudioPlayer
{
    class NAudioStreamWrapper : NAudio.Wave.WaveStream
    {
        public NAudioSource Source { get; private set; }
        public NAudioStreamWrapper(NAudioSource source)
        {
            Source = source;
        }
        public override NAudio.Wave.WaveFormat WaveFormat
        {
            get { return Source.WaveFormat; }
        }

        public override long Length
        {
            get { return Source.Length; }
        }

        public override long Position
        {
            get
            {
                return Source.Position;
            }
            set
            {
                throw new NotImplementedException();
            }
        }
        
        public override int Read(byte[] buffer, int offset, int count)
        {
            byte[] data = new byte[count];
            var finalcount = Source.ReadSamples(data, count);
            Array.Copy(data, 0, buffer, offset, (int)finalcount);
            return (int)finalcount;
        }
    }
}
