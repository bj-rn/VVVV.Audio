﻿#region usings
using System;
using Lomont;
#endregion
namespace VVVV.Audio
{
    public enum FFTWindowFunction
    {
        None,
        Hamming,
        Hann,
        BlackmannHarris
    }
    
    public class FFTOutSignal : SinkSignal
    {
        protected LomontFFT FFFT = new LomontFFT();
        protected CircularBuffer FRingBuffer = new CircularBuffer(512);

        public FFTOutSignal(AudioSignal input)
        {
            InputSignal.Value = input;
        }

        public int Size 
        {
            get 
            {
                return FRingBuffer.Size; 
            }
            set 
            { 
                FRingBuffer.Size = value;
            }
        }
        
        WindowFunction windowFunc;
        
        public WindowFunction WindowFunc 
        {
            get 
            { 
                return windowFunc; 
            }
            set 
            { 
                if(windowFunc != value)
                {
                    windowFunc = value;
                    FWindow = AudioUtils.CreateWindowDouble(FRingBuffer.Size, WindowFunc);
                }
            }
        }

        double[] FFFTBuffer = new double[1];
        public double[] FFTOut = new double[2];
        double[] FWindow = new double[1];

        protected override void FillBuffer(float[] buffer, int offset, int count)
        {
            if (InputSignal.Value != null) 
            {
                InputSignal.Read(buffer, offset, count);
                
                //write to buffer
                FRingBuffer.Write(buffer, offset, count);
                
                //calc fft
                var fftSize = FRingBuffer.Size;
                
                if (FFFTBuffer.Length != fftSize)
                {
                    FFFTBuffer = new double[fftSize];
                    FFTOut = new double[fftSize];
                    FWindow = AudioUtils.CreateWindowDouble(fftSize, WindowFunc);
                }
            
                FRingBuffer.ReadDoubleWindowed(FFFTBuffer, FWindow, 0, fftSize);
                FFFT.RealFFT(FFFTBuffer, true);
                Array.Copy(FFFTBuffer, FFTOut, fftSize);
            }
        }
    }
}




