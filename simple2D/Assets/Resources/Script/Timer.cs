using UnityEngine;
using System.Threading;

public class Timer// : MonoBehaviour
{
    private Thread thread;

    private void Start()
    {
        thread = new Thread(Call);
        thread.IsBackground = true;
        thread.Start();
    }

    private void Update()
    {
        Debug.Log("Update");
    }

    private void Call()
    {
        int count = 0;
        while (count <= 5)
        {
            count++;
            Debug.Log(count);
            Thread.Sleep(1000);
        }
    }
    public void SimpleTimer(bool temp)
    {

    }
    //OnDisable()和OnApplicationQuit()看狀況自行選擇一個使用即可
    //由於這是示範，所以讓兩個函數都做同樣的事情

    private void OnDisable()
    {
        //Unity在離開當前場景後會自動呼叫這個函數
        thread.Abort();//強制中斷當前執行緒
    }

    private void OnApplicationQuit()
    {
        //當應用程式結束時會自動呼叫這個函數
        thread.Abort();//強制中斷當前執行緒
    }
}
