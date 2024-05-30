using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimentoZigZagDiagonal : MonoBehaviour
{
    public float speed = 6f;
    public Vector3 direcaoPadrao;
    public Vector3 direcaoInvertida;
    public float posParaInverter;
    public Transform cameraTransform;

    public bool inverteu;
    void Start()
    {
        cameraTransform = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        InverterDirecao();
    }

    public void InverterDirecao()
    {
        if (inverteu == false)
        {
            transform.Translate(direcaoPadrao * speed * Time.deltaTime);
            if (transform.position.z < (cameraTransform.position.z + posParaInverter))
            {
                inverteu = true;
                transform.rotation = Quaternion.LookRotation(direcaoInvertida);
            }
        }
        else
        {
            //inverti
            transform.Translate(direcaoInvertida * speed * Time.deltaTime, Space.World);
        }
    }
}
