using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class OptionMenuCanvasManager : MonoBehaviour
{

    [SerializeField] private Canvas mainCanvas;
    [SerializeField] private Canvas videoCanvas;

    private static Canvas currentCanvas;

    void Start()
    {
        currentCanvas = mainCanvas;
        currentCanvas.enabled = true;

        videoCanvas.enabled = false;
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
}
