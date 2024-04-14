using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Perspective;
using UnityEngine.EventSystems;

public class MovementComponent : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 10f;
    [SerializeField] private Perspective perspectiveController;

    [SerializeField] private enum MovementDirection {
        UP,
        DOWN,
        LEFT,
        RIGHT
    }
    [SerializeField] private MovementDirection movementDirectionEnum;

    [SerializeField] private Vector2 movementDirection;


    private void BasicStraightMovement() {
        Vector3 movDir = new Vector3(0, 0, 0);

        switch (perspectiveController.perspective) {

            case PerspectiveOptions.topDown:
                movDir = new Vector3(movementDirection.x, 0, movementDirection.y);
                break;

            case PerspectiveOptions.sideScroler:
                movDir = new Vector3(movementDirection.x, movementDirection.y, 0);
                break;

            case PerspectiveOptions.thirdPerson:
                movDir = new Vector3(0, movementDirection.y, movementDirection.x * -1);
                break;
        }

        transform.position += movDir * movementSpeed * Time.deltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        BasicStraightMovement();
    }
}
