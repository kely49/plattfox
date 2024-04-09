using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Murcielago : MonoBehaviour
{
    [SerializeField] public Transform jugador;
    [SerializeField] public float distancia;
    public Vector3 puntoInicial;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    public Rigidbody2D rb2d;
    private bool golpeado = false;
    
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip sonidoGolpe;

    private void Start() {
        animator = GetComponent<Animator>();
        puntoInicial = transform.position;
        spriteRenderer =  GetComponent<SpriteRenderer>();
    }

    private void Update() {
        distancia = Vector2.Distance(transform.position, jugador.position);
        animator.SetFloat("Distancia",distancia);  
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.CompareTag("Player")){
            //Si damos el hit por encima del enemigo, lo matamos
            if(!golpeado){
                if(other.GetContact(0).normal.y < 0){
                    other.gameObject.GetComponent<Player>().reboteSuperior();
                    animator.SetBool("golpeado",true);
                    golpeado = true;
                    rb2d.gravityScale = 0.5f;
                    audioSource.PlayOneShot(sonidoGolpe);
                    StartCoroutine(DestruirDespuesDeAnimacion());
                } else{
                    //si colisionamos desde cualquier otro punto, nos hace daño
                    other.gameObject.GetComponent<combateJugador>().tomarDaño(1,other.GetContact(0).normal);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Player")){
            audioSource.mute= false;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.CompareTag("Player")){
            audioSource.mute= true;
        }
    }

    public void Girar(Vector3 objetivo){
        if(transform.position.x < objetivo.x){
            spriteRenderer.flipX = false;
        } else{
            spriteRenderer.flipX = true;
        }
    }

    private IEnumerator DestruirDespuesDeAnimacion()
    {
        // Espera hasta que la animación "golpeado" haya terminado
        yield return new WaitForSeconds(1.2f);
        
        // Destruye el objeto
        Destroy(gameObject);
    }

}
