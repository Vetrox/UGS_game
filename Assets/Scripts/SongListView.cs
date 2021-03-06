using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class SongListView : MonoBehaviour
{

    [SerializeField] private Transform m_ContentContainer;
    [SerializeField] private GameObject m_ItemPrefab;
    [SerializeField] private Text m_songInfoText;

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
                name = name.Substring(0, name.Length - ".json".Length);
                setupEntry(name);
            }
        }
    }

    void setupEntry(string id)
    {
        var level = GameManager.LoadLevelFile(id);

        var item_go = Instantiate(m_ItemPrefab);
        item_go.GetComponentInChildren<Text>().text = level.displayName;
        item_go.transform.SetParent(m_ContentContainer);
        item_go.transform.localScale = Vector2.one;

        LevelButton btn = item_go.GetComponent<LevelButton>();
        btn.levelFile = level;
        btn.songInfoText = m_songInfoText;

        item_go.GetComponent<Button>().onClick
            .AddListener(new UnityAction(() =>
            {
                GameManager.SetActiveLevel(level);
                GameManager.LoadLevel();
            }));
    }
}
