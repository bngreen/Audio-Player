using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FFT;

namespace DSP
{
    public class VolumeFilter : IFilter
    {
        public float Volume { get; set; }
        public int N { get; private set; }
        public VolumeFilter(int N)
        {
            this.N = N;
        }

        public FFT.Complex[] H
        {
            get
            {
                Complex[] z = new Complex[N];
                Complex x = ComplexHelper.ToComplex3(Volume, 0);
                for (int i = 0; i < N; i++)
                    z[i] = x;
                return z;
            }
        }


        public Complex HAt(uint k)
        {
            if (k >= N)
                throw new OverflowException("k cant be bigger or equal to N");
            return ComplexHelper.ToComplex3(Volume, 0);
        }
    }
}
