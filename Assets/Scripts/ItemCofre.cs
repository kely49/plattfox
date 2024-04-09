using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCofre : MonoBehaviour
{
    private bool itemRecogido = false;
    private Player player;
    [SerializeField] private string nombreItem;
    [SerializeField] private GameObject itemContenido;
    [SerializeField] private GameObject mensaje;
    [SerializeField] private AudioClip sonidoItem;

    private void Start() {
        player = FindObjectOfType<Player>();
    }
    private void OnTriggerEnter2D(Collider2D other) {
         if(other.gameObject.CompareTag("Player")){
            if(!itemRecogido){
                if(nombreItem == "tripleSalto"){
                    player.setSaltosExtra(1);

                    gameObject.GetComponent<BoxCollider2D>().enabled = false;
                    itemContenido.SetActive(false);

                    itemRecogido = true;

                    player.enviarSonido(sonidoItem);
                    StartCoroutine(mostrarMensaje());
                }
            }
         }
    }

    private IEnumerator mostrarMensaje(){
        mensaje.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        mensaje.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        mensaje.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        mensaje.SetActive(false);
         yield return new WaitForSeconds(0.5f);
        mensaje.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        mensaje.SetActive(false);
         yield return new WaitForSeconds(0.5f);
        mensaje.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        mensaje.SetActive(false);
    }
}
