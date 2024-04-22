using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject parent;
    public string parentTag;
    public float speed = 15.0f;
    public Vector3 direction = Vector3.forward;
    public float damage = 2f;
    

    private void CheckOutOfScreen() {
        if (transform.position.x > 30 || transform.position.y > 30 || transform.position.z > 30) {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other) {

        if (!other.gameObject.CompareTag(parentTag)) {
            HealthComponent health = other.GetComponent<HealthComponent>();
            if (health != null) {
                health.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
    }

    void Update()
    {
        transform.position += (direction * speed) * Time.deltaTime;
        CheckOutOfScreen();
    }

}
