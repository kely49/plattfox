using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Final : MonoBehaviour
{
    [SerializeField] private GameObject pantallaFinal;

    private Player player;
    // Start is called before the first frame update
    void Start()
    {
        player =  FindObjectOfType<Player>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
         if(other.gameObject.CompareTag("Player")){
             mostrarPantallaFinal(); 
         }
    }
    private void mostrarPantallaFinal(){
        pantallaFinal.SetActive(true);
        Destroy(player);
    }
}
