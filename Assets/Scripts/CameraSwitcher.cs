using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{

    [SerializeField] private Animator animator;

    private bool topDownCamera = true;


    private void Awake() {
        animator = GetComponent<Animator>();
    }

    private void switchState() {
        if (topDownCamera) {
            animator.Play("TopDown");
        }
        else {
            animator.Play("SideScroller");
        }
        topDownCamera = !topDownCamera;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            switchState();
        }   
    }
}

