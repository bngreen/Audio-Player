using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DSP
{
    public abstract class WindowFunction:IWindowFunction
    {
        public WindowFunction(int N)
        {
            this.N = N;
            W = new double[N];
            for (int i = 0; i < N; i++)
                W[i] = w(i);
        }
        public int N
        {
            get;
            private set;
        }

        public abstract double w(int n);

        public double[] W
        {
            get;
            private set;
        }
    }
}
