using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Perspective;
using UnityEngine.EventSystems;
using System;

public class MovementComponent : MonoBehaviour
{
    [SerializeField] private float screenHeight = 25f;
    [SerializeField] private float screenWidth = 46f;
    [SerializeField] private float movementSpeed = 10f;
    [SerializeField] private Perspective perspectiveController;
    public enum MovementBehaviour {
        STRAIGHT,
        DIAGONAL
    }
    [SerializeField] private MovementBehaviour movementBehaviour;
    private enum MovementDirection {
        RIGHT,
        LEFT,
        UP,
        DOWN
    }
    [SerializeField] private MovementDirection movementDirection;
    [SerializeField] private Vector2 movementDirectionVector;
    public Vector3 movementDirectionVector3;

    public bool fixedPosition = false;
    public Vector3 targetPosition;
    

    public void RandomizeMovementDirection() {
        switch (movementBehaviour) {
            case MovementBehaviour.DIAGONAL:
                // faz nada
                break;
            case MovementBehaviour.STRAIGHT:
                RandomizeStraightDirection();
                break;
        }
    }
    private void RandomizeStraightDirection() {
        MovementDirection direction = GetRandomDirection();
        movementDirection = direction;
        movementDirectionVector = new Vector2(0, 0);

        float screenHeightCenter = screenHeight / 2;
        float screenWidthCenter = screenWidth / 2;

        Vector2 newPosition = new Vector2(0, 0);
        switch (direction) {
            case MovementDirection.RIGHT:
                movementDirectionVector.x = 1;
                newPosition.x = UnityEngine.Random.Range(-screenWidthCenter - 20, -screenWidthCenter);
                newPosition.y = UnityEngine.Random.Range(-screenHeightCenter + 2, screenHeightCenter - 2);
                break;
            case MovementDirection.LEFT:
                movementDirectionVector.x = -1;
                newPosition.x = UnityEngine.Random.Range(screenWidthCenter, screenWidthCenter + 20);
                newPosition.y = UnityEngine.Random.Range(-screenHeightCenter + 2, screenHeightCenter - 2);
                break;
            case MovementDirection.UP:
                movementDirectionVector.y = 1;
                newPosition.x = UnityEngine.Random.Range(-screenWidthCenter + 2, screenWidthCenter - 2);
                newPosition.y = UnityEngine.Random.Range(-screenHeightCenter, -screenHeightCenter - 20);
                break;
            case MovementDirection.DOWN:
                movementDirectionVector.y = -1;
                newPosition.x = UnityEngine.Random.Range(-screenWidthCenter + 2, screenWidthCenter - 2);
                newPosition.y = UnityEngine.Random.Range(screenHeightCenter, screenHeightCenter + 20);
                break;
        }
        ApplyPosition(newPosition);
    }
    private void ApplyPosition(Vector2 newPosition) {

        Vector3 objectPosition = new Vector3();
        switch (perspectiveController.perspective) {
            case PerspectiveOptions.topDown:
                objectPosition.x = newPosition.x;
                objectPosition.z = newPosition.y;
                break;

            case PerspectiveOptions.sideScroler:
                objectPosition.z = newPosition.x;
                objectPosition.y = newPosition.y;
                break;
        }

        transform.position = objectPosition;
    }
    private MovementDirection GetRandomDirection() {
        // Obtém todos os valores do enum como um array
        Array values = Enum.GetValues(typeof(MovementDirection));

        // Gera um índice aleatório entre 0 e o comprimento do array
        int randomIndex = UnityEngine.Random.Range(0, values.Length);

        // Retorna o valor correspondente ao índice aleatório
        return (MovementDirection)values.GetValue(randomIndex);
    }

    private void BasicStraightMovement() {
        Vector3 movDir = new Vector3(0, 0, 0);

        switch (perspectiveController.perspective) {

            case PerspectiveOptions.topDown:
                movementDirectionVector3 = new Vector3(movementDirectionVector.x, 0, movementDirectionVector.y);
                movDir = movementDirectionVector3;
                break;

            case PerspectiveOptions.sideScroler:
                movementDirectionVector3 = new Vector3(0, movementDirectionVector.y, movementDirectionVector.x);
                movDir = movementDirectionVector3;
                break;
        }

        transform.position += movDir * movementSpeed * Time.deltaTime;
    }
    private void MoveToFormation() {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, (movementSpeed * 3) * Time.deltaTime);
    }
    private void CheckOutOfBorder() {
        bool xLimit = transform.position.x < -screenWidth || transform.position.x > screenWidth;
        bool yLimit = transform.position.y < -screenHeight || transform.position.y > screenHeight;
        bool zLimit = transform.position.z < -screenWidth || transform.position.z > screenWidth;

        if (xLimit || yLimit || zLimit) RandomizeMovementDirection();
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
        CheckOutOfBorder();
    }
}
