using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Door : MonoBehaviour
{
    private Rigidbody rb;

    [SerializeField]
    private Transform followPoint;
    [SerializeField] [Range(0,1)]
    private float followDragAmt;

    [SerializeField]
    private float maxDistanceToMouse = 5.0f;
    [SerializeField]
    private float reach = 1.5f;
    private Vector3 mouseOffset;

    [SerializeField]
    private Camera cam;

    [Header("Movement")]
    [SerializeField]
    private float moveSpeed = 5.0f;

    [Header("Rotation")]
    [SerializeField]
    private float rotateSpeed = 10.0f;
    private float rotateDirection;

    private GameManager gm;

    // Input
    private Vector2 mousePos;

    private void Start() {
        rb = GetComponent<Rigidbody>();
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        if(gm == null) {
            Debug.Log("Door: GameManager not found");
        }
    }

    private void FixedUpdate() {
        if(!gm.GetPaused()) {
            if(followPoint != null) {
                mousePos = Mouse.current.position.ReadValue();
                Vector3 point = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, transform.position.z));
                Vector3 directionToMouse = point - transform.position;

                float distanceToMouse = Mathf.Clamp(Mathf.Abs(directionToMouse.magnitude), 0, maxDistanceToMouse);

                mouseOffset = directionToMouse.normalized * (distanceToMouse / maxDistanceToMouse) * reach;
                mouseOffset = new Vector3(mouseOffset.x, mouseOffset.y, 0);

                Vector3 toPoint = Vector3.Lerp(transform.position, followPoint.position + mouseOffset, followDragAmt);
                Vector3 moveDir = toPoint - transform.position;

                rb.velocity = moveDir * moveSpeed;
            } else {
                Debug.Log("Door: Follow Point not defined");
            }

            // STRETCH GOAL: Want to make this a lerp somehow

            // Rotates the door
            transform.Rotate(new Vector3(0, 0, rotateDirection * rotateSpeed * Time.deltaTime), Space.Self);

            // Clamps the rotation
            if(transform.localEulerAngles.z > 90 && transform.localEulerAngles.z < 180) {
                // Clamp z to 90
                Debug.Log("Clamp z to 90");
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 90);
            } else if (transform.localEulerAngles.z < 270 && transform.localEulerAngles.z > 180) {
                // Clamp z to 270
                Debug.Log("Clamp z to 270");
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 270);
            }
        } else {
            rb.velocity = Vector3.zero;
        }
    }

    public void RotateClockwise(InputAction.CallbackContext value) {
        // Right Click
        if(value.performed) {
            rotateDirection -= 1;
        } else if(value.canceled) {
            rotateDirection += 1;
        }
    }

    public void RotateCounterClockwise(InputAction.CallbackContext value) {
        // Left Click
        if(value.performed) {
            rotateDirection += 1;
        } else if(value.canceled) {
            rotateDirection -= 1;
        }
    }
}
