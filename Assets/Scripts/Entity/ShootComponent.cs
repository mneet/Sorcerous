using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootComponent : MonoBehaviour
{
    [SerializeField] bool playerControlled = false;
    [SerializeField] private GameObject bulletPreFab;
    private GameObject target;

    [SerializeField] private float fireRate = 0.5f;
    private float fireCooldown = 0f;
    public int bulletLevel = 0;

    public void ShootBullet() {

        fireCooldown -= Time.deltaTime;

        if (fireCooldown <= 0f) {
            Vector3 position = transform.position;
            Quaternion rotation = transform.rotation;

            GameObject bullet = Instantiate(bulletPreFab, position, rotation);
            bullet.GetComponent<Bullet>().parent = gameObject;
            bullet.GetComponent<Bullet>().parentTag = tag;
            SetBulletDirection(bullet);
            fireCooldown = fireRate;
        }

    }
    
    public void SetBulletDirection(GameObject bullet) {
        if (!playerControlled) {
            if (gameObject.GetComponent<MovementComponent>().fixedPosition) {
                if (target != null) {
                    Vector3 direction = (target.transform.position - transform.position).normalized;
                    Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
                    bullet.transform.rotation = rotation;
                    bullet.GetComponent<Bullet>().direction = direction;
                }
            }
            else {
                Vector3 mobDirection = gameObject.GetComponent<MovementComponent>().GetVector3Direction();
                bullet.GetComponent<Bullet>().direction = mobDirection;
                Quaternion rotation = Quaternion.LookRotation(mobDirection, Vector3.up);
                bullet.transform.rotation = rotation;
            }
        }
        else {
            if (Perspective.Instance.perspective == Perspective.PerspectiveOptions.topDown) {
                bullet.GetComponent<Bullet>().direction = -(bullet.transform.position - Utility.Instance.GetMouseDirectionTopDown()).normalized;
            }
        }
    }

    private void Awake() {
        if (!playerControlled) {
            target = GameObject.Find("Player");
            fireCooldown = Random.Range(0, fireCooldown);
        }
    }
   
    // Update is called once per frame
    void Update()
    {
        if (!playerControlled) {
            ShootBullet();
        }
    }
}
