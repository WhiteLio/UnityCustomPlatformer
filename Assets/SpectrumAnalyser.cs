using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpectrumAnalyser : MonoBehaviour
{
    void Update()
    {
        float[] spectrum = new float[256];
        float[] segments = new float[7];
        AudioListener.GetSpectrumData(spectrum, 0, FFTWindow.Rectangular );

        for (int i = 1; i < spectrum.Length - 1; i++)
        {
            Debug.DrawLine(new Vector3(i - 1, spectrum[i] + 10, 0), new Vector3(i, spectrum[i + 1] + 10, 0), Color.red);
            Debug.DrawLine(new Vector3(i - 1, Mathf.Log(spectrum[i - 1]) + 10, 2), new Vector3(i, Mathf.Log(spectrum[i]) + 10, 2), Color.cyan);
            Debug.DrawLine(new Vector3(Mathf.Log(i - 1), spectrum[i - 1] - 10, 1), new Vector3(Mathf.Log(i), spectrum[i] - 10, 1), Color.green);
            Debug.DrawLine(new Vector3(Mathf.Log(i - 1), Mathf.Log(spectrum[i - 1]), 3), new Vector3(Mathf.Log(i), Mathf.Log(spectrum[i]), 3), Color.blue);
        }

        /* segments[0] = spectrum [0] + spectrum [2] + spectrum [4];
        segments[1] = spectrum [10] + spectrum [11] + spectrum [12];
        segments[2] = spectrum[20] + spectrum [21] + spectrum [22];
        segments[3] = spectrum [40] + spectrum [41] + spectrum [42] + spectrum [43];
        segments[4] = spectrum [80] + spectrum [81] + spectrum [82] + spectrum [83];
        segments[5] = spectrum [160] + spectrum [161] + spectrum [162] + spectrum [163];
        segments[6] = spectrum [320] + spectrum [321] + spectrum [322] + spectrum [323];
        
        string output = "";
        foreach(float segment in segments)
        {
            output += segment.ToString("F2") + " ";
        }
        print(output); */
        
        
        /* GameObject [] cubes = GameObject.FindGameObjectsWithTag("CUBE");

        for( int i = 1; i < cubes.Length; i++ )
        {
            switch (i)
            {
                case 1:
                    cubes[i].gameObject.transform.localScale = new Vector3(1, l1 * 100, 0.5f); // base drum
                    break;
                case 2:
                    cubes[i].gameObject.transform.localScale = new Vector3(1, l2 * 200, 0.5f); // base guitar
                    break;
                case 3:
                    cubes[i].gameObject.transform.localScale = new Vector3(1, l3 * 400, 0.5f);
                    break;
                case 4:
                    cubes[i].gameObject.transform.localScale = new Vector3(1, l4 * 800, 0.5f);
                    break;
                case 5:
                    cubes[i].gameObject.transform.localScale = new Vector3(1, l5 * 1600, 0.5f);
                    break;
                case 6:
                    cubes[i].gameObject.transform.localScale = new Vector3(1, l6 * 3200, 0.5f);
                    break;
                case 7:
                    cubes[i].gameObject.transform.localScale = new Vector3(1, l7 * 6400, 0.5f); //*tsk tsk tsk
                    break;
            }           
        } */
    }
}
