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
    private float distance1, stayTime;
    private float lifeTime, returnTime;
    bool flag = true;
    private Vector3 velocity;

    new private void Start()
    {
        base.Start();
        startTime = Time.time;
        player = Player.Instance;
        directionX = Mathf.Sign(player.forward);
        lifeTime = (distance / speed) + stayTime;   //  cal the time the skill will exist
        returnTime = distance1 / speed;
        velocity = new Vector3(speed * directionX, 0, 0);
    }
    private void Update()
    {
        UpdatRaycastOrigins();
        UpdateVelocity();
        transform.Translate(velocity);
        DetectCollision();
        if (startTime + lifeTime <= Time.time)
        {
            Destroy(gameObject, 0.1f);
        }
    }
    IEnumerator StayZero(float stayTime)
    {
        yield return new WaitForSeconds(stayTime);
        velocity = new Vector3(speed * directionX, 0, 0);

    }
    void UpdateVelocity()
    {
        if (flag && startTime + returnTime <= Time.time)
        {
            directionX *= -1;
            flag = false;
            velocity = Vector3.zero;
            StartCoroutine(StayZero(stayTime));
            HurtObj.Clear();
        }
    }
    private void SkillType()
    {

    }

}
