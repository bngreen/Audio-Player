using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DSP
{
    public class RetangularWindow:IWindowFunction
    {
        public RetangularWindow(int N)
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

        public double w(int n)
        {
            return 1;
        }

        public double[] W
        {
            get;
            private set;
        }
    }
}
