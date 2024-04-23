using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HealthPowerup : MonoBehaviour
{
    [SerializeField] private float heal;
    [SerializeField] private float timerTotal;
    private float currentTimer;

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            Debug.Log("destroy");
            other.GetComponent<HealthComponent>().TakeHeal(heal);
            Destroy(gameObject);
        }
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
