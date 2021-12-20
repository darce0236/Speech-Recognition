﻿using System;
using System.Collections.Generic;
using System.Text;
using NAudio;
using NAudio.Wave;

namespace SR
{
    class Reader : SignalBase
    {
        WaveFileReader _reader;
        private int position;
        private int step;

        public Reader(string fileName)
        {
            _reader = new WaveFileReader(fileName);
            _sampleRate = _reader.WaveFormat.SampleRate;
            _channels = _reader.WaveFormat.Channels;
            _length = (int)(_reader.SampleCount / _channels);
            _wF = _reader.WaveFormat;

            Read();
        }

        public override void Read()
        {
            byte[] wave = new byte[_reader.Length];
            data = new float[(wave.Length - 44) / 2];
            _reader.Read(wave, 0, Convert.ToInt32(_reader.Length));

            float i = 0;

            for (i = 0; i < data.Length; i++)
                data[(int)i] = BitConverter.ToInt16(wave, 44 + (int)i * 2) / 65536.0f;

            position = 0;
            step = (int)(20.0 / (1000.0 / SampleRate));
        }

        public bool isEmppty()
        {
            if (position >= Length)
                return true;
            return false;
        }

        public float[] Next()
        {
            if (isEmppty())
                return null;

            if (position + step >= Length)
                step = Length - position;

            float[] dataStep = new float[step];

            for (int i = 0; i < dataStep.Length; i++)
                dataStep[i] = data[position + i];

            HammingWindow hamming = new HammingWindow(dataStep);

            position += step;

            return hamming.Data;
        }

        public int SampleRate => _sampleRate;
        public int Channels => _channels;
        public int Length => _length;
        public float[] Data => data;
        public WaveFormat WF => _wF;

        public override void Reset(string fileName)
        {
            data = null;
            _reader = new WaveFileReader(fileName);

            Read();
        }
    }
}
