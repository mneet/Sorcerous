using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MovimentoSenoidal : MonoBehaviour
{
    public float Amplitude = 0;
    public float Frequencia = 0;
    public float speed;
    public float angulo = 30f;
    private float offsetX = 0f;

    void Start(){
        //offsetX = transform.position.x;
    }


    private Vector3 posFinal;
    void Update()
    {
        transform.Translate(-transform.forward * speed * Time.deltaTime);

        posFinal = transform.position;
        posFinal.x += Mathf.Sin(transform.position.z * Frequencia) * Amplitude;
        
       // posFinal.x += offsetX;
        transform.position = posFinal;
    }
}
