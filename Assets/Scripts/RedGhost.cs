using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedGhost : MonoBehaviour
{
    private Vector2 spawnPosition = new Vector2(0, 3.5f);
    private Rigidbody2D rb;
    private Vector2 currentPosition;
    private Vector2 currentDirection = Vector2.left;
    public float speed;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ResetRedGhost();
    }

    void FixedUpdate()
    {
        currentPosition = this.rb.position;
        this.rb.MovePosition(currentPosition + currentDirection * speed * Time.fixedDeltaTime);
    }

  
    public void ResetRedGhost()
    {
        this.transform.position = spawnPosition;
    }

    private void ChangeDirection()
    {
        
    }

    public void OnNodeLocation()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.layer == LayerMask.NameToLayer("Node"))
        {
            OnNodeLocation();
        }
    }

}
