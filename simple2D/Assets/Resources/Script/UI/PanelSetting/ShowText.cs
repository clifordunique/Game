using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowText : MonoBehaviour
{
    Transform m_transform;
    Event keyEvent;
    bool isWaitinForKey;
    KeyCode newKeyCode;
    Text buttonText;

    int smallFontSize = 50;
    int normalSize = 70;
    private void Awake()
    {
        m_transform = gameObject.transform;
    }
    void Start()
    {
        for (int i = 0; i < m_transform.childCount; i++)
        {
            Transform button_obj = m_transform.GetChild(i);
            //  this will access the button object
            switch (button_obj.name)
            {
                case "Jump":
                    button_obj.GetComponentsInChildren<Text>()[0].text = KeyBoardInput.Instance.jump.ToString();
                    break;
                case "Attack":
                    button_obj.GetComponentsInChildren<Text>()[0].text = KeyBoardInput.Instance.attack.ToString();
                    break;
                case "Change":
                    button_obj.GetComponentsInChildren<Text>()[0].text = KeyBoardInput.Instance.change.ToString();
                    break;
                case "Up":
                    button_obj.GetComponentsInChildren<Text>()[0].text = KeyBoardInput.Instance.up.ToString();
                    break;
                case "Down":
                    button_obj.GetComponentsInChildren<Text>()[0].text = KeyBoardInput.Instance.down.ToString();
                    break;
                case "Right":
                    button_obj.GetComponentsInChildren<Text>()[0].text = KeyBoardInput.Instance.right.ToString();
                    break;
                case "Left":
                    button_obj.GetComponentsInChildren<Text>()[0].text = KeyBoardInput.Instance.left.ToString();
                    break;
                case "SkillA":
                    button_obj.GetComponentsInChildren<Text>()[0].text = KeyBoardInput.Instance.skillA.ToString();
                    break;
                case "SkillB":
                    button_obj.GetComponentsInChildren<Text>()[0].text = KeyBoardInput.Instance.skillB.ToString();
                    break;
            }
            if ("UpArrow" == button_obj.GetComponentsInChildren<Text>()[0].text)
            {
                button_obj.GetComponentsInChildren<Text>()[0].fontSize = smallFontSize;
            }
            else if ("DownArrow" == button_obj.GetComponentsInChildren<Text>()[0].text)
            {
                button_obj.GetComponentsInChildren<Text>()[0].fontSize = smallFontSize;
            }
            else if ("RightArrow" == button_obj.GetComponentsInChildren<Text>()[0].text)
            {
                button_obj.GetComponentsInChildren<Text>()[0].fontSize = smallFontSize;
            }
            else if ("LeftArrow" == button_obj.GetComponentsInChildren<Text>()[0].text)
            {
                button_obj.GetComponentsInChildren<Text>()[0].fontSize = smallFontSize;
            } else button_obj.GetComponentsInChildren<Text>()[0].fontSize = normalSize;
        }
    }
    private void OnGUI()
    {
        keyEvent = Event.current;
        if (keyEvent.isKey && isWaitinForKey)
        {
            newKeyCode = keyEvent.keyCode;
            isWaitinForKey = false;
        }
    }
    public void StartAssignment(string keyName)
    {   // if thier is no object waiting for key, the obj will wait for it
        if (!isWaitinForKey)
        {
            StartCoroutine(AssignKey(keyName));
        }
    }
    public void SendText(Text text)
    {
        buttonText = text;
    }
    IEnumerator WaitForKey()
    {
        while (!keyEvent.isKey) yield return null;
    }
    public IEnumerator AssignKey(string keyName)
    {
        isWaitinForKey = true;
        yield return WaitForKey();
        switch (keyName)
        {
            case "up":
                KeyBoardInput.Instance.up = newKeyCode;
                buttonText.text = newKeyCode.ToString();
                PlayerPrefs.SetString("upKey", buttonText.text);
                break;
            case "down":
                KeyBoardInput.Instance.down = newKeyCode;
                buttonText.text = newKeyCode.ToString();
                PlayerPrefs.SetString("downKey", buttonText.text);
                break;
            case "right":
                KeyBoardInput.Instance.right = newKeyCode;
                buttonText.text = newKeyCode.ToString();
                PlayerPrefs.SetString("rightKey", buttonText.text);
                break;
            case "left":
                KeyBoardInput.Instance.left = newKeyCode;
                buttonText.text = newKeyCode.ToString();
                PlayerPrefs.SetString("leftKey", buttonText.text);
                break;
            case "jump":
                KeyBoardInput.Instance.jump = newKeyCode;
                buttonText.text = newKeyCode.ToString();
                PlayerPrefs.SetString("jumpKey", buttonText.text);
                break;
            case "attack":
                KeyBoardInput.Instance.attack = newKeyCode;
                buttonText.text = newKeyCode.ToString();
                PlayerPrefs.SetString("attackKey", buttonText.text);
                break;
            case "change":
                KeyBoardInput.Instance.change = newKeyCode;
                buttonText.text = newKeyCode.ToString();
                PlayerPrefs.SetString("changeKey", buttonText.text);
                break;
            case "skillA":
                KeyBoardInput.Instance.skillA = newKeyCode;
                buttonText.text = newKeyCode.ToString();
                PlayerPrefs.SetString("skillAKey", buttonText.text);
                break;
            case "skillB":
                KeyBoardInput.Instance.skillB = newKeyCode;
                buttonText.text = newKeyCode.ToString();
                PlayerPrefs.SetString("skillBKey", buttonText.text);
                break;
            case "skillC":
                KeyBoardInput.Instance.skillC = newKeyCode;
                buttonText.text = newKeyCode.ToString();
                PlayerPrefs.SetString("skillCKey", buttonText.text);
                break;
        }

        if ("UpArrow" == buttonText.text)
        {
            buttonText.fontSize = smallFontSize;
        }
        else if ("DownArrow" == buttonText.text)
        {
            buttonText.fontSize = smallFontSize;
        }
        else if ("RightArrow" == buttonText.text)
        {
            buttonText.fontSize = smallFontSize;
        }
        else if ("LeftArrow" == buttonText.text)
        {
            buttonText.fontSize = smallFontSize;
        }
        else buttonText.fontSize = normalSize;
    }
}
