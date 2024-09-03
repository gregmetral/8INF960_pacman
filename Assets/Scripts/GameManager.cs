using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    private int score;
    private int lives;
    public Transform orbs;
    public Pacman pacman;
    public GameObject[] ghosts;
    private int numOrbs;

    private void Start()
    {
        StartGame();
    }
    private void StartGame() // début de partie
    {
        SetScore(0);
        SetLives(3);
        foreach(Transform orb in orbs)
        {
            orb.gameObject.SetActive(true);
            numOrbs += 1;
        }
        StartRound();
    }

    private void StartRound() //début de round
    {
        pacman.ResetPacman();
        //timer 3 2 1

    }

    public void OnPacmanDeath() //a appeler quand collision entre pacman et ghost
    {
        if(this.lives-1 != 0)
        {
            SetLives(this.lives - 1);
            StartRound();
        }
        else
        {
            EndGame();
        }

    }

    private void EndGame() //fin de partie
    {
        //animation mort
        SceneManager.LoadScene("Score");
    }

    private void SetScore(int numPoints)
    {
        this.score = numPoints;
    }
    private void SetLives(int numLives)
    {
        this.lives = numLives;
    }

    private void SetNumOrbs(int newNumOrbs)
    {
        this.numOrbs = newNumOrbs;
    }

    public void EatOrb(GameObject orb) //a appeler quand pacman mange une orbe
    {
        orb.SetActive(false);
        SetScore(score + 10);
        SetNumOrbs(numOrbs - 1);
        if(numOrbs == 0)
        {
            EndGame();
        }
    }
}
