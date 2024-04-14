using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perspective : MonoBehaviour
{
    [SerializeField] private Animator animator;
    public enum PerspectiveOptions {
        topDown,
        sideScroler,
        thirdPerson
    }
    public PerspectiveOptions perspective;

    // Screen Limits
    public float topDownWidthMin;
    public float topDownWidthMax;
    public float topDownHeightMin;
    public float topDownHeightMax;

    public float sideScrollerWidthMin;
    public float sideScrollerWidthMax;
    public float sideScrollerHeightMin;
    public float sideScrollerHeightMax;

    public float thirdPersonWidthMin;
    public float thirdPersonWidthMax;
    public float thirdPersonHeightMin;
    public float thirdPersonHeightMax;


    private void Awake() {
        animator = GetComponent<Animator>();
    }

    public void switchState(PerspectiveOptions perspective) {

        switch (perspective) {
            case PerspectiveOptions.topDown:
                animator.Play("TopDown");
                break;

            case PerspectiveOptions.sideScroler:
                animator.Play("SideScroller");
                break;

            case PerspectiveOptions.thirdPerson:
                animator.Play("ThirdPerson");
                break;

        }
    }


    void DebugSwitchPerspective() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            int enumLen = System.Enum.GetValues(typeof(PerspectiveOptions)).Length - 1;
            perspective++;
            if ((int)perspective > enumLen) perspective = 0;
            switchState(perspective);
        }
    }

    private void Update() {
        DebugSwitchPerspective();
    }
}
