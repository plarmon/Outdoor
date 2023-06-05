using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private bool paused;

    private void Awake() {
        if(Instance == null){
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    private void Start() {
        paused = false;
    }

    public void PausePlayer(bool isPaused) {
        paused = isPaused;
    }

    public bool GetPaused() {
        return paused; 
    }
}
