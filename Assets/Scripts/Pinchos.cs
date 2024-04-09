using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinchosEstaticos : MonoBehaviour
{
    [SerializeField] private bool SeCaen = false;
    private Rigidbody2D rb2d;
    private bool hanCaido = false;
    [SerializeField] private float velocidadCaida;

    private void Start() {
        rb2d = GetComponent<Rigidbody2D> ();
    }

    private void OnCollisionEnter2D(Collision2D other) {
        Debug.Log(" other.gameObject "+ other.gameObject.tag);
        if(other.gameObject.CompareTag("Player")){
            //Si damos el hit por encima del enemigo, lo matamos
            other.gameObject.GetComponent<combateJugador>().tomarDaño(1,other.GetContact(0).normal);
            if(SeCaen){
                Destroy(gameObject);
            }
        } else if(SeCaen && (other.gameObject.CompareTag("Plataforma") || other.gameObject.CompareTag("Suelo"))){
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (SeCaen) {
            rb2d.gravityScale = velocidadCaida;
            
        }
    }
}
