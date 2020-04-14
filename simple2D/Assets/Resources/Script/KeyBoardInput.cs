using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBoardInput : Singleton<KeyBoardInput>
{
    public KeyCode up { get; set; }
    public KeyCode down { get; set; }
    public KeyCode right { get; set; }
    public KeyCode left { get; set; }
    public KeyCode jump { get; set; }
    public KeyCode attack { get; set; }
    public KeyCode change { get; set; }
    public KeyCode skillA { get; set; }
    public KeyCode skillB { get; set; }
    public KeyCode skillC { get; set; }
    // Start is called before the first frame update
    void Awake()
    {
        up = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("upKey", "UpArrow"));
        down = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("downKey", "DownArrow"));
        right = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("rightKey", "RightArrow"));
        left = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("leftKey", "LeftArrow"));
        jump = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("jumpKey", "Space"));
        attack = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("attackKey", "Z"));
        change = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("changeKey", "C"));
        skillA = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("skillAKey", "X"));
        skillB = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("skillBKey", "S"));
        skillC = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("skillCKey", "D"));
    }
}
