using System;

namespace SR
{
    public class Recognize
    {
        Reader reader;
        public Recognize()
        {

        }

        public float[] Start(string fileName)
        {
            reader = new Reader(fileName);
            
            return reader.Data;
        }
    }
}
