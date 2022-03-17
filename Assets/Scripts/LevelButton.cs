using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public LevelFile levelFile;
    public Text songInfoText;

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        var percentage = GameManager.PersistantSettings.Instance()
            .highScores.Find((pair) => pair.id.Equals(levelFile.id)).percentage
            .ToString("0.0", CultureInfo.InvariantCulture);

        songInfoText.text = $"{levelFile.displayName} by {levelFile.interpret}\n" +
            $"{levelFile.bpm} bpm\n" +
            $"High Score: {percentage}%";
        GameManager.SetActiveLevel(levelFile);
        GameManager.PlayCurrentSong(0);
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        songInfoText.text = "";
        GameManager.StopCurrentSong();
    }

}
