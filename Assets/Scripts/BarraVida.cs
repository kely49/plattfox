using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarraVida : MonoBehaviour
{
    private Slider slider;
    private Animator animator;

    private void Start() {
        slider = GetComponent<Slider>();
        animator = GetComponent<Animator>();    
    }

    public void cambiarVidaMaxima(int vidaMaxima){
        slider.maxValue = vidaMaxima;
    }

    public void cambiarVidaActual(int cantidadVida){
        slider.value = cantidadVida;
        animator.SetTrigger("golpe");
    }

    public void inicializarBarraDeVida(int cantidadVida){
        cambiarVidaMaxima(cantidadVida);
        cambiarVidaActual(cantidadVida);
    }
}
