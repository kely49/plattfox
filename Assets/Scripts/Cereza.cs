using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cereza : MonoBehaviour
{
    private combateJugador combateJugador;
    private Animator animator;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip sonidoCura;
    private void Start() {
        combateJugador = FindObjectOfType<combateJugador>();
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Player")){
            if(combateJugador.curar(1)){
                animator.SetBool("Curado",true);
                StartCoroutine(itemRecogido());
                audioSource.PlayOneShot(sonidoCura);
            } else{
                Debug.Log("VIDA AMXIMA");
            }
        }
    }

    private IEnumerator itemRecogido(){
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
