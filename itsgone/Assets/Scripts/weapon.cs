using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weapon : MonoBehaviour
{
    public int damage = 20;
    public bool isAttacking = false;
    public bool isAtt = false;
    public Transform hand;
    public Transform end;
    public float force = 5f;
    public int dir = -1;

    // Start is called before the first frame update
    void Start()
    {
        //hand = gameObject.GetComponentInParent<Transform>();
        transform.position = hand.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && !isAtt)
        {
            isAtt = true;
            isAttacking = true;
            //transform.Translate(transform.forward * force * dir);
            transform.position = end.position;
            StartCoroutine("coolDown");
        }
    }



    private void OnTriggerStay(Collider coll)
    {
        if ((coll.gameObject.tag == "Enemy") && isAttacking)
        {
            //Destroy(coll.gameObject);
            //coll.gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
            //Debug.Log("ATTACK");
            isAttacking = false;
            if (coll.gameObject.GetComponent<MeshRenderer>())
                StartCoroutine(glow(coll.gameObject.GetComponent<MeshRenderer>().material));
            else StartCoroutine(glow(coll.gameObject.GetComponent<SkinnedMeshRenderer>().material));
            coll.gameObject.GetComponent<Rigidbody>().AddForce(transform.up * force * Time.deltaTime, ForceMode.Impulse);
            coll.gameObject.GetComponent<BasicEnemy>().health -= damage;
        }
    }

    IEnumerator coolDown() 
    {
        yield return new WaitForSeconds(0.5f);
        //isAttacking = false;

        transform.position = hand.position;
        yield return new WaitForSeconds(1.0f);
        
        isAtt = false;

    }

    IEnumerator glow(Material mat)
    {
        Color orig = mat.GetColor("_Color");
        //while (true)
        //{
            mat.color = Color.red;
            yield return new WaitForSeconds(0.7f);
            mat.color = orig;
        //}
        
        //isAttacking = false;


    }

}
