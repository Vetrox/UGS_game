using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class GameManager : MonoBehaviour
{
    [System.Serializable]
    public struct Pair
    {
        public string id;
        public float percentage;

        public Pair(string id, float percentage)
        {
            this.id = id;
            this.percentage = percentage;
        }
    }
    [System.Serializable]
    public class PersistantSettings
    {
        public int   FPSCap          = 60;
        public bool  VSyncEnabled    = false;

        public List<Pair> highScores = new List<Pair>(); // level.id -> percentage completion

        private PersistantSettings() { }
        [System.NonSerialized]
        private static PersistantSettings instance;
        [System.NonSerialized]
        private static readonly string path = Path.Combine(Application.persistentDataPath, "usersettings.json");

        public static PersistantSettings Instance()
        {
            if (instance == null)
                DefaultOrInitFromDisk();
            return instance;
        }

        public static void SaveToDisk()
        {
            var json = JsonUtility.ToJson(Instance(), true);
            File.WriteAllText(path, json);
        }

        private static void DefaultOrInitFromDisk()
        {
            if (instance != null) return;
            try {
                var settingsFile = File.ReadAllText(path);
                var loadedSettings = JsonUtility.FromJson<PersistantSettings>(settingsFile);
                instance = loadedSettings;
            } catch (System.Exception) {
                instance = new PersistantSettings();
            }
        }
    }

    private static GameManager instance = null;
    private static AudioSource audioSource = null;

    private static bool paused = false; // true, when the music and the game is paused
    public static bool gameOver = false; // true, when we are about to display the gameoverscreen but the level is still loading.

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

    void OnApplicationQuit()
    {
        PersistantSettings.SaveToDisk();
    }

    void Start()
    {
        audioSource = instance.GetComponent<AudioSource>();
        QualitySettings.vSyncCount = PersistantSettings.Instance().VSyncEnabled ? 1 : 0;
        Application.targetFrameRate = PersistantSettings.Instance().FPSCap;
    }

    void Update()
    {
        if (audioSource && audioSource.clip && !IsPaused() && !gameOver)
            lastPercentage = audioSource.time * 100 / audioSource.clip.length;
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

    public static LevelFile LoadLevelFile(string id)
    {
        return JsonUtility.FromJson<LevelFile>(
           Resources.Load<TextAsset>("Levels/" + id).text
        );
    }

    private static float lastPercentage = 0;
    public static float GetCurrentLevelPercentage()
    {
        return lastPercentage;
    }


    public static void PlayCurrentSong(float offset)
    {
        audioSource.clip = currentSong;
        audioSource.PlayDelayed(offset);
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
        if (audioSource)
            audioSource.Stop();
    }

    public static void LoadLevel()
    {
        PrepareLevel();
        SceneManager.LoadScene("LevelScene"); 
    }

    private static void PrepareLevel()
    {
        if (audioSource)
        {
            audioSource.Stop();
            audioSource.clip = null;
            audioSource.time = 0;
            audioSource.timeSamples = 0;
        }
        paused = true;
        gameOver = true;
        lastPercentage = 0;
        PausePhysics();
    }

    public static void StartGameplay()
    {
        ResumePhysics();
        paused = false;
        gameOver = false;
        lastPercentage = 0;
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

    public static void ToggleFullscreen()
    {
        var res = Screen.currentResolution;
        if (Screen.fullScreen) {
            Screen.SetResolution(res.width, res.height, FullScreenMode.Windowed);
        } else {
            Screen.SetResolution(res.width, res.height, FullScreenMode.MaximizedWindow);
        }
    }

    public static void LoadCreditsAdditive()
    {
        SceneManager.LoadScene("CreditsScene", LoadSceneMode.Additive);
    }
}
