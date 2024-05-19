using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility : MonoBehaviour
{
    public static Utility Instance;

    [SerializeField] private LayerMask mouseLayer;
    
    public Vector3 GetMouseDirectionTopDown() {
        // Obter a posição do mouse na tela
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 mousePosition = new Vector3();
        if (Physics.Raycast(ray, out RaycastHit raycastHit, Mathf.Infinity, mouseLayer)) {
            mousePosition = raycastHit.point;
        }
        mousePosition.y = 0;

        return mousePosition;
    }

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
        }
        else {
            Instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }
}
