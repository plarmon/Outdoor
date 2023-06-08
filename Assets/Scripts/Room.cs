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
    [SerializeField]
    private Transform personTransformPoint;

    private GameObject playerRef;

    private bool moving;

    private LevelManager lm;

    private void Start() {
        lm = GameObject.Find("LevelManager").GetComponent<LevelManager>();
    }

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
                    lm.MovePeople(personTransformPoint.position);
                }
            }
        }
    }

    private IEnumerator TransitionWait() {
        // yield return new WaitForSecondsRealtime(0.5f);
        Vector3 toPoint = playerRef.transform.position + (transform.right * 2);
        while(Vector3.Distance(playerRef.transform.position, toPoint) > 0.1f) {
            playerRef.transform.position = Vector3.Lerp(playerRef.transform.position, toPoint, 0.5f);
            yield return new WaitForEndOfFrame();
        }
        GameObject door = GameObject.Find("Door");
        door.transform.Translate(transform.right);
        yield return new WaitForSeconds(2.0f);
        GameManager.Instance.PausePlayer(false);
    }
}
