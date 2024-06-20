using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perspective : MonoBehaviour
{
    public static Perspective Instance;

    [SerializeField] private Animator animator;
    public enum PerspectiveOptions {
        topDown,
        sideScroler,
    }
    public PerspectiveOptions perspective;
    private PerspectiveOptions activePerspective;
    private PerspectiveOptions lastPerspective;

    // Screen Limits
    public float topDownWidthMin;
    public float topDownWidthMax;
    public float topDownHeightMin;
    public float topDownHeightMax;

    public float sideScrollerWidthMin;
    public float sideScrollerWidthMax;
    public float sideScrollerHeightMin;
    public float sideScrollerHeightMax;

    public float screenHeight;
    public float screenWidth;


    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
        }
        else {
            Instance = this;
        }
        animator = GetComponent<Animator>();
    }

    private void SwitchState(PerspectiveOptions perspective) {

        switch (perspective) {
            case PerspectiveOptions.topDown:
                animator.Play("TopDown");
                break;

            case PerspectiveOptions.sideScroler:
                animator.Play("SideScroller");
                break;

        }
        activePerspective = perspective;
    }

    public void SwitchPerspective(PerspectiveOptions newPerspective) {
        Debug.Log(newPerspective);
        lastPerspective = perspective;
        perspective = newPerspective;
        SwitchState(perspective);
    }
    
    public PerspectiveOptions GetRandomPerspective() {
        List<PerspectiveOptions> values = new List<PerspectiveOptions>();
        values.Add(PerspectiveOptions.topDown);
        values.Add(PerspectiveOptions.sideScroler);

        if (values.Contains(lastPerspective)) {
            values.Remove(lastPerspective);
        }
        PerspectiveOptions randomPerspective = values[Mathf.Max(UnityEngine.Random.Range(0, values.Count - 1), 0)];

        return randomPerspective;
    }

    void DebugSwitchPerspective() {
        if (Input.GetKeyDown(KeyCode.F1)) {
            int enumLen = System.Enum.GetValues(typeof(PerspectiveOptions)).Length - 1;
            perspective++;
            if ((int)perspective > enumLen) perspective = 0;
            SwitchState(perspective);
        }
    }

    private void Update() {
        //DebugSwitchPerspective();
        if (activePerspective != perspective) SwitchState(perspective);
    }
}
