using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rana : MonoBehaviour
{
    public float fuerzaSalto = 10f; // La fuerza del salto
    public float velocidadMovimiento = 5f; // La velocidad de movimiento horizontal
    public float esperaEntreSaltos = 2f; // Tiempo de espera entre saltos
    private Rigidbody2D rb;
    private Animator animator;
    private BoxCollider2D collider2D;
    private bool enSuelo;
    public Transform controladorAbajo;
    public float distanciaAbajo;
    public LayerMask capaAbajo;
    public bool informacionAbajo;
    private bool mirandoDerecha;
    private SpriteRenderer spriteRenderer;
    private bool golpeado = false;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip sonidoGolpe;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator= GetComponent<Animator>();
        collider2D = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        // Comienza la rutina para alternar los saltos
        StartCoroutine(AlternarSaltos());
    }

    private void Update() {
        float movimientoHorizontal = rb.velocity.y * velocidadMovimiento;

        informacionAbajo =  Physics2D.Raycast(controladorAbajo.position,transform.up * -1,distanciaAbajo,capaAbajo);
        animator.SetBool("EnSuelo",informacionAbajo);
        

        if(Mathf.Abs(rb.velocity.y) > Mathf.Epsilon){
            animator.SetFloat("VelocidadY", rb.velocity.y);
        } else{
            animator.SetFloat("VelocidadY",0);
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

    IEnumerator AlternarSaltos()
    {
        while (true)
        {
            // Salto a la derecha
            SaltoDerecha();
            Girar();
            yield return new WaitForSeconds(esperaEntreSaltos);

            // Salto a la izquierda
            SaltoIzquierda();
            Girar();
            yield return new WaitForSeconds(esperaEntreSaltos);
        }
    }

    void SaltoDerecha()
    {
        rb.velocity = new Vector2(velocidadMovimiento, fuerzaSalto);
    }

    void SaltoIzquierda()
    {
        rb.velocity = new Vector2(-velocidadMovimiento, fuerzaSalto);
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(controladorAbajo.transform.position,controladorAbajo.transform.position + transform.up * -1 * distanciaAbajo);
    }

    private void Girar(){
        mirandoDerecha = !mirandoDerecha;
        if(mirandoDerecha){
            spriteRenderer.flipX = true;
        } else{
            spriteRenderer.flipX = false;
        }
    }

}
