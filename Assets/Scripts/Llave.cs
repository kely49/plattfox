using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Llave : MonoBehaviour
{
    private Player player;
    [SerializeField] private string nombreItem;
    [SerializeField] private AudioClip sonidoLlave;
    private void Start() {
        player = FindObjectOfType<Player>();
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Player")){
            if(nombreItem == "lvl1"){
                player.setTieneLlaveLvl1(true);
                player.enviarSonido(sonidoLlave);
            }
            Destroy(gameObject);
        }
    }
}
