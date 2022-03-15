using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
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
    private float gameplayDelay = 2;
    void Start()
    {
        levelText.text = GameManager.GetCurrentLevel().displayName;
        var musicDelay = gameplayDelay - GameManager.GetCurrentLevel().start_offset;
        GameManager.PlayCurrentSong(musicDelay);
        start = Time.realtimeSinceStartup;
        _ = delayedWork();
    }

    private async Task delayedWork()
    {
        await Task.Delay((int)(gameplayDelay * 1000));
        DoMyDelayedWork();
    }

    private void DoMyDelayedWork()
    {
        timeToStart.text = "";
        started = true;
        GameManager.StartGameplay();
    }

    void Update()
    {
        float percentage = GameManager.GetCurrentLevelPercentage();
        slider.value = percentage;
        sliderText.text = percentage.ToString("0.0", CultureInfo.InvariantCulture) + "%";

        if (!started)
        {
            var tts = -gameplayDelay + (Time.realtimeSinceStartup - start);
            if (tts < 0)
            {
                timeToStart.text = tts.ToString("0.00", CultureInfo.InvariantCulture);
            }
        }

    }
}
