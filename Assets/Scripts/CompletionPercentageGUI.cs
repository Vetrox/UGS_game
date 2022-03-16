using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class CompletionPercentageGUI : MonoBehaviour
{
    private Text perc;
    void Start()
    {
        perc = GetComponent<Text>();
        perc.text = "Completion:\n" + GameManager.GetCurrentLevelPercentage().ToString("0.0", CultureInfo.InvariantCulture) + "%";
    }
}
