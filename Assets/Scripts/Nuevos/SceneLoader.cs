using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private static SceneLoader Instance;

    [System.Serializable]
    public enum GameDifficulty { Easy, Normal, Hard }
    public GameDifficulty gameDifficulty;

    [System.Serializable]
    public enum GameMode { Single, Multi }
    public GameMode gMode;

    public string singlePlayerSceneName;
    public string multiPlayerSceneName;

    public string nameSceneToLoad;

    public Animator fadingAnim;

    public static SceneLoader Get()
    {
        return Instance;
    }

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetSceneToLoad(string sceneName)
    {
        nameSceneToLoad = sceneName;
    }

    public void LoadScene()
    {
        FadeToLevelIn();
        Scene AuxSene = SceneManager.GetSceneByName(nameSceneToLoad);
        if (AuxSene != null)
        {
            Debug.Log("OH HEOLLO THERE");
            SceneManager.LoadScene(nameSceneToLoad);
        }

    }

    public void FadeToLevel()
    {
        fadingAnim.SetTrigger("Fade_OUT");
    }

    public void FadeToLevelIn()
    {
        fadingAnim.ResetTrigger("Fade_OUT");
        fadingAnim.SetTrigger("Fade_IN");
    }

    public void SetGameModeSinglePlayer()
    {
        gMode = GameMode.Single;
        SetSceneToLoad("SinglePlayer");
    }

    public void SetGameModeMultiPlayer()
    {
        gMode = GameMode.Multi;
        SetSceneToLoad("LocalMultiplayer");
    }

    public void GoCredits()
    {
        SetSceneToLoad("Credits");
    }

    public void GoMenu()
    {
        SetSceneToLoad("MainMenu");
    }

    public void GoFinalPointsSinglePlayer()
    {
        SetSceneToLoad("PtsFinal Singleplayer");
        FadeToLevel();
    }

    public void GoFinalPointsMultiLocal()
    {
        SetSceneToLoad("PtsFinal");
        FadeToLevel();
    }
    public GameMode GetActualMode()
    {
        return gMode;
    }
    public void SetDifficultyEasy()
    {
        gameDifficulty = GameDifficulty.Easy;
    }
    public void SetDifficultyNormal()
    {
        gameDifficulty = GameDifficulty.Normal;

    }
    public void SetDifficultyHard()
    {
        gameDifficulty = GameDifficulty.Hard;

    }
    public GameDifficulty GetActualDifficulty()
    {
        return gameDifficulty;
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }
}
