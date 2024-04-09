using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aguila : MonoBehaviour
{
    [SerializeField] private float velocidadMovimiento;
    [SerializeField] private Transform[] puntosMovimiento;
    [SerializeField] private float distanciaMinima;
    private int siguientePaso = 0;
    private SpriteRenderer spriteRenderer;
    private bool golpeado;
    private Animator animator;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip sonidoGolpe;

    private void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        Girar();
    }

    private void Update() {
        transform.position = Vector2.MoveTowards(transform.position,puntosMovimiento[siguientePaso].position,velocidadMovimiento * Time.deltaTime);

        if(Vector2.Distance(transform.position,puntosMovimiento[siguientePaso].position) < distanciaMinima){
            siguientePaso += 1;
            if(siguientePaso >= puntosMovimiento.Length){
                siguientePaso = 0;
            }
            Girar();
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        
        if(other.gameObject.CompareTag("Player")){
            //Si damos el hit por encima del enemigo, lo matamos
            if(!golpeado){
                if(other.GetContact(0).normal.y < 0){
                    other.gameObject.GetComponent<Player>().reboteSuperior();
                    animator.SetBool("golpeado",true);
                    golpeado = true;
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

    private IEnumerator DestruirDespuesDeAnimacion()
    {
        // Espera hasta que la animación "golpeado" haya terminado
        yield return new WaitForSeconds(0.8f);
        
        // Destruye el objeto
        Destroy(gameObject);
    }

    private void Girar() {
        if(transform.position.x < puntosMovimiento[siguientePaso].position.x){
            spriteRenderer.flipX = true;
        } else{
            spriteRenderer.flipX = false;
        }
    }
}
