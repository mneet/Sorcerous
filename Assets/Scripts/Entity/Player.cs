using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static Perspective;

public class Player : MonoBehaviour
{
    // Attributes
    public float movementSpeed = 10f;

    [SerializeField] private GameObject bulletPreFab;

    // Game Systems
    [SerializeField] private LayerMask mouseLayer;
    private PlayerInput playerInput;

    // Flags
    private bool lockMovement = false;

    private void Awake() {

        playerInput = gameObject.GetComponent<PlayerInput>();
    }

    #region Player Movement
    // Use input to move the player
    private void HandleMovement() {
        Vector2 movVector = playerInput.GetMovementVectorNormalized();

        Vector3 movDir = new Vector3(0, 0, 0);

        switch (Perspective.Instance.perspective) {

            case PerspectiveOptions.topDown:
                movDir = new Vector3(movVector.x, 0, movVector.y);
                break;

            case PerspectiveOptions.sideScroler:
                movDir = new Vector3(0, movVector.y, movVector.x);
                break;
        }

        transform.position += movDir * movementSpeed * Time.deltaTime;
        LimitPlayerMovement();
        MoveToCenter();
    }

    // Centralize the player based on the current perspective
    private void MoveToCenter() {

        Vector3 movDir = transform.position;
        switch (Perspective.Instance.perspective) {

            case PerspectiveOptions.topDown:
                movDir.y = 0;
                break;

            case PerspectiveOptions.sideScroler:
                movDir.x = 0;
                break;
        }
        transform.position = Vector3.MoveTowards(transform.position, movDir, (movementSpeed * Time.deltaTime));
    }

    // Limit player position inside the window
    private void LimitPlayerMovement() {
        Vector3 objectPosition = transform.position;
        switch (Perspective.Instance.perspective) {
            case PerspectiveOptions.topDown:
                objectPosition.x = Mathf.Clamp(objectPosition.x, Perspective.Instance.topDownWidthMin, Perspective.Instance.topDownWidthMax);
                objectPosition.z = Mathf.Clamp(objectPosition.z, Perspective.Instance.topDownHeightMin, Perspective.Instance.topDownHeightMax);
                break;

            case PerspectiveOptions.sideScroler:
                objectPosition.z = Mathf.Clamp(objectPosition.z, Perspective.Instance.sideScrollerWidthMin, Perspective.Instance.sideScrollerWidthMax);
                objectPosition.y = Mathf.Clamp(objectPosition.y, Perspective.Instance.sideScrollerHeightMin, Perspective.Instance.sideScrollerHeightMax);
                break;
        }
        transform.position = objectPosition;
    }

    // Rotate player
    private void RotatePlayerMouse() {
        // Obter a posi��o do mouse na tela
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 mousePosition = new Vector3();
        if (Physics.Raycast(ray, out RaycastHit raycastHit, Mathf.Infinity, mouseLayer)) {
            mousePosition = raycastHit.point;
        }
        mousePosition.y = 0;
        transform.forward = transform.position - mousePosition;     
    }
    #endregion

    // Update is called once per frame
    void Update()
    {
        if (Perspective.Instance.perspective == PerspectiveOptions.topDown) RotatePlayerMouse();
        else transform.rotation = Quaternion.identity;

        HandleMovement();  
    }
}
