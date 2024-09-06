using UnityEditor.Rendering;
using UnityEngine;

public class BlueGhost : MonoBehaviour
{
    // Initialisation des sprites pour le mouvement static du fantôme
    public SpriteRenderer spriteRenderer {get; private set;}
    public Sprite[] sprites;
    public int indexSprite {get; private set;}

    // Variable pour savoir dans quel mode est le fantôme 
    public bool home = true;
    public bool frightened = false;

    public LayerMask obstacleLayer;

    public Vector2 direction {get; private set;}
    private Vector2 currentPosition;
    public float speed;

    public new Rigidbody2D rigidbody {get; private set;}

    private void Awake(){
        this.spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start(){
        // On appelle une fonction de façon répétitive toutes les n secondes
        InvokeRepeating(nameof(Advance), 0.25f, 0.25f);
        this.direction = Vector2.left;
    }

    private void FixedUpdate(){
        this.currentPosition = this.transform.position;
        this.rigidbody.MovePosition(currentPosition + direction * speed * Time.fixedDeltaTime);
    }

    // Fonction pour le chagement de sprite 
    private void Advance(){

        // On vérifie si le spriteRenderer est actif ou non
        if(!this.spriteRenderer.enabled){
            return;
        }

        this.indexSprite++;

        if(this.indexSprite >= this.sprites.Length){
            this.indexSprite = 0;
        }

        if(this.indexSprite >= 0 && this.indexSprite < this.sprites.Length){
            this.spriteRenderer.sprite = this.sprites[this.indexSprite];
        }

    }




    /*private void Update(){
    }

    private void OnCollisionEnter2D(Collision2D collision){

        // Si on touche un obstacle on souhaite tout simplement aller dans la direction opposée
        if (this.enabled && collision.gameObject.layer == LayerMask.NameToLayer("Wall")){
            this.rigidbody.MovePosition(Vector3.up);
        }
    }*/
}
