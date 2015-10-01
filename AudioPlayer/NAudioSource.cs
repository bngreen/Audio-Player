using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core;
using NAudio;
using System.Threading;
using DSP;

namespace AudioPlayer
{
    class NAudioSource : AudioSource
    {
        public delegate void ProcessSamplesDelegate(short[] input, short[] output);
        public delegate void PostProccessAudioDelegate(byte[] audiodata, long length);
        public static PostProccessAudioDelegate PostProcessAudio { get; set; }
        public static ProcessSamplesDelegate ProcessSamples { get; set; }
        private NAudio.Wave.WaveStream inputstream;
        private InterlockedCircularBuffer<byte> AudioBuffer { get; set; }
        public NAudio.Wave.WaveFormat WaveFormat { get; private set; }
        public int DataBlock { get; set; }
        private NAudioSource()
        {
            DataBlock = Config.BlockSize;
            tempBuffer = new byte[DataBlock * 4];
            Position = 0;
            AudioBuffer = new InterlockedCircularBuffer<byte>((int)(Config.BufferSize));
            AudioBuffer.BufferLowEvent += new EventHandler(AudioBuffer_BufferLowEvent);
            BufferingCapability = AudioBuffer.Capacity - (DataBlock * 4);
            BufferingCritical = (long)((double)AudioBuffer.Capacity * 0.1);
            channel1 = new short[DataBlock];
            channel2 = new short[DataBlock];
        }

        void AudioBuffer_BufferLowEvent(object sender, EventArgs e)
        {
            FillBuffer();
        }
        public NAudioSource(string file)
            : this()
        {
            LoadMetadata(file);
            var reader = new NAudio.Wave.Mp3FileReader(file);
            var cc = NAudio.Wave.WaveFormatConversionStream.CreatePcmStream(reader);
            inputstream = new NAudio.Wave.BlockAlignReductionStream(cc);
            Length = inputstream.Length;
            BitsPerSample = inputstream.WaveFormat.BitsPerSample;
            Channels = inputstream.WaveFormat.Channels;
            SampleFrequency = inputstream.WaveFormat.SampleRate;
            WaveFormat = inputstream.WaveFormat;
            if (BitsPerSample != 16 || Channels != 2)
                throw new NotImplementedException();
            Name = System.IO.Path.GetFileNameWithoutExtension(file);
           // FillBuffer();
            //ThreadPool.QueueUserWorkItem(PopulateBuffer);
        }
        byte[] tempBuffer;

        private long BufferingCapability { get; set; }
        private long BufferingCritical { get; set; }
        short[] channel1;
        short[] channel2;
        private int failed_populate = 0;
        private void PopulateBuffer(object state)
        {
            //lock (AudioBuffer)
            //{
                while (BufferingCapability > AudioBuffer.Available)
                {
                    var count = inputstream.Read(tempBuffer, 0, (int)(DataBlock * 4));
                    if (inputstream.Position >= inputstream.Length && count == 0)
                        return;
                    if (Position != 0 && count == 0)
                    {
                        failed_populate++;
                        if (failed_populate > 20)
                            return;
                    }
                    if (count == 0)
                        continue;
                    for (int i = 0; i < count; i += 4)
                    {
                        var blockindex = i / 4;
                        channel1[blockindex] = BitConverter.ToInt16(tempBuffer, i);
                        channel2[blockindex] = BitConverter.ToInt16(tempBuffer, i + 2);
                    }
                    if (ProcessSamples != null)
                    {
                        ProcessSamples(channel1, channel1);
                        ProcessSamples(channel2, channel2);
                    }
                    var blockcount = count / 4;
                    for (int i = 0; i < blockcount; i++)
                    {
                        var x = i * 4;
                        var ch1 = channel1[i];
                        var ch2 = channel2[i];
                        var dt = BitConverter.GetBytes(ch1);
                        tempBuffer[x] = dt[0];
                        tempBuffer[x + 1] = dt[1];
                        dt = BitConverter.GetBytes(ch2);
                        tempBuffer[x + 2] = dt[0];
                        tempBuffer[x + 3] = dt[1];
                    }
                    AudioBuffer.Write(tempBuffer, count);
                }
            //}
        }

        private int isFillingBuffer = 0;

        private void FillBufferWorker(object state)
        {
            if (Interlocked.Exchange(ref isFillingBuffer, 1) != 0)
                return;//is already filling the buffer
            PopulateBuffer(null);
            Interlocked.Exchange(ref isFillingBuffer, 0);
        }
        private void FillBuffer()
        {
            ThreadPool.QueueUserWorkItem(FillBufferWorker);
        }
        private bool Ended { get { return Position >= Length; } }


        public override void SetPosition(long position)
        {
            //lock (AudioBuffer)
            //{
                position -= position % 4;
                AudioBuffer.Clear();
                Position = position;
                inputstream.Position = position;
                //FillBuffer();
           // }
        }

        private void mediaEnded()
        {
            failed_populate = 0;
            SetPosition(0);
            InformMediaEnded();
        }
        private bool HasEnded()
        {
            if ((Ended && AudioBuffer.Available == 0) || failed_populate > 20)
                return true;
            return false;
        }
        public override long ReadSamples(byte[] data, long count)
        {
            if (HasEnded())
            {
                mediaEnded();
                return 0;
            }
           // ThreadPool.QueueUserWorkItem(PopulateBuffer);
            if (AudioBuffer.Available == 0)
                FillBuffer();
            while (AudioBuffer.Available == 0)
            {
                int a = 0;
                a++;
                //this is really bad, buffer run out
            }
            /*lock (AudioBuffer)
            {*/
                long performed = Math.Min(count, AudioBuffer.Available);
                AudioBuffer.Read(data, (int)performed);
                if (PostProcessAudio != null)
                    PostProcessAudio(data, performed);
                Position += performed;
                InformPositionChanged();
                return performed;
            //}
        }
    }
}
