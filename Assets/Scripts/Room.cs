using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Room : MonoBehaviour
{
    [SerializeField]
    private GameObject nextVC;
    [SerializeField]
    private GameObject thisVC;
    [SerializeField]
    private Transform TravelToPos;

    private GameObject playerRef;

    private bool moving;

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.CompareTag("Player")) {
            if(playerRef == null) {
                playerRef = other.transform.parent.parent.gameObject;
            }
            if(nextVC != null) {
                if(!GameManager.Instance.GetPaused()) {
                    nextVC.SetActive(true);
                    thisVC.SetActive(false);
                    GameManager.Instance.PausePlayer(true);
                    playerRef.transform.Translate(transform.right * 2, Space.Self);
                    StartCoroutine(TransitionWait());
                }
            }
        }
    }

    private IEnumerator TransitionWait() {
        yield return new WaitForSecondsRealtime(0.5f);
        GameManager.Instance.PausePlayer(false);
    }
}
