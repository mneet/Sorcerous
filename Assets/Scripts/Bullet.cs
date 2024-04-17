using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject parent;
    public float speed = 15.0f;
    public Vector3 direction = Vector3.forward;
    public float damage = 2f;
    

    private void CheckOutOfScreen() {
        if (transform.position.x > 20 || transform.position.y > 20 || transform.position.z > 20) {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other) {

        if (other.gameObject != parent) {
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
