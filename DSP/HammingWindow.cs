using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DSP
{
    public class HammingWindow : DSP.IWindowFunction
    {
        public int N { get; private set; }
        public HammingWindow(int N)
        {
            this.N = N;
            W = new double[N];
            for (int i = 0; i < N; i++)
                W[i] = w(i);
        }
        public double w(int n)
        {
            return 0.54 - 0.46*Math.Cos((2 * Math.PI * n) / (N - 1));
        }
        public double[] W { get; private set; }
    }
}
