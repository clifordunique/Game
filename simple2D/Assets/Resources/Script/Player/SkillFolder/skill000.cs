using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skill000 : MonoBehaviour
{
    private string PathRoot = "Prefab/PlayerSkill/";
    [SerializeField]
    static GameObject skillA_prefab;
    string prefabName = "skill003";
    // Must be change the Path

    private void Awake()
    {
        ChangeSkill(prefabName);
        SubscribeSkill(prefabName);
        SubUnpack();
    }
    protected void SubscribeSkill(string prefabName)
    {   //  notice to subscribe witch event
        PlayerSkill.On_SkillA_Click += skill;
        Debug.Log("skill 001 is listen to A");
    }
    protected void SubUnpack()
    {
        PlayerSkill.UnPack_SkillA_Click += DestroyTheClass;
    }
    protected void DestroyTheClass()
    {
        Destroy(this);
    }
    public void ChangeSkill(string name)
    {
        skillA_prefab = Resources.Load(PathRoot + name) as GameObject;
        if (skillA_prefab == null)
        {
            Debug.Log("Chanege skill A fail: " + name);
        }
    }

    //  the main attack way
    public IEnumerator skill()
    {
        Player.Instance.state.canControll = false;
        Vector3 position = Player.Instance.transform.position;
        Instantiate(skillA_prefab, position, Quaternion.identity);
        yield return new WaitForSeconds(0.3f);  //  postpone for skill
        Player.Instance.state.canControll = true;
    }
    //  end

}
