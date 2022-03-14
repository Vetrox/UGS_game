using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;


public class HUD : MonoBehaviour
{
    [SerializeField] private Text   levelText;
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
            var now = Time.realtimeSinceStartup;
            if (now - start > 2)
            {
                GameManager.StartGameplay();
                started = true;
            }
        }

    }
}
