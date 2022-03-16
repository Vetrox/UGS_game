using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(InputField))]
public class VolumeTextInputHandler : MonoBehaviour
{
    private InputField textField;
    void Start()
    {
        textField = GetComponent<InputField>();
        textField.text = GameManager.PersistantSettings.Instance().MasterVolume.ToString();
    }

    void Update()
    {
        if (!textField.isFocused)
            textField.text = GameManager.PersistantSettings.Instance().MasterVolume.ToString();
    }

    public void OnEndEdit()
    {
        try
        {
            int newVolume = int.Parse(textField.text);
            if (newVolume < 1 || newVolume > 100) return;
            GameManager.PersistantSettings.Instance().MasterVolume = newVolume;
            GameManager.getInstance().SetVolume(newVolume);
        }
        catch (System.FormatException) { }
    }
}
