using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DSP
{
    public class LanczosWindow:WindowFunction
    {
        public LanczosWindow(int N):
            base(N)
        {

        }
        public override double w(int n)
        {
            var x = (Math.PI*2*n)/(N-1);
            return Math.Sin(x) / x;
        }
    }
}
