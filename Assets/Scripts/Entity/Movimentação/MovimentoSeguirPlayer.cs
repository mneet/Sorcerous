using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MovimentoSeguirPlayer : MonoBehaviour
{
    public Transform target; // alvo que eu quero seguir
    public float speed;



    void Update()
    {
        Vector3 dirTarget = (transform.position - target.position); // direção entre o alvo e o player~
        dirTarget.x = 0f;
        dirTarget.y = 0f;

        dirTarget.Normalize(); // normalize a direcao para que o tamanho do vetor seja 1 mas a direcao e sentido se mantenham

        if (Vector3.Distance(target.position, transform.position) > 3f)
        {
            transform.Translate(dirTarget * Time.deltaTime * speed);
        }
        transform.rotation = Quaternion.LookRotation(-dirTarget); // faz o objeto olhar para a direcao que esta
    }
}
