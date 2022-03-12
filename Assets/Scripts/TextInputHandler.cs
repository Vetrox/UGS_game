using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(InputField))]
public class TextInputHandler : MonoBehaviour
{
    private InputField textField;
    void Start()
    {
        textField = GetComponent<InputField>();
    }

    void Update()
    {
        if (!textField.isFocused)
            textField.text = GameManager.PersistantSettings.Instance().FPSCap.ToString();
    }

    public void OnEndEdit()
    {
        try {
            int newFPS = int.Parse(textField.text);
            GameManager.PersistantSettings.Instance().FPSCap = newFPS;
            print("Set the FPS value to " + newFPS);
        } catch (System.FormatException) {}
    }
}
