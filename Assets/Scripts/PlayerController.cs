using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Movement
    [SerializeField]
    private float speed;
    [SerializeField]
    private AudioSource gruntingSoundSource;

    [SerializeField]
    private Animator feetAnimator;
    private int speedHash;
    private int speedActualHash;

    // Input
    private float horizontalInput;
    private float verticalInput;

    private GameManager gm;

    private void Start() {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        if(gm == null) {
            Debug.Log("PlayerController: GameManager not found");
        }
        
        horizontalInput = 0.0f;

        speedHash = Animator.StringToHash("speed");
        speedActualHash = Animator.StringToHash("speedActual");
    }

    private void FixedUpdate() {
        if(!gm.GetPaused()) {
            transform.Translate(new Vector3(horizontalInput * speed * Time.deltaTime, 0, 0), Space.Self);
            if(feetAnimator != null) {
                feetAnimator.SetInteger(speedHash, Mathf.RoundToInt(horizontalInput));
                feetAnimator.SetFloat(speedActualHash, horizontalInput);
            } else {
                Debug.Log("PlayerController: feetAnimator not defined.");
            }
        }
    }

    public void MoveInput(InputAction.CallbackContext value) {
        if(!gm.GetPaused()) {
            horizontalInput = value.ReadValue<Vector2>().x;
            horizontalInput = Mathf.RoundToInt(horizontalInput);
            verticalInput = value.ReadValue<Vector2>().y;
            verticalInput= Mathf.RoundToInt(verticalInput);
        } else {
            horizontalInput = 0;
            verticalInput = 0;
            feetAnimator.SetInteger(speedHash, 0);
        }
    }
}
