using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Person : MonoBehaviour
{
    [System.Serializable]
    public class Dialogue {
        public string personName = "";
        public string[] speech = new string[0];
    }

    // Speech
    private float textDisplayTime = 3.0f;

    // Text Assets
    [SerializeField]
    private TextAsset casualDialogueText;
    public Dialogue casualDialogue;
    private int casualDialogueIndex;

    [SerializeField]
    private TextAsset alertedDialogueText;
    public Dialogue alertedDialogue;
    private int alertedDialogueIndex;

    [SerializeField]
    private GameObject textBox;
    [SerializeField]
    private TextMeshProUGUI speechText;
    [SerializeField]
    private TextMeshProUGUI nameText;

    private Queue<string> inputStream = new Queue<string>();

    private void Awake() {
        casualDialogue = ProceessText(casualDialogueText);
        alertedDialogue = ProceessText(alertedDialogueText);
    }

    private void Start() {
        textBox.SetActive(false);
        casualDialogueIndex = 0;
        alertedDialogueIndex = 0;
        StartCoroutine(CasualDialogue());
    }

    private Dialogue ProceessText(TextAsset textDoc) {
        Dialogue dialogue = new Dialogue();

        string txt = textDoc.text;
        string[] lines = txt.Split(System.Environment.NewLine.ToCharArray()); // Split dialogue lines by newline

        foreach (string line in lines) { // for every line of dialogue
            if(!string.IsNullOrEmpty(line)) { // ignore empty lines of dialogue
                if(line.StartsWith('[')) { // e.g. [Name=Michael] hello, my name is Michael
                    string name = line.Substring(line.IndexOf('=') + 1, line.IndexOf(']') - (line.IndexOf('=') + 1)); // special = [NAME=Michael]
                    dialogue.personName = name;
                } else if(!line.StartsWith('$')) {
                    // Resize the array
                    int speechLength = dialogue.speech.Length;
                    string[] dialogueCopy = dialogue.speech;
                    dialogue.speech = new string[speechLength + 1];
                    for(int i = 0; i <= speechLength - 1; i++) {
                        dialogue.speech[i] = dialogueCopy[i];
                    }
                    // Add the line to the array
                    dialogue.speech[speechLength] = line;
                }
            }
        }

        return dialogue;
    }

    public void Alert(){
        StopAllCoroutines();
        EndDialogue();
        StartCoroutine(AlertDialogue());
    }

    public IEnumerator AlertDialogue() {
        nameText.text = alertedDialogue.personName;
        speechText.text = alertedDialogue.speech[alertedDialogueIndex];
        alertedDialogueIndex += 1;
        if(alertedDialogueIndex >= alertedDialogue.speech.Length) {
            alertedDialogueIndex = 0;
        }
        textBox.SetActive(true);
        yield return new WaitForSecondsRealtime(textDisplayTime);
        while(GameManager.Instance.GetPaused()) {
            yield return new WaitForEndOfFrame();
        }
        EndDialogue();
        StartCoroutine(CasualDialogue());
    }

    private IEnumerator CasualDialogue() {
        while(true) {
            // Calculate random wait time
            float waitTime = Random.Range(1, 6);
            yield return new WaitForSecondsRealtime(waitTime);
            while(GameManager.Instance.GetPaused()) {
                yield return new WaitForEndOfFrame();
            }
            nameText.text = casualDialogue.personName;
            speechText.text = casualDialogue.speech[casualDialogueIndex];
            casualDialogueIndex += 1;
            if(casualDialogueIndex >= casualDialogue.speech.Length) {
                casualDialogueIndex = 0;
            }
            textBox.SetActive(true);
            yield return new WaitForSecondsRealtime(textDisplayTime);
            while(GameManager.Instance.GetPaused()) {
                yield return new WaitForEndOfFrame();
            }
            EndDialogue();
        }
    }

    public void EndDialogue() {
        speechText.text = "";
        nameText.text = "";
        textBox.SetActive(false);
    }
}
