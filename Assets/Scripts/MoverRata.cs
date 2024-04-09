using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public Rigidbody2D rb2d;
    public float velocidadDeMovimiento;
    public LayerMask capaAbajo;
    public LayerMask capaEnfrente;
    public float distanciaAbajo;
    public float distanciaEnfrente;
    public Transform controladorEnfrente;
    public Transform controladorAbajo;
    public bool informacionAbajo;
    public bool informacionEnfrente;
    private bool mirandoDerecha;
    [SerializeField] Animator animator;
    private bool golpeado = false;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip sonidoGolpe;

    private void Start() {
        animator = GetComponent<Animator>();
    }
    private void Update() {
        rb2d.velocity = new Vector2(velocidadDeMovimiento,rb2d.velocity.y);
        //Raycast para detectar el suelo y lo que tiene enfrente para darse la vuelta y patrullar
        informacionEnfrente = Physics2D.Raycast(controladorEnfrente.position,transform.right,distanciaEnfrente,capaEnfrente);
        informacionAbajo =  Physics2D.Raycast(controladorAbajo.position,transform.up * -1,distanciaAbajo,capaAbajo);

        if(informacionEnfrente || !informacionAbajo){
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
    private void Girar(){
        mirandoDerecha = !mirandoDerecha;
        transform.eulerAngles = new Vector3(0,transform.eulerAngles.y + 180, 0);
        velocidadDeMovimiento *= -1;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(controladorAbajo.transform.position,controladorAbajo.transform.position + transform.up * -1 * distanciaAbajo);
        Gizmos.DrawLine(controladorEnfrente.transform.position,controladorEnfrente.transform.position + transform.right * distanciaEnfrente);
    }

    private IEnumerator DestruirDespuesDeAnimacion()
    {
        // Espera hasta que la animación "golpeado" haya terminado
        yield return new WaitForSeconds(0.8f);
        
        // Destruye el objeto
        Destroy(gameObject);
    }

}
