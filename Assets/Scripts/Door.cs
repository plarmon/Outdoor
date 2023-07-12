using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Door : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rb;

    [SerializeField]
    private GameObject model;

    [SerializeField]
    public Transform followPoint;
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

    [Header("Health/Damage")]
    [SerializeField]
    private GameObject brokenDoorPrefab;
    [SerializeField]
    private Material crackedMat;
    private bool cracked = false;
    [SerializeField]
    private float maxHealth;
    public float currentHealth;
    private float previousHealth;

    [Header("Sounds")]
    [SerializeField]
    private AudioSource doorAudioSource;
    [SerializeField]
    private AudioClip woodBreak;
    [SerializeField]
    private AudioClip[] woodCracks;
    [SerializeField]
    private AudioSource gruntingSource;
    [SerializeField]
    private float gruntingVelocity = 2.0f;
    private bool isGrunting;

    private GameManager gm;

    private InputAction leftMouseClick;
    private InputAction rightMouseClick;

    // Input
    private Vector2 mousePos;

    private void Awake() {
/*         leftMouseClick = new InputAction(binding: "<Mouse>/leftButton");
        leftMouseClick.performed += ctx => LeftMouseClicked();
        leftMouseClick.canceled += ctx => LeftMouseReleased();
        leftMouseClick.Enable();

        rightMouseClick = new InputAction(binding: "<Mouse>/rightButton");
        rightMouseClick.performed += ctx => RightMouseClicked();
        rightMouseClick.canceled += ctx => RightMouseReleased();
        rightMouseClick.Enable(); */
    }

    private void Start() {
        rb = GetComponent<Rigidbody>();
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        if(gm == null) {
            Debug.Log("Door: GameManager not found");
        }

        currentHealth = maxHealth;
        previousHealth = currentHealth;
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

            // Not a fan of how this sounds
/*             Debug.Log(rb.velocity.magnitude);
            if(rb.velocity.magnitude >= gruntingVelocity) {
                if(!isGrunting) {
                    StartGrunting();
                }
            } else {
                if(isGrunting) {
                    StopGrunting();
                }
            } */
        } else {
            rb.velocity = Vector3.zero;
            // StopGrunting();
        }
    }

    private void StartGrunting() {
        isGrunting = true;
        gruntingSource.Play();
    }

    private void StopGrunting() {
        isGrunting = false;
        gruntingSource.Pause();
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

    public bool TakeDamage(float damage) {
        if(!gm.GetPaused()) {
            currentHealth -= damage;
            // Play cracking sound
            if((currentHealth / maxHealth) <= 0.5f && !cracked) {
                cracked = true;
                model.GetComponent<MeshRenderer>().material = crackedMat;
            }
            if(currentHealth <= 0) {
                Break();
                return true;
            } else {
                // Play sound if needed
                if(previousHealth - currentHealth > 10) {
                    doorAudioSource.Stop();
                    doorAudioSource.clip = woodCracks[Random.Range(0,woodCracks.Length)];
                    doorAudioSource.Play();
                    previousHealth = currentHealth;
                }
                return false;
            }
        } else {
            return false;
        }
    }

    private void Break() {
        // Pause the Player
        gm.PausePlayer(true);
        // Spawn the broken door model
        GameObject brokenDoor = Instantiate(brokenDoorPrefab, transform.position, transform.rotation);
        brokenDoor.transform.Rotate(new Vector3(0, 0, 90), Space.Self);
        // Play sound
        GameObject breakSound = Instantiate(new GameObject(), transform.position, transform.rotation);
        breakSound.AddComponent(typeof(AudioSource));
        breakSound.GetComponent<AudioSource>().clip = woodBreak;
        breakSound.GetComponent<AudioSource>().Play();

        /* doorAudioSource.Stop();
        doorAudioSource.clip = woodBreak;
        doorAudioSource.Play(); */
        // Start coroutine to restart the game
        gm.RestartLevelWithWait(2.0f);
        Destroy(gameObject);
    }
}
