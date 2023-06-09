using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Animator transitionAnimator;
    public GameObject transitionRenderTexture;
    public bool isStartingClosed;
    private int isStartingClosedHash;
    private int closeDoorHash;

    public static GameManager Instance;

    private bool paused;

    private void Awake() {
        closeDoorHash = Animator.StringToHash("CloseDoor");
        isStartingClosedHash = Animator.StringToHash("isStartingClosed");

        Instance = this;
    }

    private void Start() {
        paused = false;
        if(isStartingClosed) {
            transitionAnimator.SetTrigger(isStartingClosedHash);
        }
    }

    public void TriggerOpen() {
        transitionAnimator.SetTrigger(isStartingClosedHash);
    }

    public void PausePlayer(bool isPaused) {
        paused = isPaused;
    }

    public bool GetPaused() {
        return paused; 
    }

    public void TransistionScene(string sceneName) {
        StartCoroutine(CloseDoor(sceneName));
    }

    private IEnumerator CloseDoor(string sceneName) {
        if(transitionAnimator != null) {
            transitionAnimator.SetTrigger(closeDoorHash);
            // 
            /* if(transitionRenderTexture != null) {
                transitionRenderTexture.SetActive(true);
            } else {
                Debug.Log("GameManager: trasition renderTexture not set");
            } */
        } else {
            Debug.Log("GameManager: animator not set");
        }
        transitionRenderTexture.SetActive(true);
        yield return new WaitForSeconds(3.0f);
        SceneManager.LoadScene(sceneName);
    }
}
