using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skill003Prefab : skill000Prefab
{
    [SerializeField]
    private float lifeTime;
    [SerializeField]
    private Vector3 velocity;

    new private void Start()
    {
        base.Start();
        startTime = Time.time;
        player = Player.Instance;
        directionX = Mathf.Sign(player.forward);
        ControllVelocity();
    }
    private void Update()
    {
        UpdatRaycastOrigins();
        DetectCollision();
        if (startTime + lifeTime <= Time.time)
        {
            Destroy(gameObject, 0.1f);
        }
    }
    private void LateUpdate()
    {
        transform.Translate(velocity * Time.deltaTime, Space.Self);
    }
    private void ControllVelocity()
    {
        velocity = new Vector3(speed * directionX, 0, 0);
    }
    private void SkillType()
    {

    }
}
