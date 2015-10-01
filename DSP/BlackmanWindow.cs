using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DSP
{
    public class BlackmanWindow:WindowFunction
    {
        private double alfa { get { return 0.16; } }
        public BlackmanWindow(int N):base(N)
        {

        }
        public override double w(int n)
        {
            var a0 = (1.0 - alfa) / 2;
            var a1 = 1.0 / 2;
            var a2 = alfa / 2;
            return a0 - a1 * Math.Cos((2 * Math.PI * n) / (N - 1)) + Math.Cos((4 * Math.PI * n) / (N - 1));
        }
    }
}
