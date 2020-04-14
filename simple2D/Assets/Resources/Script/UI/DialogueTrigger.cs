using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    public string NPCName;

    [TextArea(3,10)]
    public string[] sentences;
}

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    [SerializeField]
    private float triggerDistance = 1f;

    private DialoguePrinter dialoguePrinter;
    private Player player;

    private void Start()
    {
        dialoguePrinter = FindObjectOfType<DialoguePrinter>();
        player = Player.Instance;
    }

    private void Update()
    {
        if (GetPlayerDistance() <= triggerDistance && player.controller.collisionInfo.below)
        {
            if (dialoguePrinter.canTalk == this) return;
            if (dialoguePrinter.canTalk == null) dialoguePrinter.canTalk = this;
            else if(dialoguePrinter.canTalk.GetPlayerDistance() > GetPlayerDistance()) dialoguePrinter.canTalk = this;
        }
        else if(dialoguePrinter.canTalk == this)
        {
            dialoguePrinter.canTalk = null;
        }
    }

    public void TriggerDialogue(int a)
    {
        dialoguePrinter.StartCoroutine(dialoguePrinter.StartDialogue(dialogue));
    }

    public void TriggerDialogue()
    {
        dialoguePrinter.StartCoroutine(dialoguePrinter.StartDialogue(dialogue));
    }

    private float GetPlayerDistance()
    {
        return (transform.position - player.transform.position).sqrMagnitude;
    }
}
