using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{

    private GameManager gameManager;

    public SpriteRenderer fruitSpriteRenderer;
    public Sprite[] fruitSprites;
    public int[] fruitPoints;
    private int currRound = -1;
    public int[] nbOfPelletNeeded;
    private int indexNbPellet = 0;
    private bool isFruitActive = false;
    private int nbPelletEaten = 0;

    public int timeToDespawn = 10;
    private float fruitTimer = 0;

    // Start is called before the first frame update
    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        fruitSpriteRenderer = GetComponent<SpriteRenderer>();
        fruitSpriteRenderer.enabled = false;
    }

    private void Update()
    {
        if (isFruitActive)
        {
            fruitTimer += Time.deltaTime;
            if (fruitTimer >= timeToDespawn)
            {
                fruitSpriteRenderer.enabled = false;
                isFruitActive = false;
                fruitTimer = 0;
                nbPelletEaten = 0;
                indexNbPellet++;
            }
        }
    }

    public void NewRound()
    {
        fruitSpriteRenderer.enabled = false;
        isFruitActive = false;
        fruitTimer = 0;
        nbPelletEaten = 0;
        indexNbPellet = 0;
        currRound++;
    }

    public void OrbEat()
    {
        nbPelletEaten++;
        if (indexNbPellet < nbOfPelletNeeded.Length && nbPelletEaten == nbOfPelletNeeded[indexNbPellet] && !isFruitActive)
        {
            fruitSpriteRenderer.sprite = fruitSprites[Math.Min(currRound, fruitSprites.Length - 1)];
            fruitSpriteRenderer.enabled = true;
            isFruitActive = true;
            fruitTimer = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Pacman"))
        {
            if (isFruitActive)
            {
                gameManager.scoreManager.AddScore(fruitPoints[Math.Min(currRound, fruitPoints.Length - 1)]);
                fruitSpriteRenderer.enabled = false;
                indexNbPellet++;
                nbPelletEaten = 0;
                isFruitActive = false;
                fruitTimer = 0;
            }
        }
    }
}
