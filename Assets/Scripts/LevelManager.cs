using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private int maxScore;
    private int currentScore;

    private bool isLevelOver;

    public GameObject[] people;

    [Header("Time")]
    [SerializeField]
    private float time;
    [SerializeField]
    private TextMeshProUGUI timeText;
    private bool outOfTime;

    [Header("UI Elements")]
    // Score Screen
    [SerializeField]
    private TextMeshProUGUI overtimeText;
    [SerializeField]
    private TextMeshProUGUI smallItemsDamagedText;
    [SerializeField]
    private TextMeshProUGUI mediumItemsDamagedText;
    [SerializeField]
    private TextMeshProUGUI largeItemsDamagedText;
    [SerializeField]
    private TextMeshProUGUI totalText;
    [SerializeField]
    private GameObject star1;
    [SerializeField]
    private GameObject star2;
    [SerializeField]
    private GameObject star3;
    [SerializeField]
    private GameObject star4;
    [SerializeField]
    private GameObject star5;

    // Pause Screen
    [SerializeField]
    private GameObject pauseScreen;

    private int lowCostBreakCount;
    private int midCostBreakCount;
    private int highCostBreakCount;

    private GameManager gm;

    private void Start() {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();

        people = GameObject.FindGameObjectsWithTag("Gallery");

        CalculateMaxScore();

        lowCostBreakCount = 0;
        midCostBreakCount = 0;
        highCostBreakCount = 0;

        isLevelOver = false;
        outOfTime = false;
        StartCoroutine(CountDown());

        gm.TriggerOpen();
    }

    public void InputPause(InputAction.CallbackContext value) {
        if(value.performed) {
            if(!GetIsLevelOver()) {
                if(gm.GetPaused()) {
                    gm.PausePlayer(false);
                    pauseScreen.SetActive(false);
                } else {
                    gm.PausePlayer(true);
                    pauseScreen.SetActive(true);
                }
            }
        }
    }

    private int GetCurrentScore() {
        return currentScore;
    }

    public void DecreaseScore(Breakable.ObjectSize objectSize) {
        switch (objectSize) {
            case Breakable.ObjectSize.SMALL:
                currentScore -= 10;
                lowCostBreakCount += 1;
                break;
            case Breakable.ObjectSize.MEDIUM:
                currentScore -= 20;
                midCostBreakCount += 1;
                break;
            case Breakable.ObjectSize.BIG:
                currentScore -= 30;
                highCostBreakCount += 1;
                break;
        }
    }

    public void CalculcateGrade() {
        if(maxScore != 0) {
            // If out of time, subtract half of score
            if(outOfTime) {
                float decreaseAmount = maxScore / 2;
                currentScore -= (maxScore / 2);
                overtimeText.text = "-" + decreaseAmount.ToString();
            } else {
                overtimeText.text = "On Time!";
            }

            smallItemsDamagedText.text = lowCostBreakCount.ToString();
            mediumItemsDamagedText.text = midCostBreakCount.ToString();
            largeItemsDamagedText.text = highCostBreakCount.ToString();
            totalText.text = currentScore.ToString();

            float percentage = (float)currentScore / (float)maxScore;

            if(percentage >= 0.2f) {
                star1.SetActive(true);
            }
            if(percentage >= 0.4f) {
                star2.SetActive(true);
            }
            if(percentage >= 0.6f) {
                star3.SetActive(true);
            }
            if(percentage >= 0.8f) {
                star4.SetActive(true);
            }
            if(percentage == 1.0f) {
                star5.SetActive(true);
            }

        } else {
            Debug.Log("LevelManager: maxScore is set to 0");
        }
    }

    public void UpdateTime(float time)
    {
        timeText.text = Mathf.Round(time).ToString();
        if(time <= 0 && !outOfTime)
        {
            outOfTime = true;
            StopCoroutine(CountDown());
            time = 0;
            timeText.text = Mathf.Round(time).ToString();
        }
    }

    private IEnumerator CountDown()
    {
        // Check if paused
        while(gm.GetPaused()) 
            yield return new WaitForEndOfFrame();

        UpdateTime(Mathf.Max(time, 0));
        yield return new WaitForSeconds(1f);

        // Check if paused
        while(gm.GetPaused()) 
            yield return new WaitForEndOfFrame();

        time -= 1f;
        StartCoroutine(CountDown());

    }

    private void CalculateMaxScore() {
        maxScore = 0;
        GameObject[] allObjects = GameObject.FindGameObjectsWithTag("Breakable");
        foreach(GameObject breakableObject in allObjects) {
            Breakable.ObjectSize objectSize = breakableObject.GetComponent<Breakable>().objectSize;
            switch (objectSize) {
                case Breakable.ObjectSize.SMALL:
                    maxScore += 20;
                    break;
                case Breakable.ObjectSize.MEDIUM:
                    maxScore += 40;
                    break;
                case Breakable.ObjectSize.BIG:
                    maxScore += 60;
                    break;
            }
        }

        currentScore = maxScore;
    }

    public void MovePeople(Vector3 toPoint) {
        foreach(GameObject person in people) {
            StartCoroutine(MovePersonToPoint(toPoint, person));
        }
    }

    private IEnumerator MovePersonToPoint(Vector3 toPoint, GameObject person) {
        while(Vector3.Distance(person.transform.position, toPoint) > 0.1f) {
            person.transform.position = Vector3.Lerp(person.transform.position, toPoint, 0.7f);
            yield return new WaitForEndOfFrame();
        }
    }

    public void AlertTheGallery() {
        foreach(GameObject person in people) {
            /* person.GetComponent<Person>().StartCoroutine("AlertDialogue"); */
            person.GetComponent<Person>().Alert();
        }
    }

    public void SetIsLevelOver(bool levelOver) {
        isLevelOver = levelOver;
    }

    public bool GetIsLevelOver() {
        return isLevelOver;
    }
}
