using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class SliderHandler : MonoBehaviour
{
    private Slider slider;
    void Start()
    {
        slider = GetComponent<Slider>();
        slider.value = GameManager.PersistantSettings.Instance().FPSCap;
    }

    void Update()
    {
        slider.value = GameManager.PersistantSettings.Instance().FPSCap;
    }

    public void OnValueChanged()
    {
        int val = (int)slider.value;
        GameManager.PersistantSettings.Instance().FPSCap = val;
        Application.targetFrameRate = val;
    }
}
