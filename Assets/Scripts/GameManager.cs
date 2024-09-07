using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    
    private int lives;
    public Transform orbs;
    public Pacman pacman;
    public ScoreManager scoreManager;
    public int totalOrbs;
    public int numOrbs;
    public Fruit fruitManager;

    public RedGhost redGhost;
    public BlueGhost blueGhost;
    // public PinkGhost pinkGhost;
    public OrangeGhost orangeGhost;
    public GameObject livesPrefab;

    public bool isFrightened = false;
    public float frightenedTimer = 0.0f;
    public float frightenedDuration = 8.0f;

    public float[] ghostModeDurations = {20.0f, 7.0f, 20.0f, 5.0f, 20.0f, 5.0f, 1000.0f};
    public float modeTimer = 0.0f;
    public int currentModeIndex = 0;
    public List<GameObject> livesTable;

    private void Start()
    {
        livesTable= new List<GameObject>();
        StartGame();
    }
    private void StartGame() // d�but de partie
    {
        scoreManager.ResetScore();
        SetLives(3);
        for(int i = 0; i < lives; i++)
        {
            livesTable.Add(Instantiate(livesPrefab, new Vector2(-30 + 3*i, -14), Quaternion.identity));
        }


        foreach (Transform orb in orbs)
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
        redGhost.ResetGhost(); // reset position du ghost
        blueGhost.ResetGhost(); // reset position du ghost
        orangeGhost.ResetGhost(); // reset position du ghost
        // pinkGhost.ResetGhost(); // reset position du ghost

        modeTimer = 0.0f;
        currentModeIndex = 0;
        isFrightened = false;
        frightenedTimer = 0.0f;
        redGhost.SetNormalMode();
        blueGhost.SetNormalMode();
        orangeGhost.SetNormalMode();
        // pinkGhost.SetNormalMode();

        //timer 3 2 1 avant de commencer
    }

    private void Update()
    {
        if (isFrightened)
        {
            frightenedTimer += Time.deltaTime;
            if (frightenedTimer >= frightenedDuration)
            {
                redGhost.SetNormalMode();
                blueGhost.SetNormalMode();
                orangeGhost.SetNormalMode();
                // pinkGhost.SetNormalMode();
                isFrightened = false;
                frightenedTimer = 0;
            }
        }

        modeTimer += Time.deltaTime;
        if (modeTimer >= ghostModeDurations[currentModeIndex])
        {
            modeTimer = 0;
            currentModeIndex++;
            redGhost.ToggleScatterMode();
            blueGhost.ToggleScatterMode();
            orangeGhost.ToggleScatterMode();
            // pinkGhost.ToggleScatterMode();
        }
    }

    public void OnPacmanDeath() //a appeler quand collision entre pacman et ghost
    {
        if(this.lives-1 != 0)
        {
            SetLives(this.lives - 1);
            int index = livesTable.Count - 1;
            Destroy(livesTable[index]);
            livesTable.RemoveAt(index);
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

        redGhost.SetFrightenedMode();
        blueGhost.SetFrightenedMode();
        orangeGhost.SetFrightenedMode();
        // pinkGhost.SetFrightenedMode();
        isFrightened = true;
        frightenedTimer = 0;
    }
}
