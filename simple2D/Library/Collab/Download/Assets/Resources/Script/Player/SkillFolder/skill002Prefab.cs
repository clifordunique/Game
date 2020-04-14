/* question:1.try to turn to the delagate type and we and change the skill
 *          2.some skill will be push by the platform  solution: add new Layer?
 *          3.    
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skill002Prefab : skill000Prefab
{
    [SerializeField]
    private float distance1;
    private float lifeTime, returnTime;
    bool flag = true;

    new private void Start()
    {
        base.Start();
        startTime = Time.time;
        player = Player.Instance;
        directionX = Mathf.Sign(player.forward);
        lifeTime = distance / speed;   //  cal the time the skill will exist
        returnTime = distance1 / speed;
    }
    private void Update()
    {
        UpdatRaycastOrigins();
        transform.Translate(new Vector3(speed * directionX, 0, 0));
        DetectCollisionForward();
        if (flag && startTime + returnTime <= Time.time)
        {
            directionX *= -1;
            flag = false;
            HurtObj.Clear();
        }
        if (startTime + lifeTime <= Time.time)
        {
            Destroy(gameObject, 0.1f);
        }
    }
    private void SkillType()
    {

    }

}
