using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DSP
{
    class KBDWindow:WindowFunction
    {
        // Converted to C# by Bruno
        // Original From:
        // Programmer:    Craig Stuart Sapp <craig@ccrma.stanford.edu>
        // Creation Date: Sat Jan 27 14:27:14 PST 2001
        // Last Modified: Sat Jan 27 15:24:58 PST 2001
        // Filename:      kbdwindow.cpp
        // $Smake:        g++ -O6 -o %b %f
        // Syntax:        C++; functions in ANSI C
        //
        // Description:   This is a sample program for generating
        //                Kaiser-Bessel Derived Windows for audio signal
        //                processing -- in particular for the Time-Domain
        //                Alias Cancellation procedure which has the
        //                overlap-add requirements:
        //                   Window_m[N-1-n] + Window_m+1[n]^2 = 1.0;
        //                which means: The squares of the overlapped
        //                windows must add to the constant value 1.0
        //                for time domain alias cancellation to work.
        //
        //                The two function necessary to create the KBD window are:
        //                   KBDWindow -- calculates the window values.
        //                   BesselI0  -- bessel function needed for KBDWindow.
        //                
        //

        public double Alpha { get; private set; }
        public KBDWindow(int N, double alpha = 0)
            : base(N)
        {
            Alpha = alpha;
            kbd_w = new double[N];
            double sumvalue = 0;
            double PI = Math.PI;
            for (int i = 0; i < N / 2; i++)
            {
                sumvalue += BesselI0(PI * alpha * Math.Sqrt(1.0 - Math.Pow(4.0 * i / N - 1.0, 2)));
                kbd_w[i] = sumvalue;
            }
            sumvalue += BesselI0(PI * alpha * Math.Sqrt(1.0 - Math.Pow(4.0 * (N / 2) / N - 1.0, 2)));
            for (int i = 0; i < N / 2; i++)
            {
                kbd_w[i] = Math.Sqrt(kbd_w[i] / sumvalue);
                kbd_w[N - 1 - i] = kbd_w[i];
            }
            for (int i = 0; i < N; i++)
            {
                W[i] = kbd_w[i];
            }
        }
        private double[] kbd_w { get; set; }
        public override double w(int n)
        {
            return 0;
        }
        private double BesselI0(double x)
        {
            double denominator;
            double numerator;
            double z;

            if (x == 0.0)
            {
                return 1.0;
            }
            else
            {
                z = x * x;
                numerator = (z * (z * (z * (z * (z * (z * (z * (z * (z * (z * (z * (z * (z *
                               (z * 0.210580722890567e-22 + 0.380715242345326e-19) +
                                   0.479440257548300e-16) + 0.435125971262668e-13) +
                                   0.300931127112960e-10) + 0.160224679395361e-7) +
                                   0.654858370096785e-5) + 0.202591084143397e-2) +
                                   0.463076284721000e0) + 0.754337328948189e2) +
                                   0.830792541809429e4) + 0.571661130563785e6) +
                                   0.216415572361227e8) + 0.356644482244025e9) +
                                   0.144048298227235e10);

                denominator = (z * (z * (z - 0.307646912682801e4) +
                                 0.347626332405882e7) - 0.144048298227235e10);
            }

            return -numerator / denominator;
        }

    }
}
