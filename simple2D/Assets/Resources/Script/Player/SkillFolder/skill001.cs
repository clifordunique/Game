using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skill001 : MonoBehaviour
{
    private string PathRoot = "Prefab/PlayerSkill/";
    [SerializeField]
    static GameObject skillA_prefab;
    // Must be change the Path
    string prefabName = "skill001";
    float last_spell = -2f;
    float spell_cd = 0.6f;
    PlayerAnim playerAnim;

    private void Awake()
    {
        ChangeSkill(prefabName);
        SubscribeSkill();
        SubUnpack();
        playerAnim = Player.Instance.gameObject.GetComponent<PlayerAnim>();
    }
    protected void SubscribeSkill()
    {
        PlayerSkill.On_SkillA_Click += skill;
        Debug.Log(prefabName + " is listen to A");
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
        if (Time.time < last_spell + spell_cd) yield break;
        playerAnim.SetAnim("throw", 2);
        last_spell = Time.time;
        Player.Instance.state.canControll = false;
        Vector3 position = Player.Instance.transform.position;
        Instantiate(skillA_prefab, position, Quaternion.identity);
        yield return new WaitForSeconds(0.3f);  //  postpone for skill
        Player.Instance.state.canControll = true;
    }
    //  end

}
