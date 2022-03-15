using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public LevelFile levelFile;
    public Text songInfoText;

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        songInfoText.text = $"{levelFile.displayName}\n" +
            $"{levelFile.bpm}";
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        songInfoText.text = "";
    }

}
