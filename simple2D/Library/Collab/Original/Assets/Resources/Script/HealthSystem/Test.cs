using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform pfHealthBar;
    public int HpMax;
    public HealthSystem healthSystem;

    private void Start()
    {
        //int hp = GetComponent<Monster>().hp;
        ////HealthSystem healthSystem = 
        //healthSystem = new HealthSystem(hp);
        //Transform healthBarTransform = Instantiate(pfHealthBar, new Vector3(0, 10), Quaternion.identity);
        //HealthBar healthBar = healthBarTransform.GetComponent<HealthBar>();
        //healthBar.Setup(healthSystem);

        //Debug.Log(healthSystem.GetHealthPercent());
        //healthSystem.Damage(10);
        //Debug.Log(healthSystem.GetHealthPercent());



    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKey(KeyCode.UpArrow))
        //{
        //    healthSystem.Damage(1);
        //    Debug.Log("- :" + healthSystem.GetHealth());
        //}
        //if (Input.GetKey(KeyCode.DownArrow))
        //{
        //    healthSystem.Heal(1);
        //    Debug.Log("+ :" + healthSystem.GetHealth());
        //}
    }
}
