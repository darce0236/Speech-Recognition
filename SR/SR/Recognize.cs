using System;

namespace SR
{
    public class Recognize
    {
        private Reader reader;
        private double[] dataStep;

        public Recognize()
        {

        }

        public string Start(string fileName)
        {
            reader = new Reader(fileName);

            double[] data = null;

            string text = "";

           /* while (!reader.isEmppty())
            {
                data = reader.Next();

                //text += LSTM(data);
            }*/



            return text;
        }

        public void Play(string fileName)
        {
            reader = new Reader(fileName);
            reader.Play(fileName);
        }

        public double[] TestRead()
        {
            return reader.Data;
        }

        public double[] TestNext()
        {
            dataStep = reader.Next();
            return dataStep;
        }
        public double[] TestHam()
        {
            HammingWindow hammingWindow = new HammingWindow(dataStep);
            return hammingWindow.Data;
        }


    }
}
