using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class VSyncButton : MonoBehaviour
{
    private Toggle toggle;
    void Start()
    {
        toggle = GetComponent<Toggle>();
        toggle.isOn = GameManager.PersistantSettings.Instance().VSyncEnabled;
    }

    public void OnValueChanged()
    {
        GameManager.PersistantSettings.Instance().VSyncEnabled = toggle.isOn;
        QualitySettings.vSyncCount = toggle.isOn ? 1 : 0;
    }
}
