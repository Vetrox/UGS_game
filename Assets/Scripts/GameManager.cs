using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    public static GameManager getInstance()
    {
        return instance;
    }

    private static LevelFile currentLevel;
    private static AudioClip currentSong;

    public static LevelFile GetCurrentLevel()
    {
        return currentLevel;
    }
    public static void SetActiveLevel(LevelFile level)
    {
        currentLevel = level;
        currentSong = Resources.Load<AudioClip>(currentLevel.path);
    }

    public static LevelFile LoadLevel(string id)
    {
        return JsonUtility.FromJson<LevelFile>(
           Resources.Load<TextAsset>("Levels/" + id).text
        );
    }

    public static void PlayCurrentSong()
    {
        var audio = instance.GetComponent<AudioSource>();
        audio.clip = currentSong;
        audio.PlayDelayed(currentLevel.start_offset);
    }

    public static void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
