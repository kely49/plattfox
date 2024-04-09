using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plataforma : MonoBehaviour
{
    private Collider2D platformCollider;
    [SerializeField] private GameObject hint;

    private void OnTriggerExit2D(Collider2D other) {
        if(other.CompareTag("Player")){
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(),other.GetComponent<Collider2D>(),false);
        }
        if(hint != null){
            hint.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(hint != null){
            hint.SetActive(true);
        }
    }
}
