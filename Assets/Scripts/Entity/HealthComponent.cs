using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] private float health = 10f;
    [SerializeField] private bool immortal = false;

    public void TakeDamage(float damage) {
        health -= damage; 

        if (health <= 0 && !immortal) {
            gameObject.GetComponent<Entity>().DestroySelf();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
