using System;
namespace DSP
{
    public interface IFilter
    {
        FFT.Complex[] H { get; }
        FFT.Complex HAt(uint k);
        int N { get; }
    }
}
