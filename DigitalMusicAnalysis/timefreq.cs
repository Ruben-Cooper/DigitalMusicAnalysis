using System;
using System.Numerics;
using System.Threading.Tasks;
using System.Threading;

namespace DigitalMusicAnalysis
{
    public class timefreq
    {
        private ParallelOptions parallelOptions;
        public float[][] timeFreqData;
        public int wSamp;

        public timefreq(float[] x, int windowSamp)
        {
            this.wSamp = windowSamp;

            int nearest = (int)Math.Ceiling((double)x.Length / (double)wSamp);
            nearest = nearest * wSamp;

            Complex[] compX = new Complex[nearest];
            for (int kk = 0; kk < nearest; kk++)
            {
                if (kk < x.Length)
                {
                    compX[kk] = x[kk];
                }
                else
                {
                    compX[kk] = Complex.Zero;
                }
            }

            timeFreqData = stft(compX, wSamp);
        }

        float[][] stft(Complex[] x, int wSamp)
        {
            int N = x.Length;
            int numWindows = 2 * (int)Math.Floor((double)N / (double)wSamp) - 1;
            float[][] Y = new float[wSamp / 2][];
            for (int ll = 0; ll < wSamp / 2; ll++)
            {
                Y[ll] = new float[numWindows];
            }

            float fftMax = 0;
            parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = 8 };
            Parallel.For(0, numWindows, parallelOptions, ii =>
            {
                Complex[] temp = new Complex[wSamp];
                Array.Copy(x, ii * (wSamp / 2), temp, 0, wSamp);

                Complex[] tempFFT = fft(temp);

                float localMax = 0;
                for (int kk = 0; kk < wSamp / 2; kk++)
                {
                    float magnitude = (float)Complex.Abs(tempFFT[kk]);
                    Y[kk][ii] = magnitude;

                    if (magnitude > localMax)
                    {
                        localMax = magnitude;
                    }
                }

                // Update fftMax safely
                float initialMax, computedMax;
                do
                {
                    initialMax = fftMax;
                    computedMax = Math.Max(initialMax, localMax);
                }
                while (initialMax != Interlocked.CompareExchange(ref fftMax, computedMax, initialMax));
            });

            // Normalize the FFT results
            for (int ii = 0; ii < numWindows; ii++)
            {
                for (int kk = 0; kk < wSamp / 2; kk++)
                {
                    Y[kk][ii] /= fftMax;
                }
            }

            return Y;
        }

        Complex[] fft(Complex[] x)
        {
            int N = x.Length;
            if (N <= 1)
                return new Complex[] { x[0] };

            // Divide
            int halfN = N / 2;
            Complex[] even = new Complex[halfN];
            Complex[] odd = new Complex[halfN];

            for (int i = 0; i < halfN; i++)
            {
                even[i] = x[2 * i];
                odd[i] = x[2 * i + 1];
            }

            // Conquer
            Complex[] E = fft(even);
            Complex[] O = fft(odd);

            // Combine
            Complex[] Y = new Complex[N];
            for (int k = 0; k < halfN; k++)
            {
                double angle = -2 * Math.PI * k / N;
                Complex twiddle = new Complex(Math.Cos(angle), Math.Sin(angle)) * O[k];
                Y[k] = E[k] + twiddle;
                Y[k + halfN] = E[k] - twiddle;
            }
            return Y;
        }
    }
}
