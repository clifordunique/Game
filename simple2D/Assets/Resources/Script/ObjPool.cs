using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjPool: Queue<GameObject>
{
    public string name;
    public GameObject PoolObj;

    //constructor
    public ObjPool(GameObject gameObj)
    {
        name = gameObj.name;
        PoolObj = GameObject.Instantiate(gameObj);
        PoolObj.name = name;
        Enqueue(PoolObj);
    }
    public ObjPool(string PathRoot, string Objname)
    {
        name = Objname;
        GameObject prefab = Resources.Load(PathRoot + Objname) as GameObject;
        PoolObj = GameObject.Instantiate(prefab);
        PoolObj.name = Objname;
        Enqueue(PoolObj);
    }


    public GameObject Get(Vector2 position, Quaternion quaternion)
    {
        GameObject result = null;
        if (Count > 0)
        {
            result = Dequeue();
            result.transform.position = position;
            result.transform.rotation = quaternion;
            result.SetActive(true);
            return result;
        }
        else
        {
            result = GameObject.Instantiate(PoolObj, position, quaternion);
            result.name = name;
            result.SetActive(true);
            return result;
        }
    }
    public void Recycle(GameObject obj)
    {
        obj.SetActive(false);
        Enqueue(obj);
    }
}
