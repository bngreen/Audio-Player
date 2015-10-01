using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DSP
{
    class HannWindow:WindowFunction
    {
        public HannWindow(int N)
            :base(N)
        {

        }
        public override double w(int n)
        {
            return 0.5 * (1.0 - Math.Cos((2 * Math.PI * n) / (N - 1)));
        }
    }
}
