using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCamController : MonoBehaviour
{

    static float[] spectrum = new float[1024];
    static float[] reducedSpectrum = new float[4];


    // Update is called once per frame
    void Update()
    {
        if (Camera.current == null) return;
        var audio = GameManager.getInstance().GetComponent<AudioSource>();
        audio.GetSpectrumData(spectrum, 0, FFTWindow.Rectangular);
        ReduceSpectrum();
        // DisplayFFTCurve(0);

        float upperLim = -5;
        float lowerLim = -10;
        float delta = Mathf.Abs(lowerLim - upperLim);
        float r = reducedSpectrum[0];
        float g = reducedSpectrum[1];
        float b = reducedSpectrum[2];
        r = Mathf.Log(r);
        g = Mathf.Log(g);
        b = Mathf.Log(b);
        r = Mathf.Clamp(r, lowerLim, upperLim) - lowerLim;
        g = Mathf.Clamp(g, lowerLim, upperLim) - lowerLim;
        b = Mathf.Clamp(b, lowerLim, upperLim) - lowerLim;

        
        r /= delta;
        g /= delta;
        b /= delta;

        Camera.current.backgroundColor = new Color(r, g, b);
    }

    void ReduceSpectrum()
    {
        int samplesPerWindow = spectrum.Length / reducedSpectrum.Length;
        int rest = spectrum.Length % reducedSpectrum.Length;
        int specTop = 0;
        for (int i = 0, lastI = -1; i < spectrum.Length; lastI = i, i += samplesPerWindow + (rest > 0 ? (--rest)*0 + 1 : 0))
        {
            if (lastI < 0) continue;
            reducedSpectrum[specTop++] = Average(spectrum, lastI, i);
        }
    }

    float Average(float[] arr, int start, int end)
    {
        float sum = 0;
        for (int i = start; i < end; i++) sum += arr[i];
        return sum / arr.Length;
    }

    void DisplayFFTCurve(int offset)
    {
        for (int i = 1; i < reducedSpectrum.Length; i++)
        {
            Debug.DrawLine(
                new Vector3(
                    i - 1,
                    Mathf.Log(reducedSpectrum[i - 1]),
                    0),
                new Vector3(
                    i,
                    Mathf.Log(reducedSpectrum[i]),
                    0),
                Color.cyan);
        }
    }
}
