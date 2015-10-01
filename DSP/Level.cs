using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DSP
{
    public class Level
    {
        private float value;
        public float Value
        {
            get { return value; }
            set
            {
                this.value = value;
                if (ValueChanged != null)
                    ValueChanged(Index, K, Value);
            }
        }

        public Level()
        {
        }

        public delegate void ValueChangedDelegate(int index, int k, float value);
        public ValueChangedDelegate ValueChanged { get; private set; }
        public int K { get; private set; }
        public int Index { get; private set; }
        public static implicit operator float(Level level)
        {
            return level.Value;
        }
        public static implicit operator double(Level level)
        {
            return level.Value;
        }
        public float Frequency { get; set; }
        public string PrettyFrequency { get { return String.Format("{0} Hz", (int)Math.Round(Frequency)); } }
        public Level(float frequency, int k, int index , ValueChangedDelegate valueChanged = null)
        {
            this.Index = index;
            K = k;
            this.Frequency = frequency;
            ValueChanged = valueChanged;
        }
       /* public static implicit operator Level(float level)
        {
            return new Level() { Value = level };
        }
        public static implicit operator Level(double level)
        {
            return new Level() { Value = (float)level };
        }*/
    }
}
