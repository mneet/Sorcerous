using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] private bool immortal = false;

    private Stats stats;

    private GameManager gameManager;
    private WaveManager waveManager;
    private bool isPlayer;

    // Apply damage to entity
    public void TakeDamage(float damage) {
        stats.health -= damage;

        if (isPlayer) gameManager.UpdateHealthUI((int)stats.health);
     
        if (stats.health <= 0 && !immortal) {
            if (!isPlayer) {
                gameManager.ScorePoint(1);
                DestroySelfEnemy();
            }
            
        }
    }

    // Apply heal to entity
    public void TakeHeal(float heal) {
        stats.health += heal;
        if (isPlayer) gameManager.UpdateHealthUI((int)stats.health);
    }

    private void DestroySelfEnemy() {
        waveManager.MobDestroyed(gameObject);
        Destroy(gameObject);
    }

    // Setup Health Component variables
    private void Awake() {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        waveManager = gameManager.GetComponent<WaveManager>();
        stats = gameObject.GetComponent<Stats>();
        isPlayer = CompareTag("Player");
    }

    private void Start() {
        if (isPlayer) gameManager.UpdateHealthUI((int)stats.health);
    }

}
