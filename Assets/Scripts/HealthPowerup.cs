using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HealthPowerup : MonoBehaviour {
    private enum Powers { 
        WATER,
        FIRE,
        EARTH,
        WIND
    }
    [SerializeField] private Powers power;
    [SerializeField] private float heal;
    [SerializeField] private float timerTotal;
    private float currentTimer;

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            Stats playerStats = other.gameObject.GetComponent<Stats>();
            switch (power) {
                case Powers.WATER:
                    Heal(other.gameObject); break;
                case Powers.FIRE:
                    playerStats.UpgradeFire(); break;
                case Powers.EARTH: 
                    playerStats.UpgradeEarth(); break;
                case Powers.WIND:
                    playerStats.UpgradeWind(); break;
            }           
            Destroy(gameObject);
        }
    }

    private void Heal(GameObject other) {
        other.GetComponent<HealthComponent>().TakeHeal(1);
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
