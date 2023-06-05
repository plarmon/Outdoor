using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevel : MonoBehaviour
{
    private LevelManager lm;
    private GameManager gm;
    [SerializeField]
    private GameObject scoreSheetUI;

    private void Start() {
        lm = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.CompareTag("Player")) {
            // End the level
            lm.CalculcateGrade();
            lm.SetIsLevelOver(true);
            scoreSheetUI.SetActive(true);
            gm.PausePlayer(true);
        }
    }
}
