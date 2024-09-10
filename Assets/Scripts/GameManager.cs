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
    public int nbOrbEaten;
    public Fruit fruitManager;

    public RedGhost redGhost;
    public BlueGhost blueGhost;
    public PinkGhost pinkGhost;
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

    private void StartRound() // round beginning
    {
        // Reset all states
        pacman.ResetPacman();
        fruitManager.NewRound(); 
        redGhost.ResetGhost(); 
        blueGhost.ResetGhost(); 
        orangeGhost.ResetGhost(); 
        pinkGhost.ResetGhost(); 

        modeTimer = 0.0f;
        currentModeIndex = 0;
        isFrightened = false;
        frightenedTimer = 0.0f;
        redGhost.SetNormalMode();
        blueGhost.SetNormalMode();
        orangeGhost.SetNormalMode();
        pinkGhost.SetNormalMode();

        // Timer
        StartCoroutine(StartCountdown());
    }

    private IEnumerator StartCountdown()
    {
        // Deactivate everything
        redGhost.enabled = false;
        pinkGhost.enabled = false;
        orangeGhost.enabled = false;
        blueGhost.enabled = false;
        pacman.enabled = false;

        CountdownManager cm = FindObjectOfType<CountdownManager>();
        cm.StartCountdown();
        yield return new WaitForSeconds(3);

        redGhost.enabled = true;
        pinkGhost.enabled = true;
        orangeGhost.enabled = true;
        blueGhost.enabled = true;
        pacman.enabled = true;

        // Respawn ghosts if they should be outside
        StartCoroutine(RespawnGhosts());
    }

    private IEnumerator RespawnGhosts()
    {
        yield return new WaitForSeconds(1);
        if (nbOrbEaten >= 2)
        {
            pinkGhost.LeaveGhostHouse();
        }
        yield return new WaitForSeconds(1);
        if (nbOrbEaten >= 30)
        {
            blueGhost.LeaveGhostHouse();
        }
        yield return new WaitForSeconds(1);
        if (nbOrbEaten >= 82)
        {
            orangeGhost.LeaveGhostHouse();
        }
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
                pinkGhost.SetNormalMode();
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
            pinkGhost.ToggleScatterMode();
        }
    }

    public void OnPacmanDeath()
    {
        if(this.lives - 1 != 0)
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

    private void EndGame()
    {
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

    private void OnOrbEaten(GameObject orb) 
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
        nbOrbEaten ++;
        if (nbOrbEaten == 2)
        {
            pinkGhost.LeaveGhostHouse();
        }
        if (nbOrbEaten == 82)
        {
            orangeGhost.LeaveGhostHouse();
        }
        else if (nbOrbEaten == 30)
        {
            blueGhost.LeaveGhostHouse();
        }
    }

    public void EatOrb(GameObject orb)
    {
        scoreManager.AddScore(10);
        OnOrbEaten(orb);
    }

    public void EatPowerOrb(GameObject powerOrb) 
    {
        scoreManager.AddScore(50);
        OnOrbEaten(powerOrb);

        if (!redGhost.home)
        {
            redGhost.SetFrightenedMode();
        }
        if (!blueGhost.home)
        {
            blueGhost.SetFrightenedMode();
        }
        if (!orangeGhost.home)
        {
            orangeGhost.SetFrightenedMode();
        }
        if (!pinkGhost.home)
        {
            pinkGhost.SetFrightenedMode();
        }
        isFrightened = true;
        frightenedTimer = 0;
    }
}