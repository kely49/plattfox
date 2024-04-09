using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject menu;
    private combateJugador combateJugador;

    private void Start() {
        combateJugador = GameObject.FindGameObjectWithTag("Player").GetComponent<combateJugador>(); 
        combateJugador.muerteJugador += AbrirMenu;
    }

    private void AbrirMenu(object sender,EventArgs e){
        menu.SetActive(true);
    }
    public void Reiniciar()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
