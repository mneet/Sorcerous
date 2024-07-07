using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Perspective;

public class Bullet : MonoBehaviour
{
    public GameObject parent;
    public string parentTag;
    public float speed = 15.0f;
    public Vector3 direction = Vector3.forward;
    public float damage = 2f;
    

    private void CheckOutOfScreen() {
        if (transform.position.x > 30 || transform.position.y > 30 || transform.position.z > 30 ||
            transform.position.x < -30 || transform.position.y < -30 || transform.position.z < -30) {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other) {

        if (!other.gameObject.CompareTag(parentTag) && !other.gameObject.CompareTag(tag)) {
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
        Vector3 movDir = transform.position;
        switch (Perspective.Instance.perspective) {

            case PerspectiveOptions.topDown:
                movDir.y = 0;
                break;

            case PerspectiveOptions.sideScroler:
                movDir.x = 0;
                break;
        }
        transform.position = Vector3.MoveTowards(transform.position, movDir, (speed * Time.deltaTime));
        CheckOutOfScreen();
    }

}
