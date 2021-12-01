using System;

namespace SR
{
    public class Recognize
    {
        Reader reader;
        public Recognize()
        {

        }

        public string Start(string fileName)
        {
            reader = new Reader(fileName);

            double[] data = null;

            string text = "";

            while (!reader.isEmppty())
            {
                data = reader.Next();

                text += LSTM(data);
            }



            return text;
        }
    }
}
