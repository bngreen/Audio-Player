using System;
namespace DSP
{
    public interface IWindowFunction
    {
        int N { get; }
        double w(int n);
        double[] W { get; }
    }
}
