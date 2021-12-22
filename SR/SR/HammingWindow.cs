using System;
using System.Collections.Generic;
using System.Text;

namespace SR
{
    class HammingWindow
    {
        private readonly double[] _values;
        private float alpha = 0.54f;
        private float beta = 0.46f;

        public HammingWindow(int length)
        {
            var phaseHammingPart = 2 * Math.PI / (length - 1);

            _values = new double[length];

            for (int index = 0; index < length; index++)
            {
                _values[index] = alpha - (beta * Math.Cos(phaseHammingPart * index));
            }
        }

        /*private void CreateWindow()
        {
            double[] hamming = MathNet.Numerics.Window.Hamming(_data.Length);
            for (int i = 0; i < _data.Length; i++)
                _data[i] *= (float)hamming[i];
        }*/

        public float[] Apply(float[] data)
        {
            for (int index = 0; index < data.Length; index++)
            {
                data[index] = data[index] * (float)this[index];
            }

            return data;
        }

        public double this[int i]
        {
            get { return _values[i]; }
        }
            
    }
}
