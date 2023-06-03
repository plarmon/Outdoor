using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateArm : MonoBehaviour
{
    [SerializeField]
    private GameObject startObject;
    [SerializeField]
    private GameObject endObject;
    private Vector3 initialScale;

    private void Start() {
        initialScale = transform.localScale;
        // Change Scale
        UpdateTransformForScale();
    }

    private void FixedUpdate() {
        if(startObject.transform.hasChanged || endObject.transform.hasChanged) {
            // Change Scale
            UpdateTransformForScale();
        }
    }

    private void UpdateTransformForScale() {
        float distance = Vector3.Distance(startObject.transform.position, endObject.transform.position);
        transform.localScale = new Vector3(initialScale.x, distance/2f, initialScale.z);

        Vector3 middlePoint = (startObject.transform.position + endObject.transform.position) / 2f;
        transform.position = middlePoint;

        Vector3 rotationDirection = (endObject.transform.position - startObject.transform.position);
        transform.up = rotationDirection;
    }
}
