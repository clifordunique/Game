using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageSelect : MonoBehaviour
{
    [SerializeField]
    GameObject[] pages = null;
    public void ShowPage(int index)
    {
        foreach(GameObject i in pages)
        {
            i.SetActive(false);
        }
        pages[index].SetActive(true);
    }
    private void Start()
    {
        ShowPage(0);
    }
}
