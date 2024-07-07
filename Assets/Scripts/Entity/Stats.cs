using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    public bool isPlayer = false;
    public float maxHealth;
    public float health;
    public float armor;

    public float movementSpeed;

    public float bulletDamage;
    public float fireRate;
    public int bulletLevel;

    private void Awake() {
        health = maxHealth;
    }

    private void Start() {
        if (!isPlayer) {
            float modifier = WaveManager.Instance.progressionModifier[WaveManager.Instance.progressionLevel];
            health *= modifier;
            maxHealth *= modifier;
            movementSpeed *= modifier;
            bulletDamage *= modifier;
            fireRate /= modifier;
        }
    }

    public void UpgradeFire() {
        fireRate -= (float)(fireRate * 0.1);
    }
    public void UpgradeEarth() {
        maxHealth += 1;
    }
    public void UpgradeWind() {
        movementSpeed -= (float)(movementSpeed * 0.1);
    }
}
