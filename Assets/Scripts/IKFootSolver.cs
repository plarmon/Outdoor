using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class IKFootSolver : MonoBehaviour
{
    [SerializeField]
    private Transform body;
    [SerializeField]
    private float footSpacing;
    [SerializeField]
    private float footHeightOffset;
    [SerializeField]
    private LayerMask floorLayer;
    [SerializeField]
    private float stepDistance;
    [SerializeField]
    private float stepHeight;
    [SerializeField]
    private float speed;

    private float lerp;

    private Vector3 currentPosition;
    private Vector3 newPosition;
    private Vector3 oldPosition;

    private void Start() {
        currentPosition = transform.position;
        oldPosition = currentPosition;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = currentPosition;

        Ray ray = new Ray(body.position + (body.right * footSpacing), Vector3.down);
        if(Physics.Raycast(ray, out RaycastHit info, 10, floorLayer)) {
            if(Vector3.Distance(newPosition, info.point) > stepDistance) {
                lerp = 0;
                newPosition = info.point;
                // transform.position = info.point;
            }
            if(lerp < 1) {
                Vector3 footPosition = Vector3.Lerp(oldPosition, newPosition, lerp);
                footPosition.y +=- (Mathf.Sin(lerp * Mathf.PI) * stepHeight) + footHeightOffset;

                currentPosition = footPosition;
                lerp += Time.deltaTime * speed;
            } else {
                oldPosition = newPosition;
            }
        }
    }
}
