/* question:1.try to turn to the delagate type and we and change the skill
 *          2.some skill will be push by the platform  solution: add new Layer?
 *          3.    
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skill001Prefab : skill000Prefab
{
    private float lifeTime ;

    new private void Start()
    {
        base.Start();
        startTime = Time.time;
        player = Player.Instance;
        directionX = Mathf.Sign(player.forward);
        lifeTime  = distance / speed;   //  cal the time the skill will exist
    }
    private void Update()
    {
        UpdatRaycastOrigins();
        transform.Translate(new Vector3(speed * directionX, 0, 0));
        DetectCollision();
        if (startTime + lifeTime <= Time.time)
        {
            Destroy(gameObject, 0.1f);
        }
    }
    private void SkillType()
    {

    }
}
