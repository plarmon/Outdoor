using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingAnchor : MonoBehaviour
{
    [SerializeField]
    private float rotateSpeed;
    [SerializeField]
    private bool rotateOnY;

    // Update is called once per frame
    void Update()
    {
        if(!rotateOnY) {
            transform.Rotate(new Vector3(0, 0, rotateSpeed * Time.deltaTime), Space.Self);
        } else {
            transform.Rotate(new Vector3(0, rotateSpeed * Time.deltaTime, 0), Space.Self);
        }
    }
}
