using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : MonoBehaviour
{
    public int health = 100;
    public int damage = 20;

    private Rigidbody rig;

    public float a = 2f;
    public float b = 3f;
    public float speed = 15f;
    public float t = 0;
    private Vector3 sPos;
    private Vector3 velocity = Vector3.zero;

    private float x = 0;
    private float y = 0;
    private float z = 0;
    public int dir = 1;

    private Transform player;
    // Start is called before the first frame update
    void Start()
    {
        //sPos = transform.position;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rig = this.gameObject.GetComponent<Rigidbody>();
        rig.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {

        if (health <= 0)
            Destroy(this.gameObject);

        //transform.position = sPos + new Vector3(x, y, z);

        //t = Mathf.Clamp(t, 0f, 2 * Mathf.PI);
        /*x = a * Mathf.Cos(t);
        y = a * Mathf.Sin(t);
        z = b * t;*/

        /*x = -a * Mathf.Sin(t);
        y = a * Mathf.Cos(t);
        z = b;

        transform.Translate(x, y, z);

        t += speed * Time.deltaTime * dir;*/

        //transform.position = Vector3.SmoothDamp(transform.position, player.position, ref velocity, 0.3f);
    }

   

    private void OnCollisionEnter(Collision collision)
    {
        //dir = -dir;
    }
}
