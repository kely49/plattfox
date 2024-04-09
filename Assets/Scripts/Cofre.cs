using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cofre : MonoBehaviour
{
    [SerializeField] private GameObject cofreAbierto;
    [SerializeField] private GameObject cofreCerrado;
    [SerializeField] private GameObject llaveNecesaria;
    private bool abierto = false;
    private bool itemRecogido = false;
    private bool tieneLlaveLvl1 = false;
    private Player player;

    [SerializeField] private AudioClip sonidoCofreAbierto;
    [SerializeField] private AudioClip sonidoError;

    private void Start() {
        player = FindObjectOfType<Player>();
    }
    private void Update() {
        if (abierto && Input.GetKeyDown(KeyCode.E) && !itemRecogido)
        {
            if(tieneLlave()){
                cofreCerrado.SetActive(false);
                cofreAbierto.SetActive(true);
                gameObject.GetComponent<BoxCollider2D>().enabled = false;
                player.enviarSonido(sonidoCofreAbierto);

                itemRecogido = true;
            } else{
                player.enviarSonido(sonidoError,1);
                StartCoroutine(mostrarLlave());
                StartCoroutine(error());
                
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
         if(other.gameObject.CompareTag("Player")){
            abierto = true;
         }
    }

    private bool tieneLlave(){
        return player.getTieneLlaveLvl1();
    }
    
    private IEnumerator mostrarLlave(){
        Debug.Log("enumerator");
        llaveNecesaria.SetActive(true);
        yield return new WaitForSeconds(2f);
        llaveNecesaria.SetActive(false);
        
    }
    private IEnumerator error(){
        yield return new WaitForSeconds(0.3f);
        player.regularSonido(0.135f);
    }
}
