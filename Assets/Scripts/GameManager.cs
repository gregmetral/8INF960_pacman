using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    
    private int lives;
    public Transform orbs;
    public Pacman pacman;
    public GameObject[] ghosts;
    public ScoreManager scoreManager;
    public int totalOrbs;
    public int numOrbs;
    public Fruit fruitManager;

    public RedGhost redGhost;

    private void Start()
    {
        StartGame();
    }
    private void StartGame() // d�but de partie
    {
        scoreManager.ResetScore();
        SetLives(3);
        foreach(Transform orb in orbs)
        {
            orb.gameObject.SetActive(true);
            numOrbs += 1;
        }
        totalOrbs = numOrbs;
        StartRound();
    }

    private void StartRound() //d�but de round (1 round/vie)
    {
        pacman.ResetPacman(); // reset position de pacman
        fruitManager.NewRound(); // reset fruit
        redGhost.ResetRedGhost(); // reset position du ghost
        //timer 3 2 1 avant de commencer
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

    private void EndGame() //fin de partie, sauvegarde le score et load la scene de fin
    {
        //animation mort
        scoreManager.SaveScore();
        SceneManager.LoadScene("Score"); 
    }

    private void SetLives(int numLives)
    {
        this.lives = numLives;
    }

    private void SetNumOrbs(int newNumOrbs)
    {
        this.numOrbs = newNumOrbs;
    }

    private void OnOrbEaten(GameObject orb) //d�sactive l'orbe et compte le nombre d'orbes restant
    {
        orb.SetActive(false);
        SetNumOrbs(numOrbs - 1);
        if (numOrbs == 0)
        {
            SetLives(3);
            foreach(Transform o in orbs)
            {
                o.gameObject.SetActive(true);
                numOrbs += 1;
            }
            StartRound();
        }
        fruitManager.OrbEat();
    }

    public void EatOrb(GameObject orb) //a appeler quand pacman mange une orbe
    {
        scoreManager.AddScore(10);
        OnOrbEaten(orb);
    }

    public void EatPowerOrb(GameObject powerOrb) //a appeler quand pacman mange une orbe sp�ciale
    {
        scoreManager.AddScore(50);
        OnOrbEaten(powerOrb);
    }

    
}
