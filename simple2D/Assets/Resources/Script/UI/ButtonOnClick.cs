/* this script is to controll Witch button should be open when the button click
 * this script load on the ButtonList object
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonOnClick : MonoBehaviour
{
    public void ClickedNewPanel(string name)
    {
        //Output this to console when the Button is clicked
        Debug.Log("Born New Panel : " + name);
        MenuManager.Instance.ShowPanel(name);
    }
    public void SwitchNewPanel(string name)
    {
        //Output this to console when the Button is clicked
        Debug.Log("Born New Panel : " + name);
        //MenuManager.Instance.SwitchPanel(name);
    }
    public void CloseNewPanel(string name)
    {
        MenuManager.Instance.CloseSpecPanel(name);
    }
    public void ChangeScene(string name)
    {
        SceneManager.LoadScene(name);
    }
    public void Quit()
    {
        Application.Quit();
    }
}
