using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class OptionMenuCanvasManager : MonoBehaviour
{

    [SerializeField] private Canvas mainCanvas;
    [SerializeField] private Canvas videoCanvas;
    [SerializeField] private Canvas audioCanvas;

    private static Canvas currentCanvas;

    void Start()
    {
        currentCanvas = mainCanvas;
        currentCanvas.enabled = true;

        videoCanvas.enabled = false;
        audioCanvas.enabled = false;
    }

    public void DisplayVideoSettigs()
    {
        currentCanvas.enabled = false;
        videoCanvas.enabled = true;
        currentCanvas = videoCanvas;
    }

    public void DisplayMainCanvas()
    {
        currentCanvas.enabled = false;
        mainCanvas.enabled = true;
        currentCanvas = mainCanvas;
    }

    public void DisplayAudioSettings()
    {
        currentCanvas.enabled = false;
        audioCanvas.enabled = true;
        currentCanvas = audioCanvas;
    }
}
