using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    private ScoreManager instance;
    private string HighscoreSaveName = "Highscore";
    private string ScoreSaveName = "GameScore";

    [Header("Score")]
    [SerializeField] private int score = 0;
    [SerializeField] private string scoreTextTag = "ScoreText";
    private TMP_Text scoreText;

    [Header("Highscore")]
    [SerializeField] private int highscore;
    [SerializeField] private string highscoreTextTag = "HighscoreText";
    private TMP_Text highscoreText;

    [Header("New Highscore")]
    [SerializeField] private bool newHighscore = false;
    [SerializeField] private string newHighscoreObjectTag = "NewHighscoreObject";
    private GameObject newHighscoreObject;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
            SceneManager.sceneLoaded += OnSceneLoaded;
        } else
        {
            Destroy(this);
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        score = PlayerPrefs.GetInt(ScoreSaveName);
        highscore = PlayerPrefs.GetInt(HighscoreSaveName);

        scoreText = GameObject.FindGameObjectWithTag(scoreTextTag)?.GetComponent<TMP_Text>();
        highscoreText = GameObject.FindGameObjectWithTag(highscoreTextTag)?.GetComponent<TMP_Text>();
        newHighscoreObject = GameObject.FindGameObjectWithTag(newHighscoreObjectTag);
        DisplayScore();
    }

    private void DisplayScore()
    {
        if (scoreText)
            scoreText.text = "Score: " + score;
        if (highscoreText)
            highscoreText.text = "Highscore: " + highscore;
        if (newHighscoreObject)
            newHighscoreObject.SetActive(newHighscore);
    }
    
    public void AddScore(int addedScore)
    {
        score += addedScore;
        if (score > highscore)
        {
            newHighscore = true;
            highscore = score;
        }
        DisplayScore();
    }

    public void ResetScore()
    {
        score = 0;
        newHighscore = false;
        DisplayScore();
    }

    public void SaveScore() //sauvegarde le score une fois la partie terminée
    {
        PlayerPrefs.SetInt(ScoreSaveName, score);
        if (newHighscore)
        {
            PlayerPrefs.SetInt(HighscoreSaveName, highscore);
        }
    }
}
