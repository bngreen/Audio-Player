using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core
{
    public class CircularBuffer<T>
    {
        private T[] buffer;
        public Int32 Capacity { get; private set; }
        private Int32 ReadPos = 0;
        private Int32 WritePos = 0;
        public int Available { get; private set; }
        public CircularBuffer(int Capacity)
        {
            buffer = new T[Capacity];
            this.Capacity = Capacity;
            Available = 0;
        }
        public void Write(T value)
        {
            buffer[WritePos% Capacity] = value;
            WritePos = (WritePos + 1) % Capacity;
            Available++;
            if (Available > Capacity)
                throw new Exception();
            /*if (WritePos == ReadPos)
                ReadPos = (ReadPos+1) % Capacity;*/
        }

        public void Write(T[] data, int count)
        {
            var len = (int)Math.Min(count, Capacity - WritePos);
            Array.Copy(data, 0, buffer, WritePos, len);
            if (len != count)
            {
                var len2 = count - len;
                Array.Copy(data, len, buffer, 0, len2);
            }
            var writepos = (WritePos + count) % Capacity;
            WritePos = writepos;
            Available += count;
        }

        public void Read(T[] data, int count)
        {
            var len = (int)Math.Min(count, Capacity - ReadPos);
            Array.Copy(buffer, ReadPos, data, 0, len);
            if (len != count)
            {
                var len2 = count - len;
                Array.Copy(buffer, 0, data, len, len2);
            }
            var readpos = (ReadPos + count) % Capacity;
            ReadPos = readpos;
            Available -= count;
        }

        public T ReadOne()
        {
            T value = buffer[ReadPos % Capacity];
            ReadPos = (ReadPos + 1) % Capacity;
            Available--;
            return value;
        }

        public void Clear()
        {
            Available = 0;
        }

        public T this[int at]
        {
            get { return buffer[(ReadPos + at) % Capacity]; }
        }
    }
}
