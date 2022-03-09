using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SongListView : MonoBehaviour
{

    [SerializeField] private Transform m_ContentContainer;
    [SerializeField] private Button m_ItemPrefab;
    [SerializeField] private int m_ItemsToGenerate;

    void Start()
    {
        string cwd = Directory.GetCurrentDirectory();
        char sep = Path.DirectorySeparatorChar;
        string musicDir = $"{cwd}{sep}Assets{sep}Resources{sep}Levels{sep}";

        string[] files = Directory.GetFiles(musicDir);
        foreach (string file in files)
        {
            string name = file.Substring(musicDir.Length);
            if (name.EndsWith(".json"))
            {
                print("music: " + name);
                addLabel(name);
            } else
            {
                print("other: " + name);
            }
        }
    }

    void addLabel(string text)
    {
        var item_go = Instantiate(m_ItemPrefab);
        item_go.GetComponentInChildren<Text>().text = text;
        item_go.transform.SetParent(m_ContentContainer);
        item_go.transform.localScale = Vector2.one;
    }
}
