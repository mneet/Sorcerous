using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour 
{ 
    [SerializeField] private bool isEnemy = true;
    [SerializeField] private GameObject healPwrup;
    public GameObject spawner = null;
    public void DestroySelf() {
        GameObject.Destroy(gameObject);

        if (spawner != null && isEnemy) {
            spawner.GetComponent<WaveManager>().MobDestroyed(gameObject);
        }

        if (isEnemy) {
            int dice = Random.Range(0, 100);
            if (dice > 70) {
                Instantiate(healPwrup, transform.position, Quaternion.identity);
            }
        }
    }
}


