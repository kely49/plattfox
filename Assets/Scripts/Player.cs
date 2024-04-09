using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb; // Referencia al componente Rigidbody2D

    [Header("Movimiento")]
    private Vector2 input;
    private float movimientoHorizontal = 0f;
    [SerializeField] private float velocidadDeMovimiento;
    [Range(0, 0.3f)][SerializeField] private float suavidadDeMovimiento;
    private Vector3 velocidad = Vector3.zero;
    private bool mirandoDerecha = true;

    [Header("Salto")]
    [SerializeField] private float fuerzaDeSalto;
    [SerializeField]  private LayerMask queEsSuelo;
    [SerializeField]  private LayerMask queEsPlataforma;
    [SerializeField] private Transform controladorSuelo;
    [SerializeField] private Vector3 dimensionesCaja;
    [SerializeField] private bool enSuelo;
    [SerializeField] private bool enPlataforma;
    private bool salto = false;
    [SerializeField] private int saltosExtraRestantes;
    [SerializeField] private int saltosExtra;

    [Header("Escalar")]
    [SerializeField] private  float velocidadEscalar;
    private CapsuleCollider2D capsuleCollider2D;
    private float gravedadInicial;
    private bool escalando;

    [Header("Agacharse")]
    [SerializeField] private Transform controladorTecho;
    [SerializeField] private  float radioTecho;
    [SerializeField] private  float multiplicadorVelocidadAgachado;
    [SerializeField] private  Collider2D colisionAgachado;
    private bool estabaAgachado = false;
    private bool agachar = false;

    [Header("Combate")]
    public bool sePuedeMover = true;
    [SerializeField] private Vector2 velocidadRebote;
    [SerializeField] private float velocidadReboteSuperior;
    private combateJugador combateJugador;

    [Header("Inventario")]
    private bool tieneLlaveLvl1 = false;

    [Header("Animacion")]
    private Animator animator;

    private Vector3 ultimaPosicionSuelo;

    [Header("Sonidos")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource audioSourceCaminar;
    [SerializeField] private AudioSource audioSourceEscalar;
    [SerializeField] private AudioClip sonidoSalto;
    [SerializeField] private AudioClip sonidoCaminar;
    [SerializeField] private AudioClip sonidoEscalar;
    
    private bool sonidoReproducido = false;
    private bool sonidoEscalarReproducido = false;

    private void Start()
    {
        //Obtencion componentes
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        combateJugador = GetComponent<combateJugador>();

        //Inicializar variables al empezar el juego
        rb.velocity = new Vector2(rb.velocity.x, 0f);
        gravedadInicial =  rb.gravityScale;

        audioSource = GetComponent<AudioSource>();
        
    }

    private void Update()
    {
        //sacamos los inputs verticales y horizontales
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");

        //Movemos y animamos al personaje
        movimientoHorizontal = input.x * velocidadDeMovimiento;
        animator.SetFloat("VelocidadX",Mathf.Abs(movimientoHorizontal));

        //Ajustamos la animacion de la escalera para que el personaje tenga una animacion lenta
        if(Mathf.Abs(rb.velocity.y) > Mathf.Epsilon){
            animator.SetFloat("VelocidadY", Mathf.Sign(rb.velocity.y));
        } else{
            animator.SetFloat("VelocidadY",0);
        }

        //Ajustes para el doble salto
        if(enSuelo || enPlataforma){
            saltosExtraRestantes = saltosExtra;
        }

        if (Input.GetKeyDown(KeyCode.Space)){
            if(input.y >= 0) {
                 salto = true;
            } else{
                desactivarPlataformas();
            }
           
        }

        if(input.y  < 0){
            agachar = true;
        } else{
            agachar = false;
        }
    }

    private void desactivarPlataformas(){
        Collider2D[] objetos = Physics2D.OverlapBoxAll(controladorSuelo.position,dimensionesCaja,0f,queEsPlataforma);
        foreach(Collider2D item in objetos){
            PlatformEffector2D platformEffector2D = item.GetComponent<PlatformEffector2D>();

            if(platformEffector2D != null){
                Physics2D.IgnoreCollision(GetComponent<Collider2D>(),item.GetComponent<Collider2D>(),true);
            }
        }
    }
    private void FixedUpdate() {
        //Creamos la caja de los pies del personaje que detecta el suelo y animamos
        enSuelo = Physics2D.OverlapBox(controladorSuelo.position,dimensionesCaja,0f,queEsSuelo);
        enPlataforma = Physics2D.OverlapBox(controladorSuelo.position,dimensionesCaja,0f,queEsPlataforma);
        animator.SetBool("enSuelo",enSuelo || enPlataforma);
        animator.SetBool("agachar",estabaAgachado);

        if(sePuedeMover){
            procesarMovimiento(movimientoHorizontal * Time.fixedDeltaTime, salto, agachar);
            escalar();
            fueraDelMapa();
            
        }

        salto = false;
    }

    //Metodo que dibuja la caja detectora de suelo en debug
    private void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(controladorSuelo.position,dimensionesCaja);
        Gizmos.DrawWireSphere(controladorTecho.position,radioTecho);
    }

    private void fueraDelMapa(){
        if (Physics2D.OverlapBox(controladorSuelo.position, dimensionesCaja, 0f, LayerMask.GetMask("Muerte"))) {
                // Devolver al personaje a la última posición del suelo con ajuste lateral
                if (transform.position.x > ultimaPosicionSuelo.x) {
                    // Si el personaje se movió hacia la derecha antes de morir, muévelo unos píxeles a la izquierda
                    transform.position = new Vector3(ultimaPosicionSuelo.x - 3f, ultimaPosicionSuelo.y, ultimaPosicionSuelo.z);
                } else {
                    // Si el personaje se movió hacia la izquierda antes de morir o no se movió horizontalmente, muévelo unos píxeles a la derecha
                    transform.position = new Vector3(ultimaPosicionSuelo.x + 3f, ultimaPosicionSuelo.y, ultimaPosicionSuelo.z);
                }
                combateJugador.tomarDañoSinRetroceso(1);
        }
    }

    public void procesarMovimiento(float mover, bool saltar, bool agachar){
        
        if (enSuelo) {
            ultimaPosicionSuelo = transform.position;
        }

        if(!agachar){
            if(Physics2D.OverlapCircle(controladorTecho.position,radioTecho,queEsSuelo)){
                agachar = true;
            }
        } 

        if(agachar) {
            if(!estabaAgachado){
                estabaAgachado = true;
            }

            mover *= multiplicadorVelocidadAgachado;
            colisionAgachado.enabled = false;
        } else{
            colisionAgachado.enabled = true;

            if(estabaAgachado){
                estabaAgachado = false;
            }
        }

        //movemos al personaje
        Vector3 velocidadObjetivo = new Vector2(mover, rb.velocity.y);
        rb.velocity = Vector3.SmoothDamp(rb.velocity, velocidadObjetivo, ref velocidad, suavidadDeMovimiento);

        //giramos sprite dependiendo donde mire el personaje
        if (mover > 0 && !mirandoDerecha){
            girarSprite();
        } else if (mover < 0 && mirandoDerecha){
            girarSprite();
        }

        if (mover != 0 && (enSuelo || enPlataforma)){
            if (!sonidoReproducido)
            {
                audioSourceCaminar.PlayOneShot(sonidoCaminar);
                sonidoReproducido = true;
            }
            
        } else if(mover == 0 && (enSuelo || enPlataforma)){
            audioSourceCaminar.Stop();
            sonidoReproducido = false;
        }

        //gestion del salto doble
        if(saltar){
            if(enSuelo || enPlataforma){
                movimientoSalto();
                audioSourceCaminar.Stop();
                audioSource.PlayOneShot(sonidoSalto);
            } else{
                if(saltar && saltosExtraRestantes > 0){
                    movimientoSalto();
                    saltosExtraRestantes -= 1;
                    audioSource.PlayOneShot(sonidoSalto);
                }
            }
        }
    }

    private void movimientoSalto(){
        //Realiza el salto
        enSuelo = false;
        rb.velocity = new Vector2(0f, fuerzaDeSalto);
        salto = false;
    }
    private void escalar(){
        //Se revisa que estemos elevados o escalando y realizamos el movimiento de escalar
        if((input.y != 0 || escalando) && capsuleCollider2D.IsTouchingLayers(LayerMask.GetMask("Escaleras"))){
            Vector2 velocidadSubida = new Vector2 (rb.velocity.x,input.y * velocidadEscalar);
            rb.velocity = velocidadSubida;
            rb.gravityScale = 0;
            escalando = true;
            if (!sonidoEscalarReproducido)
            {
                audioSourceEscalar.PlayOneShot(sonidoEscalar);
                sonidoEscalarReproducido = true;
            }
        } else{
            rb.gravityScale = gravedadInicial;
            escalando = false;
            audioSourceEscalar.Stop();
            sonidoEscalarReproducido = false;
        }

        if(enSuelo || enPlataforma){
            escalando = false;
        }

        animator.SetBool("estaEscalando",escalando);
    }

    void girarSprite()
    {
        mirandoDerecha = !mirandoDerecha;
        Vector3 escala = transform.localScale;
        escala.x *= -1;
        transform.localScale = escala;
    }

    public void rebote(Vector2 puntoGolpe){
        rb.velocity = new Vector2(-velocidadRebote.x * puntoGolpe.x, velocidadRebote.y);
    }

    public void reboteSuperior(){
        rb.velocity = new Vector2(rb.velocity.x, velocidadReboteSuperior);
    }

    public void setSaltosExtra(int nuevoSalto){
        saltosExtra = nuevoSalto;
    }

    public bool getTieneLlaveLvl1(){
        return tieneLlaveLvl1;
    }

    public void setTieneLlaveLvl1(bool llave){
        tieneLlaveLvl1 = llave;
    }

    public void enviarSonido(AudioClip sonido){
        audioSource.PlayOneShot(sonido);
    }

    public void enviarSonido(AudioClip sonido, int volumen){
        audioSource.PlayOneShot(sonido);
        audioSource.volume = volumen;
    }

    public void regularSonido(float volumen){
        audioSource.volume = volumen;
    }
    
}
