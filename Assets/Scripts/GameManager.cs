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

    public void Quit() {
        Application.Quit();
    }

    public void RestartLevelWithWait(float waitTime) {
        Debug.Log("Hit 1");
        StartCoroutine(RestartLevel(waitTime));
    }

    private IEnumerator RestartLevel(float waitTime) {
        Debug.Log("Hit 2");
        yield return new WaitForSeconds(waitTime);
        Debug.Log(SceneManager.GetActiveScene().name);
        StartCoroutine(CloseDoor(SceneManager.GetActiveScene().name));
    }

    public void TriggerOpen() {
        StartCoroutine(OpenDoor());
    }

    private IEnumerator OpenDoor() {
        transitionAnimator.SetTrigger(isStartingClosedHash);
        yield return new WaitForSeconds(3.0f);
        transitionRenderTexture.SetActive(false);
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
        } else {
            Debug.Log("GameManager: animator not set");
        }
        transitionRenderTexture.SetActive(true);
        yield return new WaitForSeconds(3.0f);
        SceneManager.LoadScene(sceneName);
    }
}
