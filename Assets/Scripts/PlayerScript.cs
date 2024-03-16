using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 10f;

    private void TopDownMovement() {
        Vector3 movVector = new Vector3(0f, 0f, 0f);

        movVector.x = Input.GetAxis("Horizontal") * movementSpeed * Time.deltaTime;
        movVector.z = Input.GetAxis("Vertical") * movementSpeed * Time.deltaTime;

        transform.Translate(movVector);
    }

    // Update is called once per frame
    void Update()
    {
        TopDownMovement();
    }
}
