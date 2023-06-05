using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Movement
    [SerializeField]
    private float speed;

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
    }

    private void FixedUpdate() {
        if(!gm.GetPaused()) {
            transform.Translate(new Vector3(horizontalInput * speed * Time.deltaTime, 0, 0), Space.Self);
        }
    }

    public void MoveInput(InputAction.CallbackContext value) {
        horizontalInput = value.ReadValue<Vector2>().x;
        horizontalInput = Mathf.RoundToInt(horizontalInput);
        verticalInput = value.ReadValue<Vector2>().y;
        verticalInput= Mathf.RoundToInt(verticalInput);
    }
}
