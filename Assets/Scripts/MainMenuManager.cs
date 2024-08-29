using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public string levelSceneName = "LevelScene";

    public void LoadLevel()
    {
        SceneManager.LoadScene(levelSceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
