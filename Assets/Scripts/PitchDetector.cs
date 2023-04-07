using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitchDetector
{
    private const int SampleRate = 48000; // Sample rate of the audio signal
    private const int FrameSize = 1024; // Size of each analysis frame in samples
    private const int HopSize = 512; // Hop size between frames in samples
    private const int MaxFrequency = 1000; // Maximum frequency to detect in Hz
    private const int MinPeriod = SampleRate / MaxFrequency; // Minimum period in samples

    // YIN difference function
    private static float[] DifferenceFunction(float[] x, int tauMax)
    {
        //TODO: FIX
        int n = x.Length;
        float[] d = new float[tauMax + 1];

        for (int tau = 0; tau <= tauMax; tau++)
        {
            float acc = 0.0f;
            for (int i = 0; i < n - tau; i++)
            {
                float diff = x[i] - x[i + tau];
                acc += diff * diff;
            }
            d[tau] = acc / (n - tau);
        }

        return d;
    }

    // Estimate the fundamental frequency using the YIN algorithm
    public static float EstimateFundamentalFrequency(float[] x)
    {
        // Step 1: Calculate the YIN difference function
        float[] d = DifferenceFunction(x, MinPeriod);
        
        /* string output = "";
        for (int i = 0; i < 20; i++)
        {
            output += x[i].ToString("F2") + " ";
        }
        Debug.Log(output); */

        // Step 2: Calculate the cumulative mean normalized difference function
        float[] cmd = new float[d.Length];
        cmd[0] = 1.0f;
        for (int tau = 1; tau < cmd.Length; tau++)
        {
            float acc = 0.0f;
            for (int j = 1; j <= tau; j++)
            {
                float diff = d[j] - d[tau];
                acc += diff * diff;
            }
            cmd[tau] = acc / ((tau + 1) * d[0]);
        }

        // Step 3: Find the first minimum of the cumulative mean normalized difference function
        int minIndex = -1;
        float minValue = float.MaxValue;
        for (int tau = MinPeriod; tau < cmd.Length; tau++)
        {
            if (cmd[tau] < minValue)
            {
                minIndex = tau;
                minValue = cmd[tau];
            }
        }

        // Step 4: Calculate the fundamental frequency
        float f0 = minIndex < 1 ? 0.0f : SampleRate / minIndex;
        return f0;
    }
}
