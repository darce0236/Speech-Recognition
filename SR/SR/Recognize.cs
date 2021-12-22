using System;

namespace SR
{
    public class Recognize
    {
        private Reader reader;
        private float[] dataStep;

        public Recognize()
        {

        }

        public string Start(string fileName)
        {
            reader = new Reader(fileName);

            float[] data = null;

            string text = "";

           while (!reader.isEmppty())
            {
                data = reader.Next();

                HammingWindow hamming = new HammingWindow(data.Length);
                data = hamming.Apply(data);

                FFT fft = new FFT();
            }



            return text;
        }

        public void Play(string fileName)
        {
            reader = new Reader(fileName);
            reader.Play(fileName);
        }

        public float[] TestRead()
        {
            return reader.Data;
        }

        public float[] TestNext()
        {
            dataStep = reader.Next();
            return dataStep;
        }
        public float[] TestHam()
        {
            HammingWindow hammingWindow = new HammingWindow(dataStep.Length);
            dataStep = hammingWindow.Apply(dataStep);
            return dataStep;
        }

        public float[] TestFFT()
        {
            FFT fft = new FFT();
            dataStep = fft.Start(dataStep);
            return dataStep;
        }


    }
}
