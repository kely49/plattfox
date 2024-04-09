using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Palanca : MonoBehaviour
{
    private bool activada = false;
    [SerializeField] private Sprite spriteNuevo;
    [SerializeField] private GameObject objetoQueDesbloquea;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private GameObject hint;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip sonidoMadera;
    private bool yaSeActivo = false;

    private void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        if (activada && Input.GetKeyDown(KeyCode.E) && !yaSeActivo)
        {
            spriteRenderer.sprite = spriteNuevo;
            objetoQueDesbloquea.SetActive(false);
            audioSource.PlayOneShot(sonidoMadera);
            yaSeActivo = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Player")){
            activada = true;
        }

        if(hint != null){
            hint.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(hint != null){
            hint.SetActive(false);
        }
    }
}
