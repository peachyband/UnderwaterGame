using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : MonoBehaviour
{
    public float a = 2f;
    public float b = 3f;
    public float speed = 15f;
    public float t = 0;
    private Vector3 sPos;

    private float x = 0;
    private float y = 0;
    private float z = 0;
    public int dir = 1;
    // Start is called before the first frame update
    void Start()
    {
        //sPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = sPos + new Vector3(x, y, z);

        //t = Mathf.Clamp(t, 0f, 2 * Mathf.PI);
        /*x = a * Mathf.Cos(t);
        y = a * Mathf.Sin(t);
        z = b * t;*/

        x = -a * Mathf.Sin(t);
        y = a * Mathf.Cos(t);
        z = b;

        transform.Translate(x, y, z);

        t += speed * Time.deltaTime * dir;
    }

    private void OnCollisionEnter(Collision collision)
    {
        dir = -dir;
    }
}
