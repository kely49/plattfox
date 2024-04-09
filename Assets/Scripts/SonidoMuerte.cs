using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonidoMuerte : MonoBehaviour
{
    [SerializeField] private AudioClip sonidoMuerte;
    [SerializeField] private AudioSource audioSource;

    public void sonarMuerte(){
        audioSource.PlayOneShot(sonidoMuerte);
    }
}
