using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SystemManager : MonoBehaviour
{
    public int coin, maxCoin, point;
    public GameObject prefab ,endPic,player;
    Text textpoint , textTime;

    int leftTime;
    float EndTime;

    private void Start()
    {
        coin = 0;
        maxCoin = 40;
        point = 0;
        leftTime = 40;
        textpoint = GameObject.Find("PointNum").GetComponent<Text>();
        textTime = GameObject.Find("LeftTime").GetComponent<Text>();
        EndTime = Time.time + 51;
    }

    private void Update()
    {
        if(coin < maxCoin)
        {
            GenerateCoin();
        }
        leftTime = (int)(EndTime- Time.time);
        if(leftTime <=0)
        {
            endPic.SetActive(true);
            player.SetActive(false);
            string temp = "0";
            textTime.text = temp;
        }
        else
        {
            string str = leftTime.ToString();
            textTime.text = str;
        }

    }

    private void GenerateCoin()
    {
        Vector3 position = GenerateCoinPosition();
        Quaternion roro = new Quaternion(1.5f, 0, 0,1);
        Instantiate(prefab, position , roro);
        coin++;
        FlashPoint();
    }
    private Vector3 GenerateCoinPosition()
    {
        return new Vector3(Random.Range(165, 410), 1.5f, Random.Range(-4, 204));
    }
    private void FlashPoint() {
        string str = point.ToString();
        textpoint.text = str;
    }
    public void EndGame()
    {
        Application.Quit();
    }
    public void ReStart()
    {
        SceneManager.LoadScene(0);
    }
}
