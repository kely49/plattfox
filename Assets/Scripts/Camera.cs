using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Referencia al objeto que la cámara seguirá
    public float verticalOffset = 3.7f; // Distancia vertical entre el jugador y la cámara

    void LateUpdate()
    {
        if (target != null)
        {
            // Centrar la cámara en el jugador, pero con un desplazamiento vertical
            transform.position = new Vector3(target.position.x, target.position.y + verticalOffset, transform.position.z);
        }
    }
}
