using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Perspective;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    // Game Systems
    [SerializeField] private Perspective perspectiveController;
    private PlayerInputActions playerInputActions;
    private Player player;

    private void Awake() {

        player = gameObject.GetComponent<Player>();

        // Set up input system
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        playerInputActions.Player.Shoot.performed += ShootInput;
    }

    #region Player Movement
    public Vector2 GetMovementVectorNormalized() {
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();

        inputVector = inputVector.normalized;

        return inputVector;
    }

    #endregion

    // Shoot when input is pressed
    public void ShootInput(InputAction.CallbackContext context) {
        player.ShootBullet();
    }

    // Update is called once per frame
    void Update()
    {
    }
}
