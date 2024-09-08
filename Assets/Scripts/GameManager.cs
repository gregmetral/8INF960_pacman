using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    [Header("Pacman")]
    public Transform orbs;
    public Pacman pacman;
    public ScoreManager scoreManager;
    public int totalOrbs;
    public int numOrbs;
    public Fruit fruitManager;

    private int lives;
    public GameObject livesPrefab;
    public List<GameObject> livesTable;

    [Header("Ghosts")]
    public float ghostSpeed = 7;
    public Ghost[] ghosts;
    public int[] pelletsBeforeGhost;
    public Vector2 boxExitPosition = new Vector2(0, 3.5f);

    [Header("Frightened Ghosts")]
    public float ghostFrightenedSpeed = 3;
    public bool isFrightened = false;
    public float frightenedTimer = 0.0f;
    public float frightenedDuration = 8.0f;

    public float[] ghostModeDurations = {20.0f, 7.0f, 20.0f, 5.0f, 20.0f, 5.0f, 1000.0f};
    public float modeTimer = 0.0f;
    public int currentModeIndex = 0;

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
        modeTimer = 0.0f;
        currentModeIndex = 0;
        isFrightened = false;
        frightenedTimer = 0.0f;

        pacman.ResetPacman(); // reset position de pacman
        pacman.enabled = false;
        fruitManager.NewRound(); // reset fruit
        foreach(Ghost ghost in ghosts)
        {
            ghost.enabled = false;
        }

        //timer 3 2 1 avant de commencer
        StartCoroutine(StartCooldown());
    }

    private IEnumerator StartCooldown()
    {
        Debug.Log("3");
        yield return new WaitForSeconds(1);
        Debug.Log("2");
        yield return new WaitForSeconds(1);
        Debug.Log("1");
        yield return new WaitForSeconds(1);
        Debug.Log("GO! GO! GO!");
        yield return new WaitForSeconds(1);
        pacman.enabled = true;
        int i = 0;
        foreach (Ghost ghost in ghosts)
        {
            if (totalOrbs - numOrbs >= pelletsBeforeGhost[i])
            {
                ghost.enabled = true;
                ghost.ResetGhost();
                ghost.SetNormalMode(ghostSpeed);
            }
            i++;
        }
    }

    private void Update()
    {
        if (isFrightened)
        {
            frightenedTimer += Time.deltaTime;
            if (frightenedTimer >= frightenedDuration)
            {
                foreach (Ghost ghost in ghosts)
                {
                    ghost.SetNormalMode(ghostSpeed);
                }
                isFrightened = false;
                frightenedTimer = 0;
            }
        }

        modeTimer += Time.deltaTime;
        if (modeTimer >= ghostModeDurations[currentModeIndex])
        {
            modeTimer = 0;
            currentModeIndex++;
            foreach (Ghost ghost in ghosts)
            {
                ghost.ToggleScatterMode();
            }
        }
    }

    public void OnPacmanDeath() //a appeler quand collision entre pacman et ghost
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
        checkIfNewGhost();
    }

    private void checkIfNewGhost()
    {
        int i = 0;
        foreach (int pbg in pelletsBeforeGhost)
        {
            if (totalOrbs - numOrbs == pbg)
            {
                ghosts[i].enabled = true;
                ghosts[i].ResetGhost();
                ghosts[i].SetNormalMode(ghostSpeed);
            }
            i++;
        }
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

        foreach (Ghost ghost in ghosts)
        {
            ghost.SetFrightenedMode(ghostFrightenedSpeed);
        }
        isFrightened = true;
        frightenedTimer = 0;
    }
}
