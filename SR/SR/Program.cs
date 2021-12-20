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
        private static string Path = "C:/Users/Matvey/Documents/GitHub/Speech-Recognition/SR/SR/";
        public static int Main(string[] args)
        {
            Reader fileData = new Reader(Path + "samples/3.wav");
            Console.WriteLine($"fileData.Length = {fileData.Length}");
            using (var bmp = new Bitmap(600, 400))
            using (var gfx = Graphics.FromImage(bmp))
            using (var pen = new Pen(Color.White))
            {
                gfx.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                gfx.Clear(Color.Navy);

                for (int i = 0; i < fileData.Length-1; i++)
                {
                    pen.Color = Color.FromArgb(255, Convert.ToInt32(((double)i / (fileData.Length-1)) * 255), Convert.ToInt32((((fileData.Length-1) - (double)i) / (fileData.Length-1)) * 255),255);
                    var pt1 = new Point(Convert.ToInt32(((double)i /(fileData.Length-1))*bmp.Width), Convert.ToInt32((bmp.Height/2) * fileData.Data[i]) + bmp.Height/2);
                    var pt2 = new Point(Convert.ToInt32((((double)i + 1) / (fileData.Length-1)) * bmp.Width), Convert.ToInt32((bmp.Height / 2) * fileData.Data[i+1]) + bmp.Height/2);
                    gfx.DrawLine(pen, pt1, pt2);
                }
                bmp.Save(Path + "drawing.png");

                //string resultFile = Path + "result.wav";
                //WaveFormat waveFormat = new WaveFormat(fileData.SampleRate, 16, fileData.Channels);

                Console.WriteLine($"fileData.SampleRate = {fileData.SampleRate}\nfileData.Channels = {fileData.Channels}");

                //int bitsPerSample = 16;

                //WaveFormat waveFormat = WaveFormat.CreateCustomFormat(WaveFormatEncoding.Pcm,fileData.SampleRate, fileData.Channels, (fileData.SampleRate * bitsPerSample * fileData.Channels)/8,(bitsPerSample * fileData.Channels)/8,bitsPerSample);
                //WaveFileWriter writer = new WaveFileWriter(resultFile, waveFormat);
                
                WaveFileWriter writer = new WaveFileWriter(Path + "result.wav", fileData.WF);

                writer.WriteSamples(fileData.Data, 0, fileData.Length);
                //for (int i = 0; i < fileData.Length; i++)
                //{
                    
                //    writer.WriteSample((float)fileData.Data[i]);
                //}
                writer.Dispose();
                writer.Close();
                return 0;
            }
        }
    }
}
