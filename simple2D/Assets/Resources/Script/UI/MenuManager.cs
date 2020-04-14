/* this script is a new UI manager and it is more readable and adjustable
 * this script is a Singleton Class, it don't load in any GameObject;
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // required when using UI elements in scripts
using System.Linq;

public class MenuManager : Singleton<MenuManager>
{
    private string PathRoot = "Prefab/UI/";
    public GameObject m_CanvasRoot;
    public static List<KeyValuePair<string, GameObject>> m_UI_List = new List<KeyValuePair<string, GameObject>>();

    private bool CheckCanvacRootIsNull()
    {
        if(m_CanvasRoot == null)
        {
            Debug.Log(m_CanvasRoot + " is null");
            return true;
        }
        return false;
    }
    private int IsPanelLive(string name)
    {
        int index = 0;
        foreach(KeyValuePair<string, GameObject> i in m_UI_List)
        {
            if(i.Key == name)
            {
                return index;
            }
            index++;
        }
        return -1;
    }
    private GameObject InstantiatePanel(GameObject parent,GameObject bornpanel)
    {
        Vector2 position = new Vector2(Screen.width / 2, Screen.height / 2);
        GameObject temp = Instantiate(bornpanel, position, Quaternion.identity);
        temp.transform.SetParent(parent.transform);
        return temp;
    }
    public GameObject ShowPanel(string name)
    {
        if (CheckCanvacRootIsNull())
        {
            return null;
        }
        if (IsPanelLive(name)>=0)
        {
            Debug.Log(name + " this panel is live");
            return null;
        }
        GameObject loadpanel = Resources.Load(PathRoot + name) as GameObject;
        //maybe fail;
        if(loadpanel == null)
        {
            Debug.Log("loadpanel "+ name + " fail, please see the MenuManager~");
            return null;
        }
        GameObject panel = InstantiatePanel(m_CanvasRoot,loadpanel);
        panel.name = name;

        KeyValuePair<string, GameObject> temp = new KeyValuePair<string, GameObject>(name, panel);
        m_UI_List.Add(temp);

        return panel;
    }
    public void TogglePanel(string name)
    {//  this part is used to be more effcient
        int thePenalIndex = IsPanelLive(name) ;
        if (thePenalIndex >= 0)
        {
            if(m_UI_List[thePenalIndex].Value != null)
            {
                m_UI_List[thePenalIndex].Value.SetActive(true);
            }
        }
    }
    public void CloseLastPanel()
    {
        //int thePenalIndex = IsPanelLive(name);
        if (m_UI_List.Count == 0)
        {
            Debug.Log("we don't have Ui to close!");
        }
        else
        {
            Object.Destroy(m_UI_List.Last().Value);
            m_UI_List.Remove(m_UI_List.Last());
            if (m_UI_List.Count == 0) GameManager.Instance.Resume();
        }
    }
    public void CloseSpecPanel(string name)
    {
        if (m_UI_List.Count == 0)
        {
            Debug.Log("we don't have Ui to close!");
        }
        else
        {
            int thePenalIndex = IsPanelLive(name);
            if (thePenalIndex >= 0 && m_UI_List[thePenalIndex].Value != null)
            {
                Destroy(m_UI_List[thePenalIndex].Value);
                m_UI_List.Remove(m_UI_List[thePenalIndex]);
                if (m_UI_List.Count == 0) GameManager.Instance.Resume();
            }
        }
    }
    
    public Vector2 GetCanvasSize()
    {
        if (CheckCanvacRootIsNull())
        {
            return Vector2.one * -1;
        }
        RectTransform trans = m_CanvasRoot.transform as RectTransform;

        return trans.sizeDelta;
    }
   
}
