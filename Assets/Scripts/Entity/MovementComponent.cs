using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Perspective;
using UnityEngine.EventSystems;

public class MovementComponent : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 10f;
    [SerializeField] private Perspective perspectiveController;
    private enum MovementDirection {
        UP,
        DOWN,
        LEFT,
        RIGHT
    }
    [SerializeField] private MovementDirection movementDirectionEnum;
    [SerializeField] private Vector2 movementDirection;

    public bool fixedPosition = false;
    public Vector3 targetPosition;
    
    private void BasicStraightMovement() {
        Vector3 movDir = new Vector3(0, 0, 0);

        switch (perspectiveController.perspective) {

            case PerspectiveOptions.topDown:
                movDir = new Vector3(movementDirection.x, 0, movementDirection.y);
                break;

            case PerspectiveOptions.sideScroler:
                movDir = new Vector3(0, movementDirection.y, movementDirection.x);
                break;
        }

        transform.position -= movDir * movementSpeed * Time.deltaTime;
    }

    private void MoveToFormation() {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, (movementSpeed * 3) * Time.deltaTime);
    }

    private void Awake() {
        perspectiveController = GameObject.Find("StateDrivenCamera").GetComponent<Perspective>();
    }
    void Update()
    {
        if (fixedPosition) {
            MoveToFormation();
        }
        else {
            BasicStraightMovement();
        }
    }
}
