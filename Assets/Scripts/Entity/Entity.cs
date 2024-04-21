using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField] private bool isEnemy = true;

    public GameObject spawner = null;
    public void DestroySelf() {
        GameObject.Destroy(gameObject);

        if (spawner != null && isEnemy) {
            spawner.GetComponent<WaveManager>().MobDestroyed(gameObject);
        }
    }

    void Update()
    {
        
    }
}
