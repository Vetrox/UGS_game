using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
[RequireComponent(typeof(AudioSource))]
public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;
    private static AudioSource audioSource = null;

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

    void Start()
    {
        audioSource = instance.GetComponent<AudioSource>();
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
        audioSource.clip = currentSong;
        audioSource.PlayDelayed(currentLevel.start_offset);
    }

    public static void PauseCurrentSong()
    {
        audioSource.Pause();
    }

    public static void ResumeCurrentSong()
    {
        audioSource.Play();
    }

    public static void StopCurrentSong()
    {
        audioSource.Stop();
    }

    public static void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public static void LoadLevelSelect()
    { 
        SceneManager.LoadScene("LevelSelectMenu");
    }

    public static void PausePhysics()
    {
        Time.timeScale = 0;
    }

    public static void ResumePhysics()
    {
        Time.timeScale = 1;
    }

    public static void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
