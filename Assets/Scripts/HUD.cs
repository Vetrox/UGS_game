using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{

    [SerializeField] private Text levelText;
    void Start()
    {
        levelText.text = GameManager.GetCurrentLevel().displayName;
    }
}
