using System;
using System.Collections.Generic;
using System.Text;

namespace SR
{
    class HammingWindow
    {
        private float[] _data;
        public HammingWindow(float[] data)
        {
            _data = data;
            CreateWindow();
        }

        private void CreateWindow()
        {
            double[] hamming = MathNet.Numerics.Window.Hamming(_data.Length);
            for (int i = 0; i < _data.Length; i++)
                _data[i] *= (float)hamming[i];
        }

        public float[] Data => _data;
    }
}
