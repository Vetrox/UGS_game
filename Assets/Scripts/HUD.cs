using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;


public class HUD : MonoBehaviour
{
    [SerializeField] private Text   levelText;
    [SerializeField] private Text   timeToStart;
    [SerializeField] private Text   sliderText;
    [SerializeField] private Slider slider;

    private float start;
    private bool started = false;

    void Start()
    {
        levelText.text = GameManager.GetCurrentLevel().displayName;
        start = Time.realtimeSinceStartup;
    }

    void Update()
    {
        float percentage = GameManager.GetCurrentLevelPercentage();
        slider.value = percentage;
        sliderText.text = percentage.ToString("0.0", CultureInfo.InvariantCulture) + "%";

        if (!started)
        {
            var tts = -2 + (Time.realtimeSinceStartup - start);
            if (tts >= 0)
            {
                GameManager.StartGameplay();
                timeToStart.text = "";
                started = true;
            } else
            {
                timeToStart.text = tts.ToString("0.00", CultureInfo.InvariantCulture);
            }
        }

    }
}
