using UnityEngine;

public class BotBullet : MonoBehaviour
{
    public float reloadTime = 1;
    public int damage = 5;
    public float speed = 5;
    public float liveTime = 1;

    private Rigidbody rb;
    private Vector3 direction;

    private void Awake()
    {
        direction = new Vector3(0, 0, speed);
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (rb != null) rb.MovePosition(transform.TransformVector(direction) * Time.fixedDeltaTime + transform.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        var player = other.gameObject.GetComponentInParent<PlayerScript>();
        if (other.gameObject.isStatic)
        {
            Destroy(gameObject);
        }
        else if (player != null)
        {
            Destroy(gameObject);
        }
    }
}
