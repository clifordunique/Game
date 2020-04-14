using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class DialoguePrinter : MonoBehaviour
{
    public bool talking, typing;
    [SerializeField]
    public DialogueTrigger canTalk;

    private Queue<string> sentences;
    private TextMeshProUGUI nameText, dialogueText;
    private Image image;
    private Color fading;
    private bool processing;

    private void Awake()
    {
        sentences = new Queue<string>();
    }
    private void Start()
    {
        nameText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        dialogueText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        image = GetComponent<Image>();
        fading = new Color(0, 0, 0, 0.05f);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            if (talking) ShowNextSentence();
            else if (canTalk != null) canTalk.TriggerDialogue();
        }
        /*if (talking && Input.GetKeyDown(KeyBoardInput.Instance.attack))
        {
            ShowNextSentence();
        }*/
    }
    public IEnumerator StartDialogue(Dialogue dialogue)
    {
        if (processing) yield break;
        processing = true;
        GameManager.Instance.Pause();
        if (SceneManager.GetActiveScene().name == "BossT")
        {
            TmpSaver.Instance.WriteFile();
        }
        if (talking)
        {
            Debug.Log("Is still talking!");
            yield break;
        }
        Debug.Log("Start Dialogue");
        sentences.Clear();
        foreach(string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }
        
        for (int i = 0; i < 20; i++)
        {
            image.color += fading;
            yield return null;
        }
        if (sentences.Count > 0) talking = true;
        processing = false;
        nameText.text = dialogue.NPCName;
        ShowNextSentence();
    }
    public void ShowNextSentence()
    {
        if (!talking) return;
        if (typing)
        {
            typing = false;
            return;
        }
        if (sentences.Count == 0)
        {
            StartCoroutine(EndDialogue());
            return;
        }
        string sentence = sentences.Dequeue();
        StartCoroutine(TypingSentence(sentence));
    }
    IEnumerator TypingSentence(string sentence)
    {
        typing = true;
        dialogueText.text = "";
        foreach(char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
            yield return null;
            yield return null;
            if (!typing) break;
        }
        dialogueText.text = sentence;
        typing = false;
    }
    public IEnumerator EndDialogue()
    {
        if (processing) yield break;
        processing = true;
        dialogueText.text = nameText.text = "";
        //talking = false;
        for (int i = 0; i < 20; i++)
        {
            image.color -= fading;
            yield return null;
        }
        talking = false;
        processing = false;
        Debug.Log("End Dialogue");
        //yield return null;
        GameManager.Instance.Resume();
        
        yield break;
    }
}
