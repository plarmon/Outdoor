using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public void TransistionScene() {
        // SceneManager.LoadScene("SampleScene");
        StartCoroutine(CloseDoor());
    }

    private IEnumerator CloseDoor() {
        yield return new WaitForSeconds(2.0f);
        SceneManager.LoadScene("SampleScene");
    }
}
