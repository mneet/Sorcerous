using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimentoLinearSimples : MonoBehaviour
{
    public float speed;
    public Vector3 direcao;


    void Update()
    {
        Mover();
    }
    public void Mover()
    {
        transform.Translate(direcao * speed * Time.deltaTime);
    }
}
