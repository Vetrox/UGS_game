using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
[RequireComponent(typeof(AudioSource))]
public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;
    private static AudioSource audioSource = null;

    private static bool paused = false; // only used in Player

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
        // QualitySettings.vSyncCount = 1;
        Application.targetFrameRate = 60;
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

    public static void ReloadLevel()
    {
        var activeScene = SceneManager.GetActiveScene();
        if (activeScene.name.Equals("LevelScene"))
        {
            paused = false;
            SceneManager.LoadScene(activeScene.buildIndex);
        }
        
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

    public static void ResumeLevel()
    {
        ResumePhysics();
        ResumeCurrentSong();
        SceneManager.UnloadSceneAsync("PauseMenu");
        paused = false;
    }

    public static void ExitLevel()
    {
        ResumePhysics();
        StopCurrentSong();
        for (int i = 0; i < SceneManager.sceneCount; ++i)
        {
            var scene = SceneManager.GetSceneAt(i);
            if (scene.name.Equals("PauseMenu"))
            {
                SceneManager.UnloadSceneAsync("PauseMenu");
                break;
            }
        }
        paused = false;
        LoadLevelSelect();
    }

    public static void PauseLevel()
    {
        PausePhysics();
        PauseCurrentSong();
        SceneManager.LoadScene("PauseMenu", LoadSceneMode.Additive);
        paused = true;
    }

    public static bool IsPaused()
    {
        return paused;
    }

    public static void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    public static void LoadOptionsAdditive()
    {
        SceneManager.LoadScene("OptionsMenu", LoadSceneMode.Additive);
    }

    public static void UnloadOptionsMenu()
    {
        SceneManager.UnloadSceneAsync("OptionsMenu");
    }
}
