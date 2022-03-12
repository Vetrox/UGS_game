using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(InputField))]
public class TextInputHandler : MonoBehaviour
{
    private InputField textField;
    void Start()
    {
        textField = GetComponent<InputField>();
        textField.text = GameManager.PersistantSettings.Instance().FPSCap.ToString();
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
            Application.targetFrameRate = newFPS;
        } catch (System.FormatException) {}
    }
}
