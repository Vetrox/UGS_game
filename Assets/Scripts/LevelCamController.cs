using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCamController : MonoBehaviour
{
    [SerializeField] private Material material;
    static float[] spectrum = new float[1024];
    static float[] reducedSpectrum = new float[3];

    private new AudioSource audio; // FIXME (new)
    void Start()
    {
        audio = GameManager.getInstance().GetComponent<AudioSource>();
    }


    private Vector3 last = Vector3.zero;
    // Update is called once per frame
    void Update()
    {
        if (Camera.current == null) return;
        
        audio.GetSpectrumData(spectrum, 0, FFTWindow.Rectangular);
        ReduceSpectrum();
        // DisplayFFTCurve(0);

        var upperLim = -5;
        var lowerLim = -10;
        var delta = Mathf.Abs(lowerLim - upperLim);

        var mult = GameManager.PersistantSettings.Instance().MasterVolume / 100f; // [0,1]
        var low     = reducedSpectrum[0];
        var mid     = reducedSpectrum[1];
        low /= mult;
        mid /= mult;
        // var high    = reducedSpectrum[2];
        low = Mathf.Log(low); mid = Mathf.Log(mid); 
        // high = Mathf.Log(high);
        low  = Mathf.Clamp(low,  lowerLim, upperLim) - lowerLim;
        mid  = Mathf.Clamp(mid,  lowerLim, upperLim) - lowerLim;

        low /= 3;
        // high = Mathf.Clamp(high, lowerLim, upperLim) - lowerLim;

        Vector3 lowCol = new Vector3(102, 182, 226) / 255;
        Vector3 midCol = new Vector3(17, 71, 196) / 255;

        float interp;
        if (low > mid)
        {
            interp = 0;
        } else
        {
            interp = 1;
        }
        var l = new Vector3(115, 136, 183) / 255;
        if (Mathf.Max(low, mid) / delta > 0.1)
        {
            l = Vector3.Lerp(lowCol, midCol, interp);
        }
        if (l.sqrMagnitude > last.sqrMagnitude)
        {
            l = Vector3.Lerp(last, l, 1f);
        } else
        {
            l = Vector3.Lerp(last, l, 0.5f * Time.deltaTime);
        }
        var col = new Color(l.x, l.y, l.z);
        material.color = col;
        Camera.current.backgroundColor = col;
        last = l;
    }

    void ReduceSpectrum()
    {
        int samplesPerWindow = spectrum.Length / reducedSpectrum.Length;
        int rest = spectrum.Length % reducedSpectrum.Length;
        int specTop = 0;
        for (int i = 0, lastI = -1; i <= spectrum.Length; lastI = i, i += samplesPerWindow + (rest > 0 ? (--rest)*0 + 1 : 0))
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
