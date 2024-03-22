using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Perspective;

public class Player : MonoBehaviour {
    [SerializeField] private float movementSpeed = 10f;
    [SerializeField] private Perspective perspectiveController;

    private void TopDownMovement() {
        Vector3 movVector = new Vector3(0f, 0f, 0f);

        movVector.x = Input.GetAxis("Horizontal");
        movVector.z = Input.GetAxis("Vertical");
        movVector = movVector.normalized;

        transform.position += movVector * movementSpeed * Time.deltaTime;
    }
    private void SideScrollerMovement() {
        Vector3 movVector = new Vector3(0f, 0f, 0f);

        movVector.x = Input.GetAxis("Horizontal");
        movVector.y = Input.GetAxis("Vertical");
        movVector = movVector.normalized;

        transform.position += movVector * movementSpeed * Time.deltaTime;
    }

    private void ThirdPersonMovement() {
        Vector3 movVector = new Vector3(0f, 0f, 0f);

        movVector.z = Input.GetAxis("Horizontal") * -1;
        movVector.y = Input.GetAxis("Vertical");
        movVector = movVector.normalized;

        transform.position += movVector * movementSpeed * Time.deltaTime;
    }

    // Update is called once per frame
    void Update() {
        switch (perspectiveController.perspective) {
            case PerspectiveOptions.topDown:
                TopDownMovement();
                break;

            case PerspectiveOptions.sideScroler:
                SideScrollerMovement();
                break;

            case PerspectiveOptions.thirdPerson:
                ThirdPersonMovement();
                break;

        }
    }
}
