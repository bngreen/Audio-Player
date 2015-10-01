using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FFT;

namespace DSP
{
    public class ComplexHelper
    {
        public static double Gain(Complex complex)
        {
            return 20 * Math.Log10(complex.Norm);
        }

        public static double Phase(Complex complex)
        {
            return Math.Atan(complex.Imaginary / complex.Real);
        }

        public static Complex ToComplex3(double Gain, double Phase)
        {
            return new Complex(Gain * Math.Cos(Phase), Gain * Math.Sin(Phase));
        }

        public static Complex ToComplex(double Gain, double Phase)
        {
            double gain = Math.Pow(10, Gain / 20);
            return new Complex(gain * Math.Cos(Phase), gain * Math.Sin(Phase));
        }

        public static Complex ToComplex2(double Gain, double Phase)
        {
            double alfa = Math.Tan(Phase);
            //double real = Math.Pow((Math.Pow(10, Gain / 10) * Math.Pow(Math.Tan(Phase), 2)) / (1 + Math.Pow(Math.Tan(Phase), 2)), 0.5);
            double real = Math.Pow(Math.Pow(10, Gain / 10) / (1 + Math.Pow(alfa, 2)), 0.5);
            double imaginary = real * alfa;
            return new Complex(real, imaginary);
        }

    }
}
