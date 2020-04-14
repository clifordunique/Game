using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    public bool isPause;

    public void Spawn(string scene1,string scene2)
    {

    }

    public void Pause()
    {
        isPause = true;
        //Time.timeScale = 0;
    }

    public void Resume()
    {
        isPause = false;
        //Time.timeScale = 1;
    }

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        TmpSaver.Instance.Init();
    }

    public void StartTransition(bool isOpening, bool waiting)
    {
        StartCoroutine(Transition(isOpening, waiting));
    }
    IEnumerator Transition(bool isOpening, bool wait)
    {
        RawImage rawImage = GameObject.Find("RawImage").GetComponent<RawImage>();
        if (isOpening) Pause();
        Color fading = new Color(0, 0, 0, 0.1f);
        int opt;
        if (isOpening) opt = 1;
        else  opt = -1;
        for(int i = 0; i < 10; i++)
        {
            rawImage.color = rawImage.color + opt * fading;
            yield return null;
        }
        if (!isOpening) Resume();
        if (wait) StartCoroutine(Transition(!isOpening, false));
    }
    
}
