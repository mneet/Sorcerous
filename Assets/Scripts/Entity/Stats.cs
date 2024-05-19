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
}
