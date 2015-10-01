/*
 * Converted to C# from http://www.librow.com/articles/article-10 complex.cpp
 * Original code is property of LIBROW
 * You can use it on your own
 * When utilizing credit LIBROW site
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FFT
{
    public class Complex
    {
        public double Real { get; set; }
        public double Imaginary { get; set; }
        public Complex(double Real, double Imaginary)
        {
            this.Real = Real;
            this.Imaginary = Imaginary;
        }
        public Complex(double Real)
            : this(Real, 0)
        {
        }
        public Complex(Complex complex)
        {
            Real = complex.Real;
            Imaginary = complex.Imaginary;
        }
        public Complex()
            : this(0, 0)
        {
        }
        public static Complex operator +(Complex c1, Complex c2)
        {
            return new Complex(c1.Real + c2.Real, c1.Imaginary + c2.Imaginary);
        }
        public static Complex operator -(Complex c1, Complex c2)
        {
            return new Complex(c1.Real - c2.Real, c1.Imaginary - c2.Imaginary);
        }
        public static Complex operator *(Complex c1, Complex c2)
        {
            return new Complex(c1.Real * c2.Real - c1.Imaginary * c2.Imaginary, c1.Real * c2.Imaginary + c1.Imaginary * c2.Real);
        }
        public static Complex operator /(Complex c1, Complex c2)
        {
            double denomitator = c2.Real * c2.Real + c2.Imaginary * c2.Imaginary;
            return new Complex((c1.Real * c2.Real + c1.Imaginary * c2.Imaginary) / denomitator, (c1.Imaginary * c2.Real - c1.Real * c2.Imaginary) / denomitator);
        }
        public static Complex operator *(Complex c, double num)
        {
            return new Complex(c.Real * num, c.Imaginary * num);
        }
        public static Complex operator /(Complex c, double num)
        {
            return new Complex(c.Real / num, c.Imaginary / num);
        }
        public static Complex operator +(Complex c, double num)
        {
            return new Complex(c.Real + num, c.Imaginary);
        }
        public static Complex operator -(Complex c, double num)
        {
            return new Complex(c.Real - num, c.Imaginary);
        }
        public static Complex operator ++(Complex c)
        {
            return new Complex(c.Real + 1, c.Imaginary);
        }
        public static Complex operator --(Complex c)
        {
            return new Complex(c.Real - 1, c.Imaginary);
        }
        public static bool operator ==(Complex c1, Complex c2)
        {
            return (c1.Real == c2.Real && c1.Imaginary == c2.Imaginary) ? true : false;
        }
        public static bool operator !=(Complex c1, Complex c2)
        {
            return !(c1 == c2);
        }
        public Complex Conjugate { get { return new Complex(Real, -Imaginary); } }
        public double Norm { get { return Math.Pow(Real * Real + Imaginary * Imaginary, 0.5); } }
        public override string ToString()
        {
            return String.Format("{0} + i{1}", Real, Imaginary);
        }
        public override bool Equals(object obj)
        {
            if (!(obj is Complex))
                return false;
            return this == (obj as Complex);
        }
        public override int GetHashCode()
        {
            return (int)Real | (int)Imaginary;
        }
    }
}
