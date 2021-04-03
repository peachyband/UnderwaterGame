using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weapon : MonoBehaviour
{
    public bool isAttacking = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1")) 
        {
            isAttacking = true;
        }
    }

    private void OnCollisionEnter(Collision coll)
    {
        if ((coll.gameObject.tag == "Enemy") && (isAttacking))
        {
            //Destroy(coll.gameObject);
            Debug.Log("ATTACK");
        }
    }

    private void OnTriggerEnter(Collider coll)
    {
        if ((coll.gameObject.tag == "Enemy") && (isAttacking))
        {
            //Destroy(coll.gameObject);
            Debug.Log("ATTACK");
        }
    }

}
