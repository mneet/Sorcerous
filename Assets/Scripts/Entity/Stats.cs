using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
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
