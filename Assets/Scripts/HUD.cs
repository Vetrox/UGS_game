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

    void Start()
    {
        levelText.text = GameManager.GetCurrentLevel().displayName;
    }

    void Update()
    {
        float percentage = GameManager.GetCurrentLevelPercentage();
        slider.value = percentage;
        sliderText.text = percentage.ToString("0.0", CultureInfo.InvariantCulture) + "%";
    }
}
