using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HealthPowerup : MonoBehaviour {
    private enum Powers { 
        HEAL,
        BULLET,
        FIRE
    }
    [SerializeField] private Powers power;
    [SerializeField] private float heal;
    [SerializeField] private float timerTotal;
    private float currentTimer;

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            Debug.Log("destroy");
            
            Destroy(gameObject);
        }
    }

    private void Heal(GameObject other) {
        other.GetComponent<HealthComponent>().TakeHeal(heal);
    }

    private void Awake() {
        currentTimer = timerTotal;
    }

    private void Update() {
        currentTimer -= Time.deltaTime;
        if (currentTimer <= 0) {
            Destroy(gameObject);
        }

    }
}
