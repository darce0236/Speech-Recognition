using System;
using System.Collections.Generic;
using System.Text;

namespace SR
{
    class HammingWindow
    {
        private double[] _data;
        public HammingWindow(double[] data)
        {
            _data = data;
            CreateWindow();
        }

        private void CreateWindow()
        {
            double[] hamming = MathNet.Numerics.Window.Hamming(_data.Length);
            for (int i = 0; i < _data.Length; i++)
                _data[i] *= hamming[i];
        }

        public double[] Data => _data;
    }
}
