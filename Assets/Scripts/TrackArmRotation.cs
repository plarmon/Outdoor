using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackArmRotation : MonoBehaviour
{
    [SerializeField]
    private GameObject trackedArm;

    // Update is called once per frame
    void Update()
    {
        // transform.rotation = trackedArm.transform.rotation;
        // transform.eulerAngles = new Vector3(trackedArm.transform.eulerAngles.x, trackedArm.transform.eulerAngles.y, trackedArm.transform.eulerAngles.z);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, trackedArm.transform.eulerAngles.z);
    }
}
