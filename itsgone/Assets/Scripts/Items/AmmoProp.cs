using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoProp : MonoBehaviour
{
    public float damage;
 
    IEnumerator deleteOnTime()
    {
        yield return new WaitForSeconds(4f);
        Destroy(gameObject);
    }
   
    private void Start()
    {
        StartCoroutine(deleteOnTime());
    }
    //private void Update()
    //{
    //    if (!grounded)
    //    {
    //        float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
    //        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    //    }
    //}

    //void OnTriggerEnter2D(Collider2D other)
    //{
    //    grounded = true;
    //}
}
