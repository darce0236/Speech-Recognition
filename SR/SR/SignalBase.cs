using System;
using System.Collections.Generic;
using System.Text;

namespace SR
{
    abstract class SignalBase
    {
        public int _sampleRate { get; set; }
        public int _channels { get; set; }
        public int _length { get; set; }

        public float[] data;

        public abstract void Read();
        public abstract void Reset(string fileName = null);
    }
}
