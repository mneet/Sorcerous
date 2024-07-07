using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] private bool immortal = false;

    private Stats stats;
    private bool isPlayer;
    private bool colisionFlag = false;

    // Apply damage to entity
    public void TakeDamage(float damage) {
        stats.health -= damage * (1.0f - (stats.armor / 100));

        if (isPlayer) GameManager.Instance.UpdateHealthUI(stats.health / stats.maxHealth);
     
        if (stats.health <= 0 && !immortal) {
            if (!isPlayer) {
                GameManager.Instance.ScorePoint(1);
                if (gameObject.GetComponent<ItemDrop>() != null) {
                    gameObject.GetComponent<ItemDrop>().DestroySelf();
                    Debug.Log("destroying self");
                }
                else {
                    Destroy(gameObject);
                }
            }
            
        }
        GameObject AudioMan = GameObject.Find("AudioManager");
        AudioMan.GetComponent<AudioController>().TocarSFX(0);
    }

    // Apply heal to entity
    public void TakeHeal(float heal) {
        stats.health += heal;
        if (isPlayer) GameManager.Instance.UpdateHealthUI((int)stats.health);
    }

    private void DestroySelfEnemy() {
        WaveManager.Instance.MobDestroyed(gameObject);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other) {
        if (!other.gameObject.CompareTag(tag) && (!isPlayer && other.gameObject.CompareTag("Player")) && !colisionFlag) {
            HealthComponent health = other.GetComponent<HealthComponent>();
            if (health != null) {
                health.TakeDamage(stats.bulletDamage);
            }
            colisionFlag = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            colisionFlag = false;
        }
    }

    // Setup Health Component variables
    private void Awake() {
        stats = gameObject.GetComponent<Stats>();
        isPlayer = CompareTag("Player");
    }

    private void Start() {
        if (isPlayer) GameManager.Instance.UpdateHealthUI((int)stats.health);
    }

}
