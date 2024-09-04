using UnityEngine;

public class BlueGhost : MonoBehaviour
{
    // Initialisation des sprites pour le mouvement static du fantôme
    public SpriteRenderer spriteRenderer {get; private set;}
    public Sprite[] sprites;
    public int indexSprite {get; private set;}

    // Variable pour savoir dans quel mode est de fantôme 
    public bool home = true;
    public bool frightened = true;


    private void Awake(){
        this.spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start(){
        // On appelle une fonction de façon répétitive toutes les n secondes
        InvokeRepeating(nameof(Advance), 0.25f, 0.25f);
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


}
