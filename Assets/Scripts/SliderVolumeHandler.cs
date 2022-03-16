using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class SliderVolumeHandler : MonoBehaviour
{
    private Slider slider;
    void Start()
    {
        slider = GetComponent<Slider>();
        slider.value = GameManager.PersistantSettings.Instance().MasterVolume;
    }

    void Update()
    {
        slider.value = GameManager.PersistantSettings.Instance().MasterVolume;
    }

    public void OnValueChanged()
    {
        int val = (int)slider.value;
        GameManager.PersistantSettings.Instance().MasterVolume = val;
        GameManager.getInstance().SetVolume(val);
    }
}
