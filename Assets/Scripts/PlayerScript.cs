using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private float speed = 1f;
    [SerializeField] private float rotationSpeed = 10f;

    private void movement1()
    {
        Vector3 movement = new Vector3(0f, 0f, 0f);

        if (Input.GetKeyDown(KeyCode.D))
        {
            movement.x += speed;

        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            movement.x -= speed;
        }

        if (Input.GetKeyDown(KeyCode.S)) 
        {
            movement.z -= speed; 
        }
        else if (Input.GetKeyDown(KeyCode.W)) 
        { 
            movement.z += 1; 
        }

        transform.Translate(movement);
    }

    private void movement2()
    {
        Vector3 movement = new Vector3(0f, 0f, 0f);

        movement.x = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        movement.z = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        transform.Translate(movement);
    }
    
    private void movement3()
    {
        Vector3 rotation = new Vector3(0f, 0f, 0f);
        Vector3 movement = new Vector3(0f, 0f, 0f);

        rotation.y = Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;
        movement.z = Input.GetAxis("Vertical") * speed * Time.deltaTime;

        transform.Rotate(rotation);
        transform.Translate(movement);
    }

    // Update is called once per frame
    void Update()
    {
        movement3();
    }
}


