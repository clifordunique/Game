using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : MonoBehaviour
{
    //variable
    public bool isOpen = false;

    private void Awake()
    {
        int x = 0;
        // change string to int
        int.TryParse(this.name.Replace("treasure_", ""), out x);
        //Debug.Log(x);

        LoadScene();
        //if(isOpen == true)
        //{
        //    this.gameObject.name = "qq";
        //}
        Debug.Log(isOpen);
    }

    // player do something
    public void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isOpen = true;
            SaveScene();
        }
    }

    public void SaveScene()
    {
        SaveFile.SaveSceneFile(this);
    }

    private void LoadScene()
    {
        ItemData data = SaveFile.LoadSceneFile();


        isOpen = data.isOpen;
    }
}
