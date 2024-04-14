using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Perspective;
using UnityEngine.EventSystems;

public class PlayerMovementComponent : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 10f;
    [SerializeField] private Perspective perspectiveController;
    [SerializeField] private GameInput gameInput;

    private bool lockMovement = false;

    private void HandleMovement() {
        Vector2 movVector = gameInput.GetMovementVectorNormalized();

        Vector3 movDir = new Vector3(0, 0, 0);

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
        LimitPlayerMovement();
        MoveToCenter();
    }

    private void MoveToCenter() {

        Vector3 movDir = transform.position;
        switch (perspectiveController.perspective) {

            case PerspectiveOptions.topDown:
                movDir.y = 0;
                break;

            case PerspectiveOptions.sideScroler:
                movDir.z = 0;
                break;

            case PerspectiveOptions.thirdPerson:
                movDir.x = 0;
                break;
        }
        transform.position = Vector3.MoveTowards(transform.position, movDir, (movementSpeed * Time.deltaTime));
    }

    private void LimitPlayerMovement() {
        Vector3 objectPosition = transform.position;
        switch (perspectiveController.perspective) {
            case PerspectiveOptions.topDown:
                objectPosition.x = Mathf.Clamp(objectPosition.x, perspectiveController.topDownWidthMin, perspectiveController.topDownWidthMax);
                objectPosition.z = Mathf.Clamp(objectPosition.z, perspectiveController.topDownHeightMin, perspectiveController.topDownHeightMax);
                break;

            case PerspectiveOptions.sideScroler:
                objectPosition.x = Mathf.Clamp(objectPosition.x, perspectiveController.sideScrollerWidthMin, perspectiveController.sideScrollerWidthMax);
                objectPosition.y = Mathf.Clamp(objectPosition.y, perspectiveController.sideScrollerHeightMin, perspectiveController.sideScrollerHeightMax);
                break;

            case PerspectiveOptions.thirdPerson:
                objectPosition.z = Mathf.Clamp(objectPosition.z, perspectiveController.thirdPersonWidthMin, perspectiveController.thirdPersonWidthMax);
                objectPosition.y = Mathf.Clamp(objectPosition.y, perspectiveController.thirdPersonHeightMin, perspectiveController.thirdPersonHeightMax);
                break;
        }
        transform.position = objectPosition;
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
    }
}
