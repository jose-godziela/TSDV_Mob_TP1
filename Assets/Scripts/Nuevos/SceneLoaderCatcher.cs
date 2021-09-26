using UnityEngine;

public class SceneLoaderCatcher : MonoBehaviour
{
    [HideInInspector] public SceneLoader slReference;

    void Awake()
    {
        slReference = FindObjectOfType<SceneLoader>();
    }

    public void GoCredits()
    {
        slReference?.GoCredits();
    }

    public void GoMainMenu()
    {
        slReference?.GoMenu();
    }

    public void SetSinglePlayer()
    {
        slReference?.SetGameModeSinglePlayer();
    }

    public void SetMultiPlayer()
    {
        slReference?.SetGameModeMultiPlayer();
    }

    public void SetEasy()
    {
        slReference?.SetDifficultyEasy();
    }

    public void SetNormal()
    {
        slReference?.SetDifficultyNormal();
    }

    public void SetHard()
    {
        slReference?.SetDifficultyHard();
    }

    public void FadeToLevel()
    {
        slReference?.FadeToLevel();
    }

    public void QuitGame()
    {
        slReference?.QuitGame();
    }
}
