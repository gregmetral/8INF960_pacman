using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public string levelSceneName = "LevelScene";
    public string mainMenuSceneName = "MainMenu";
    public string scoreSceneName = "ScoreScene";

    public void LoadLevel()
    {
        SceneManager.LoadScene(levelSceneName);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void LoadScoreScene()
    {
        SceneManager.LoadScene(scoreSceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
