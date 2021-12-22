using System;
using System.Collections.Generic;
using System.Text;

namespace SR
{
    class FFT
    {
        float[] data;

        /**
	 * number of points
	 */
        protected int _numPoints;
        /**
         * real part
         */
        private float[] _real;
        /**
         * imaginary part
         */
        private float[] _imag;

        public FFT()
        {
            
        }

        public float[] Start(float[] data)
        {
            ComputeFft(data, 16);
            float energy;
            data = GetMagnitudeSquared(1, out energy);
            return data;
        }

        public static int ClosestPower(ulong x)
        {
            int power = 2;

            while ((x >>= 1) != 0)
            {
                power <<= 1;
            }

            return power;
        }

        public static bool IsPowerOfTwo(ulong x)
        {
            return (x & (x - 1)) == 0;
        }

        public int ComputeFft(float[] signal, int numberOfCoefficients)
        {
            float[] x = signal;
            if (!IsPowerOfTwo((uint)signal.Length))
            {

                if (numberOfCoefficients < signal.Length)
                {
                    numberOfCoefficients = ClosestPower((uint)signal.Length);
                }

                x = new float[numberOfCoefficients];
                for (int index = 0; index < signal.Length; index++)
                {
                    x[index] = signal[index];
                }
            }

            signal = x;

            _numPoints = signal.Length;
            // initialize real & imag array
            _real = new float[_numPoints];
            _imag = new float[_numPoints];
            // move the N point signal into the real part of the complex DFT's time
            // domain
            _real = signal;
            // set all of the samples in the imaginary part to zero
            for (int i = 0; i < _imag.Length; i++)
            {
                _imag[i] = 0;
            }
            // perform FFT using the real & imag array
            Fft();
            return numberOfCoefficients;
        }

        public float[] GetMagnitudeSquared(int scale, out float energy)
        {
            var ret = new float[_real.Length];
            energy = 0.0f;

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

            // FFT time domain decomposition carried out by "bit reversal sorting"
            // algorithm
            int k;
            for (int i = 1; i < _numPoints - 2; i++)
            {
                if (i < j)
                {
                    // swap
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

            // loop for each stage
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
                // calculate sine & cosine values
                double SR = Math.Cos(pi / LE2);
                double SI = -Math.Sin(pi / LE2);
                // loop for each sub DFT
                for (int subDFT = 1; subDFT <= LE2; subDFT++)
                {
                    // loop for each butterfly
                    for (int butterfly = subDFT - 1; butterfly <= _numPoints - 1; butterfly += LE)
                    {
                        int ip = butterfly + LE2;
                        // butterfly calculation
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
