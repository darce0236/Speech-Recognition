using System;
using System.Collections.Generic;
using System.Text;

namespace SR
{
    class FFT
    {
        protected int _numPoints;
        private float[] _real;
        private float[] _imag;

        public FFT()
        {
            
        }

        public float[] Start(float[] data)
        {
            ComputeFft(data, 16);
            data = GetMagnitudeSquared(1);
            return data;
        }

        private static int ClosestPower(ulong x)
        {
            int power = 2;

            while ((x >>= 1) != 0)
            {
                power <<= 1;
            }

            return power;
        }

        private int ComputeFft(float[] data, int numberOfCoefficients)
        {
            float[] x = data;
            if (!(((uint)data.Length & ((uint)data.Length - 1)) == 0))
            {

                if (numberOfCoefficients < data.Length)
                {
                    numberOfCoefficients = ClosestPower((uint)data.Length);
                }

                x = new float[numberOfCoefficients];
                for (int index = 0; index < data.Length; index++)
                {
                    x[index] = data[index];
                }
            }

            data = x;

            _numPoints = data.Length;

            _real = new float[_numPoints];
            _imag = new float[_numPoints];

            _real = data;

            for (int i = 0; i < _imag.Length; i++)
            {
                _imag[i] = 0;
            }

            Fft();
            return numberOfCoefficients;
        }

        private float[] GetMagnitudeSquared(int scale)
        {
            var ret = new float[_real.Length];
            float energy = 0.0f;

            for (int index = 0; index < _real.Length; index++)
            {
                ret[index] = (_real[index] * _real[index] + _imag[index] * _imag[index]) / scale;

                energy += ret[index];
            }
            return ret;
        }

        private void Fft()
        {
            if (_numPoints == 1) { return; }
            const double pi = Math.PI;
            int numStages = (int)(Math.Log(_numPoints) / Math.Log(2));
            int halfNumPoints = _numPoints >> 1;
            int j = halfNumPoints;


            int k;
            for (int i = 1; i < _numPoints - 2; i++)
            {
                if (i < j)
                {
                    float tempReal = _real[j];
                    float tempImag = _imag[j];
                    _real[j] = _real[i];
                    _imag[j] = _imag[i];
                    _real[i] = tempReal;
                    _imag[i] = tempImag;
                }
                k = halfNumPoints;
                while (k <= j)
                {
                    j -= k;
                    k >>= 1;
                }
                j += k;
            }

            for (int stage = 1; stage <= numStages; stage++)
            {
                int LE = 1;
                for (int i = 0; i < stage; i++)
                {
                    LE <<= 1;
                }
                int LE2 = LE >> 1;
                double UR = 1;
                double UI = 0;

                double SR = Math.Cos(pi / LE2);
                double SI = -Math.Sin(pi / LE2);

                for (int subDFT = 1; subDFT <= LE2; subDFT++)
                {
                    for (int butterfly = subDFT - 1; butterfly <= _numPoints - 1; butterfly += LE)
                    {
                        int ip = butterfly + LE2;

                        float tempReal = _real[ip] * (float)UR - _imag[ip] * (float)UI;
                        float tempImag = _real[ip] * (float)UI + _imag[ip] * (float)UR;
                        _real[ip] = _real[butterfly] - tempReal;
                        _imag[ip] = _imag[butterfly] - tempImag;
                        _real[butterfly] += tempReal;
                        _imag[butterfly] += tempImag;
                    }

                    double tempUR = UR;
                    UR = tempUR * SR - UI * SI;
                    UI = tempUR * SI + UI * SR;
                }
            }
        }
    }
}
