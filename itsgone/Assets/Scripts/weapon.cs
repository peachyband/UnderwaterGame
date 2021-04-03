using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public bool isAttacking = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isAttacking = true;
        }
    }

    private void OnTriggerStay(Collider coll)
    {
        if ((coll.gameObject.tag == "Enemy") && isAttacking)
        {
            //Destroy(coll.gameObject);
            Debug.Log("ATTACK");
            isAttacking = false;
        }
    }

}
