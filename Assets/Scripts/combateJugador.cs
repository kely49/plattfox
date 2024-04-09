using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class combateJugador : MonoBehaviour
{
    [SerializeField] private int vida;
    [SerializeField] private int maximoVida;
    [SerializeField] private BarraVida barraVida;
    private Player player;
    private Animator animator;
    [SerializeField] private float tiempoPerdidaControl;
    public event EventHandler muerteJugador;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip sonidoHerido;
    [SerializeField] private SonidoMuerte sonidoMuerte;


    private void Start() {
        player = GetComponent<Player>();
        animator = GetComponent<Animator>();
        vida = maximoVida;
        barraVida.inicializarBarraDeVida(vida);
    }

    public void tomarDaño(int daño,Vector2 posicion){
        if(vida > 1){
            animator.SetBool("golpeado",true);
            
            vida -= daño;
            
            StartCoroutine(perderControl());
            StartCoroutine(desactivarcolision());
            player.rebote(posicion);
            barraVida.cambiarVidaActual(vida);
            audioSource.PlayOneShot(sonidoHerido);
        }else {
            vida -= daño;
            barraVida.cambiarVidaActual(vida);
            animator.SetBool("Muerte",true);
            sonidoMuerte.sonarMuerte();
            Destroy(gameObject);
            MuerteJugadorEvento();
            
        }
    }

    public void tomarDañoSinRetroceso(int daño){
        if(vida > 1){
            animator.SetBool("golpeado",true);
            
            vida -= daño;
            StartCoroutine(perderControl());
            StartCoroutine(desactivarcolision());
            barraVida.cambiarVidaActual(vida);
            audioSource.PlayOneShot(sonidoHerido);
        } else{
            vida -= daño;
            barraVida.cambiarVidaActual(vida);
            animator.SetBool("Muerte",true);
            sonidoMuerte.sonarMuerte();
            Destroy(gameObject);
            MuerteJugadorEvento();
        }
    }

    public bool curar(int curacion){
        bool seHaCurado = false;
        if((vida + curacion) > maximoVida){
            vida = maximoVida;
        } else{
            vida += curacion;
            seHaCurado = true;
        }
        barraVida.cambiarVidaActual(vida);

        return seHaCurado;
    }

    private IEnumerator perderControl(){
        player.sePuedeMover = false;
        yield return new WaitForSeconds(tiempoPerdidaControl);
        player.sePuedeMover = true;
        animator.SetBool("golpeado",false);
    }

    private IEnumerator desactivarcolision(){
        Physics2D.IgnoreLayerCollision(8,9,true);
        yield return new WaitForSeconds(tiempoPerdidaControl);
        Physics2D.IgnoreLayerCollision(8,9,false);
    }

    public void MuerteJugadorEvento(){
        muerteJugador?.Invoke(this,EventArgs.Empty);
    }
}

