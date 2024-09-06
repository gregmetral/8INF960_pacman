
using UnityEngine;

public class Passage : MonoBehaviour
{
    // On définit une référence selon la connection ou l'on veut aller
    public Transform connection;
    private void OnTriggerEnter2D(Collider2D other){
        // On récupère la position actuelle de l'objet 
        Vector3 position = other.transform.position;
        // On définit les positions x et y
        position.x = this.connection.position.x;
        position.y = this.connection.position.y;
        other.transform.position = position;

    }
}
