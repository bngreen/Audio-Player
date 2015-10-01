using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AudioPlayer
{
    class Config
    {
        public static int BlockSize { get { return 2048; } }
        public static long BufferSize { get { return BlockSize * 80; } }
    }
}
