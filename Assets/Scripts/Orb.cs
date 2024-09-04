using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Pacman"))
        {
            FindObjectOfType<GameManager>().EatOrb(this.gameObject);
        }
    }
}
