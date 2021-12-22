using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using NAudio;
using NAudio.Wave;
namespace SR
{
    class Program
    {
        private static string Path = "C:/Users/Matvey/Documents/GitHub/darce Speech-Recognition/SR/SR/";
        static public void DrawWaves(Reader fileData, string dirFile)
        {
            using (var bmp = new Bitmap(600, 400))
            using (var gfx = Graphics.FromImage(bmp))
            using (var pen = new Pen(Color.White))
            {

                int fileLength = fileData.Length * fileData.Channels;
                gfx.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                gfx.Clear(Color.Navy);

                for (int i = 0; i < fileLength - 1; i++)
                {
                    pen.Color = Color.FromArgb(255, Convert.ToInt32(((double)i / (fileLength - 1)) * 255), Convert.ToInt32((((fileLength - 1) - (double)i) / (fileLength - 1)) * 255), 255);
                    var pt1 = new Point(Convert.ToInt32(((double)i / (fileLength - 1)) * bmp.Width), Convert.ToInt32((bmp.Height / 2) * fileData.Data[i]) + bmp.Height / 2);
                    var pt2 = new Point(Convert.ToInt32((((double)i + 1) / (fileLength - 1)) * bmp.Width), Convert.ToInt32((bmp.Height / 2) * fileData.Data[i + 1]) + bmp.Height / 2);
                    gfx.DrawLine(pen, pt1, pt2);
                }
                bmp.Save(dirFile);
            }
        }
        public static int Main(string[] args)
        {
            Reader fileData = new Reader(Path + "samples/5.wav");
            Console.WriteLine($"readerObject.Length = {fileData.Length}");
            int fileLength = fileData.Length * fileData.Channels;
            //Console.WriteLine($"fileLength = {fileLength}");
            Console.WriteLine($"readerObject.Data.Length = {fileData.Data.Length}");

            DrawWaves(fileData, Path + "before.bmp");

            //using (var bmp = new Bitmap(600, 400))
            //using (var gfx = Graphics.FromImage(bmp))
            //using (var pen = new Pen(Color.White))
            //{
            //    gfx.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            //    gfx.Clear(Color.Navy);

            //    for (int i = 0; i < fileLength - 1; i++)
            //    {
            //        pen.Color = Color.FromArgb(255, Convert.ToInt32(((double)i / (fileLength - 1)) * 255), Convert.ToInt32((((fileLength - 1) - (double)i) / (fileLength - 1)) * 255), 255);
            //        var pt1 = new Point(Convert.ToInt32(((double)i / (fileLength - 1)) * bmp.Width), Convert.ToInt32((bmp.Height / 2) * fileData.Data[i]) + bmp.Height / 2);
            //        var pt2 = new Point(Convert.ToInt32((((double)i + 1) / (fileLength - 1)) * bmp.Width), Convert.ToInt32((bmp.Height / 2) * fileData.Data[i + 1]) + bmp.Height / 2);
            //        gfx.DrawLine(pen, pt1, pt2);
            //    }
            //    bmp.Save(Path + "drawing.png");
            //}


            //string resultFile = Path + "result.wav";
            //WaveFormat waveFormat = new WaveFormat(fileData.SampleRate, 16, fileData.Channels);

            //Console.WriteLine($"fileData.SampleRate = {fileData.SampleRate}\nfileData.Channels = {fileData.Channels}");
            
             //int bitsPerSample = 16;
            
             //WaveFormat waveFormat = WaveFormat.CreateCustomFormat(WaveFormatEncoding.Pcm,fileData.SampleRate, fileData.Channels, (fileData.SampleRate * bitsPerSample * fileData.Channels)/8,(bitsPerSample * fileData.Channels)/8,bitsPerSample);
             //WaveFileWriter writer = new WaveFileWriter(resultFile, waveFormat);
            
            //

             WaveFileWriter writer = new WaveFileWriter(Path + "after.wav", fileData.WF);

            //float[] fData = new float[fileLength];
            //for (int i = 0; i < fileLength; i++)
            //    fData[i] = (float)fileData.Data[i];
            //writer.WriteSamples(fData, 0, fileLength);

            //

            double[]? dataChunk;
            float[] fData = new float[fileLength];
            int pos = 0;
            int ChunkSum = 0;
            while (true)
            {
                dataChunk = fileData.Next();
                if (dataChunk == null)
                    break;

                ChunkSum += dataChunk.Length;
                //fData = new float[dataChunk.Length];

                HammingWindow HamWin = new HammingWindow(dataChunk);
                for (int i = 0; i < HamWin.Data.Length; i++)
                    fData[pos++] = (float)HamWin.Data[i];

            }
            Console.WriteLine($"ChunkSum = {ChunkSum}");
            writer.WriteSamples(fData, 0, fData.Length);

            //

            //for (int i = 0; i < fileLength; i++)
            //{

            //    writer.WriteSample((float)fileData.Data[i]);
            //}

            //

            writer.Dispose();
             writer.Close();

            fileData = new Reader(Path + "result.wav");
            DrawWaves(fileData, Path + "after.bmp");

             return 0;
            
        }
    }
}
