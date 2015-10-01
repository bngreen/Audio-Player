using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FFT;

namespace DSP
{
    public class ShortFilterSet
    {
        public List<IFilter> Filters { get; private set; }
        public int N { get; private set; }
        private CooleyTuckey<short> fft = new CooleyTuckey<short>();
        public IWindowFunction WindowFunction { get; set; }
        public ShortFilterSet(int N)
        {
            this.N = N;
            Filters = new List<IFilter>();
            buffer = new Complex[N];
            WindowFunction = new HammingWindow(N);
        }
        private Complex[] buffer;
        public bool PerformFilter(short[] input, short[] output, int N)
        {
            if (N != this.N)
                return false;
            for (int i = 0; i < N; i++)
                input[i] = (short)(input[i] * WindowFunction.W[i]);
            fft.Forward(input, buffer, (uint)N);
            foreach (var filter in Filters)
            {
                var H = filter.H;
                for (int i = 0; i < buffer.Length; i++)
                    buffer[i] *= H[i];
            }
            fft.Inverse(buffer, (uint)N);
            double maxVal = double.MinValue;
            for (int i = 0; i < N; i++)
            {
                var val = buffer[i].Real = buffer[i].Real / WindowFunction.W[i];
                if (maxVal < val)
                    maxVal = val;
            }
            for (int i = 0; i < N; i++)
            {
                var val = buffer[i].Real;
                val = Math.Min(Math.Abs(val), short.MaxValue) * Math.Abs(val) / val;//take care of overflows
                output[i] = (short)(val);//short.MaxValue * buffer[i].Real / maxVal);
            }
            return true;
        }
    }
}
