using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenarioController : MonoBehaviour
{
    [SerializeField] private Vector3 topDownPosition;
    [SerializeField] private Vector3 sideScroller;

    [SerializeField] private float movementSpeed;

    // Update is called once per frame
    void Update()
    {
        Vector3 destPos = new Vector3(0f,0f, 0f);
        switch(Perspective.Instance.perspective) {
            case Perspective.PerspectiveOptions.topDown:
                destPos = topDownPosition;
                break;
            case Perspective.PerspectiveOptions.sideScroler:
                destPos = sideScroller;
                break;
        }
        transform.position = Vector3.MoveTowards(transform.position, destPos, movementSpeed * Time.deltaTime);
    }
}
