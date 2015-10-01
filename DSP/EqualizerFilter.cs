using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FFT;

namespace DSP
{
    public class EqualizerFilter : IFilter
    {
        public int N { get; private set; }
        public int HalfN { get; private set; }
        public Level[] Levels { get; private set; }
        public float SampleFrequency { get; private set; }
        public int BandCount { get; private set; }
        public void SetSampleFrequency(float frequency)
        {
            SampleFrequency = frequency;
            for (int i = 0; i < Levels.Length; i++)
                Levels[i].Frequency = GetFrequency(i);
        }
        private float GetFrequency(int k)
        {
            return (k * SampleFrequency) / N;
        }
        private int GetKByFrequency(double frequency)
        {
            return (int)Math.Round((frequency * N) / SampleFrequency);
        }
        private void ValueChanged(int index, int k, float value)
        {
            SetH(k, ComplexHelper.ToComplex(value, 0));
            if (index != 0)
            {
                var PreviousL = Levels[index-1];
                var finalv = value;
                var initialv = PreviousL.Value;
                var initialk = PreviousL.K;
                var finalk = k;
                SetMiddleHs(finalv, initialv, initialk, finalk);
            }
            else if (k != 0)
            {
                var Complex = ComplexHelper.ToComplex(value,0);
                for (int i = 0; i < k; i++)
                    SetH(i, Complex);
            }
            if (index != Levels.Count() - 1)
            {
                var FinalL = Levels[index + 1];
                var initialv = value;
                var finalv = FinalL.Value;
                var initialk = k;
                var finalk = FinalL.K;
                SetMiddleHs(finalv, initialv, initialk, finalk);
            }
            else if (k != HalfN - 1)
            {
                var Complex = ComplexHelper.ToComplex(value, 0);
                for (int i = k; i < HalfN; i++)
                    SetH(i, Complex);
            }
            //NormalizeH();
        }

        private void NormalizeH()
        {
            var maxH = double.MinValue;
            for (int i = 0; i < N; i++)
                if (maxH < H[i].Real)
                    maxH = H[i].Real;
            for (int i = 0; i < N; i++)
                H[i] = ComplexHelper.ToComplex(H[i].Real / maxH, 0);
        }

        private void SetMiddleHs(float finalv, float initialv, int initialk, int finalk)
        {
            double a = (double)(finalv - initialv) / (finalk - initialk);
            for (int i = initialk; i < finalk; ++i)
                SetH(i, ComplexHelper.ToComplex(initialv + a * (i - initialk), 0));
        }

        private void SetH(int k, Complex value)
        {
            H[N - 1 - k] = H[k] = value;
        }
        int GetKbyIndex(int index)
        {
            return index * HalfN / BandCount;
        }
        int GetIndexByK(int k)
        {
            return k * BandCount / HalfN;
        }
        public EqualizerFilter(int N, float sampleFrequency, int BandCount)
        {
            this.N = N;
            HalfN = N / 2;
            SampleFrequency = sampleFrequency;
            Levels = new Level[BandCount];
            this.BandCount = BandCount;
            var initialFrequency = 16.0;
            var r = Math.Pow(SampleFrequency / (2 * initialFrequency), 1.0 / (BandCount - 1));
            for (int i = 0; i < BandCount; i++)
            {
                var freq = initialFrequency * Math.Pow(r, i);
                var k = GetKByFrequency(freq);
                Levels[i] = new Level(GetFrequency(k), k, i, ValueChanged);
            }
            H = new Complex[N];
            for (int i = 0; i < N; i++)
                H[i] = new Complex();
        }
        public Complex[] H { get; private set; }
            /*
        {
            get
            {
                Complex[] z = new Complex[N];
                for (int i = 0; i < N; i++)
                    z[i] = ComplexHelper.ToComplex(Levels[i], 0);
                return z;
            }
        }*/
        public Complex HAt(uint k)
        {
            if (k >= N)
                throw new OverflowException("k cant be bigger or equal to N");
            return H[k];
        }
    }
}
