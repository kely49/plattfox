using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlataformaSeCae : MonoBehaviour
{
    [SerializeField] private float tiempoEspera;
    private Rigidbody2D rb2d;
    [SerializeField] private float velocidadRotacion;
    private bool caida = false;
    [SerializeField]private GameObject objetoPlataforma;
    private Vector2 posicionInicial;
    private Vector2 initialVelocity;
    private bool initialCollisionState;
    private RigidbodyConstraints2D initialConstraints;
    private bool primeraVez = false;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        posicionInicial = transform.position;
    }
    private void Update() {
        if(caida){
            transform.Rotate(new Vector3(0,0,-velocidadRotacion * Time.deltaTime));
        } 
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.CompareTag("Player") && !primeraVez){
            StartCoroutine(caidaPlataforma(other));
            primeraVez = true;
        }
        if(other.gameObject.layer == LayerMask.NameToLayer("Suelo") || other.gameObject.layer == LayerMask.NameToLayer("Muerte")){
            Vector3 newPosition = objetoPlataforma.transform.position;
            newPosition.z = -19f;
            objetoPlataforma.transform.position = newPosition;
            gameObject.GetComponent<Collider2D>().enabled = false;
            StartCoroutine(resetPlataforma(other));
            
        }
        
    }

    private IEnumerator caidaPlataforma(Collision2D other){
        initialVelocity = rb2d.velocity;
        initialCollisionState = Physics2D.GetIgnoreCollision(transform.GetComponent<Collider2D>(), other.transform.GetComponent<Collider2D>());
        initialConstraints = rb2d.constraints;

        yield return new WaitForSeconds(tiempoEspera);
        caida = true;
        Physics2D.IgnoreCollision(transform.GetComponent<Collider2D>(), other.transform.GetComponent<Collider2D>());
        rb2d.constraints = RigidbodyConstraints2D.None;
        rb2d.AddForce(new Vector2(0.1f,0));
    }

    private IEnumerator resetPlataforma(Collision2D other){
        yield return new WaitForSeconds(2f);
        // Restaurar el estado inicial después del reset
        rb2d.velocity = initialVelocity;
        Vector3 newPosition = objetoPlataforma.transform.position;
        newPosition.z = 0;
        objetoPlataforma.transform.position = newPosition;
        transform.rotation = Quaternion.Euler(Vector3.zero);
        transform.position = posicionInicial;
        gameObject.GetComponent<Collider2D>().enabled = true;
        rb2d.constraints = initialConstraints;
        caida = false;
        Physics2D.IgnoreCollision(transform.GetComponent<Collider2D>(), other.transform.GetComponent<Collider2D>(), false);
        primeraVez = false;
    }

}
