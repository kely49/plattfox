using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlSonido : MonoBehaviour
{
    [SerializeField] private Sprite altavoz;
    [SerializeField] private Sprite altavozMute;
    [SerializeField] private Button botonSonido;
    public void switchAudio(){
        AudioListener.pause = !AudioListener.pause;

        if (AudioListener.pause)
        {
            botonSonido.image.sprite = altavozMute;
        }
        else
        {
            botonSonido.image.sprite = altavoz;
        }
    }
}
