using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class PlayerScript : MonoBehaviour
{
    public int health = 100;
    public int maxHealth = 100;

    public float oxygen = 100;

    public Image hImage;
    public Text healthText;
    public Image oxImage;
    public Text oxText;
    public Image lowH;
    public Image scroll;
    private Color lowC;

    private CharacterController charCntrl;
    private float velocityY = 0f;
    public PlayerCamera pCamera;
    public float anglz = 0f;
    //public bool isGr = false;
    //private int m = 1;

    [Header("Player characterisic")]
    public float speed = 6.0f;
    public float jumpSpeed = 23.0f;
    public float gravity = 9.81f;
    //public int lives = 5;

    private float initialLives = 5;

    void Start()
    {
        charCntrl = this.GetComponent<CharacterController>();
        pCamera = Camera.main.GetComponent<PlayerCamera>();
        Cursor.lockState = CursorLockMode.Locked;
        lowC = Color.red;
        lowC.a = 0f;
        //initialLives = lives;
    }

    void Update()
    {
        oxygen = Mathf.Clamp(oxygen, 0, 100);
        oxygen -= Time.deltaTime * 2;
        oxImage.fillAmount = oxygen / 100;
        oxText.text = "OXYGEN: " + (int)oxygen;

        if (((float)health/maxHealth) <= 0.2f)
            lowC.a = 0.5f - ((float)health / maxHealth) * 0.5f;
        else lowC.a = 0f;
        //lowH.color = lowC;

        health = Mathf.Clamp(health, 0, maxHealth);
        hImage.fillAmount = (float)health / maxHealth;
        healthText.text = "HEALTH: " + health;
        //if (charCntrl.isGrounded) isGr = true;

        if (charCntrl.isGrounded && velocityY < 0) velocityY = -2f;

        if (Input.GetKeyDown(KeyCode.R))
        {
            health -= 20;
            //pCamera.enabled = false;
            //Quaternion q = Quaternion.Euler(0, 0, -90.0f);
            //transform.localRotation = q;
            //Camera.main.transform.localRotation = q;
            //m = -1;
            //pCamera.enabled = true;

            /*if (anglz >= -180f)
                anglz -= -90.0f;
            else anglz = 180f;/*/
        }

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        Vector3 dir = transform.right * moveX + transform.forward * moveZ;





        if (charCntrl.isGrounded && Input.GetButtonDown("Jump"))
        {
            //isGr = false;
            velocityY += jumpSpeed /** Time.deltaTime * 150*/;
            
        }

        charCntrl.Move(dir * speed * Time.deltaTime);
        velocityY -= gravity * Time.deltaTime;
        charCntrl.Move(transform.up * velocityY /*+ new Vector3(0, velocityY, 0)*/ *Time.deltaTime);

        
    }

    void OnCollisionStay(Collision col)
    {
        if (col.gameObject.tag == "Scroll")
        {
            Debug.Log("Text");
            scroll.gameObject.SetActive(true);
        }
    }

    void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == "Scroll")
        {
            Debug.Log("Text");
            if (Input.GetKeyDown(KeyCode.E))
                scroll.gameObject.SetActive(true);
        }
    }

}