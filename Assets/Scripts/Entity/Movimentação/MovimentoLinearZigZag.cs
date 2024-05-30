using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimentoLinearZigZag : MonoBehaviour
{
    public float speed = 6f;
    public Vector3 direcao;
    public float posParaInverter;
    public Transform cameraTransform;

    public bool inverteu;

    void Start()
    {
        cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        ZigZag();
    }

    public void ZigZag()
    {
        transform.Translate(direcao * speed * Time.deltaTime);
        if (transform.position.z < (cameraTransform.position.z + posParaInverter))
        {
            inverteu = true;
            direcao = direcao * -1f;
            transform.localEulerAngles = new Vector3(0,0,0);
        }


    }
}
