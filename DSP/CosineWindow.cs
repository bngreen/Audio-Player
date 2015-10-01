using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DSP
{
    class CosineWindow:WindowFunction
    {
        public CosineWindow(int N)
            :base(N)
        {

        }
        public override double w(int n)
        {
            return Math.Sin((Math.PI * n) / (N - 1));
        }
    }
}
