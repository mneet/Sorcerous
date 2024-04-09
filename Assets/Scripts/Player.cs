using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Perspective;

public class Player : MonoBehaviour {
    [SerializeField] private float movementSpeed = 10f;
    [SerializeField] private Perspective perspectiveController;
    [SerializeField] private GameInput gameInput;

    private void HandleMovement() {
        Vector2 movVector = gameInput.GetMovementVectorNormalized();

        Vector3 movDir = new Vector3(0,0,0);

        switch (perspectiveController.perspective) {

            case PerspectiveOptions.topDown:
                movDir = new Vector3(movVector.x, 0, movVector.y);
                break;

            case PerspectiveOptions.sideScroler:
                movDir = new Vector3(movVector.x, movVector.y, 0);
                break;

            case PerspectiveOptions.thirdPerson:
                movDir = new Vector3(0, movVector.y, movVector.x * -1);
                break;
        }
       
        transform.position += movDir * movementSpeed * Time.deltaTime;
        MoveToCenter();
    }

    private void MoveToCenter() {
        Vector3 movDir = new Vector3(0, 0, 0);

        switch (perspectiveController.perspective) {

            case PerspectiveOptions.topDown:
                movDir = new Vector3(transform.position.x, 0, transform.position.z);
                break;

            case PerspectiveOptions.sideScroler:
                movDir = new Vector3(transform.position.x, transform.position.y, 0);
                break;

            case PerspectiveOptions.thirdPerson:
                movDir = new Vector3(0, transform.position.y, transform.position.z);
                break;
        }
        transform.position = Vector3.MoveTowards(transform.position, movDir, Time.deltaTime * movementSpeed);
    }


    // Update is called once per frame
    void Update() {
        HandleMovement();
    }
}
