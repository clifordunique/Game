using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjPoolTest : MonoBehaviour
{
    private ObjPool pool;
    // Start is called before the first frame update
    void Start()
    {
        pool = new ObjPool(transform.GetChild(1).gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            GameObject yeee = pool.Get(Player.Instance.transform.position, Quaternion.identity);
            yeee.transform.parent = transform;
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            pool.Recycle(transform.GetChild(1).gameObject);
            transform.GetChild(1).parent = transform.GetChild(0);
        }
    }
}
