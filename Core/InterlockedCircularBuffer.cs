using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Core
{
    public class InterlockedCircularBuffer<T>
    {
        private T[] buffer;
        public long Capacity { get; private set; }
        private long readPos = 0;
        private long writePos = 0;
        private long available = 0;
        private long WritePos { get { return Interlocked.Read(ref writePos) % Capacity; } }
        private long ReadPos { get { return Interlocked.Read(ref readPos) % Capacity; } }
        public int Available { get { return (int)Interlocked.Read(ref available); } }
        public event EventHandler BufferLowEvent;
        public InterlockedCircularBuffer(long Capacity)
        {
            buffer = new T[Capacity];
            this.Capacity = Capacity;
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
            Interlocked.Add(ref writePos, count);
            Interlocked.Add(ref available, count);
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
            if (BufferLowEvent != null && Interlocked.Read(ref available) < Capacity / 2)
                BufferLowEvent.Invoke(this, null);
            Interlocked.Add(ref readPos, count);
            Interlocked.Add(ref available, -count);
        }

        public void Clear()
        {
            Interlocked.Add(ref available, -Available);
            Interlocked.Add(ref writePos, -WritePos);
            Interlocked.Add(ref readPos, -ReadPos);
        }
    }
}
