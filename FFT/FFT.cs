/*
 * Converted to C# from http://www.librow.com/articles/article-10 fft.cpp
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
    public class CooleyTuckey<T> where T : IConvertible
    {
        public bool Forward(Complex[] Input, Complex[] Output, uint N)
        {
            if (Input == null || Output == null || N < 1 || (N & (N - 1)) != 0)
                return false;
            ReArrange(Input, Output, N);
            Perform(Output, N);
            return true;
        }
        public bool Forward(Complex[] Data, uint N)
        {
            if (Data == null || N < 1 || (N & (N - 1)) != 0)
                return false;
            ReArrange(Data, N);
            Perform(Data, N);
            return true;
        }
        public bool Forward(T[] Input, Complex[] Output, uint N)
        {
            if (Input == null || Output == null || N < 1 || (N & (N - 1)) != 0)
                return false;
            for (int i = 0; i < N; i++)
                Output[i] = new Complex(Convert.ToDouble(Input[i]));
            ReArrange(Output, N);
            Perform(Output, N);
            return true;
        }
        public bool Inverse(Complex[] Input, Complex[] Output, uint N, bool scale = true)
        {
            if (Input == null || Output == null || N < 1 || (N & (N - 1)) != 0)
                return false;
            ReArrange(Input, Output, N);
            Perform(Output, N, true);
            if (scale)
                Scale(Output, N);
            return true;
        }
        public bool Inverse(Complex[] Data, uint N, bool scale = true)
        {
            if (Data == null || N < 1 || (N & (N - 1)) != 0)
                return false;
            ReArrange(Data, N);
            Perform(Data, N, true);
            if (scale)
                Scale(Data, N);
            return true;
        }
        private void ReArrange(Complex[] Input, Complex[] Output, uint N)
        {
            uint target = 0;
            for (uint Position = 0; Position < N; ++Position)
            {
                Output[target] = Input[Position];
                uint mask = N;
                while ((target & (mask >>= 1)) != 0)
                    target &= ~mask;
                target |= mask;
            }
        }
        private void ReArrange(Complex[] Data, uint N)
        {
            uint target = 0;
            for (uint Position = 0; Position < N; ++Position)
            {
                if (target > Position)
                {
                    Complex temp = new Complex(Data[target]);
                    Data[target] = Data[Position];
                    Data[Position] = temp;
                }
                uint mask = N;
                while ((target & (mask >>= 1)) != 0)
                    target &= ~mask;
                target |= mask;
            }
        }
        private void Perform(Complex[] Data, uint N, bool Inverse = false)
        {
            double pi = Inverse ? Math.PI : -Math.PI;
            for (uint Step = 1; Step < N; Step <<= 1)
            {
                uint Jump = Step << 1;
                double delta = pi / Step;
                double sine = Math.Sin(delta / 2);
                Complex Multiplier = new Complex(-2 * sine * sine, Math.Sin(delta));
                Complex Factor = new Complex(1, 0);
                for (uint Group = 0; Group < Step; ++Group)
                {
                    for (uint Pair = Group; Pair < N; Pair += Jump)
                    {
                        uint Match = Pair + Step;
                        Complex Product = new Complex(Factor * Data[Match]);
                        Data[Match] = Data[Pair] - Product;
                        Data[Pair] += Product;
                    }
                    Factor = Multiplier * Factor + Factor;
                }
            }
        }
        private void Scale(Complex[] Data, uint N)
        {
            for (uint Position = 0; Position < N; ++Position)
                Data[Position] /= N;

        }
    }
}
