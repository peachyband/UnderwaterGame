using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]
public class PlayerScript : MonoBehaviour
{
    private CharacterController charCntrl;
    private float velocityY = 0f;
    public PlayerCamera pCamera;
    public float anglz = 0f;
    //private int m = 1;

    [Header("Player characterisic")]
    public float speed = 6.0f;
    public float jumpSpeed = 23.0f;
    public float gravity = 20.0f;
    public int lives = 5;

    private Vector3 moveDirection = Vector3.zero;
    private float initialLives = 5;
    public bool isGround = false;

    void Start()
    {
        charCntrl = this.GetComponent<CharacterController>();
        pCamera = Camera.main.GetComponent<PlayerCamera>();
        Cursor.lockState = CursorLockMode.Locked;
        initialLives = lives;
    }

    void Update()
    {

        if (Input.GetKey(KeyCode.R))
        {
            //pCamera.enabled = false;
            Quaternion q = Quaternion.Euler(0, 0, -90.0f);
            transform.localRotation = q;
            Camera.main.transform.localRotation = q;
            //m = -1;
            //pCamera.enabled = true;

            /*if (anglz >= -180f)
                anglz -= -90.0f;
            else anglz = 180f;/*/
        }

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        Vector3 dir = transform.right * moveX + transform.forward * moveZ;
        if (charCntrl.isGrounded && velocityY < 0) velocityY = 0;
        velocityY += -9.81f * Time.deltaTime;

        if (charCntrl.isGrounded && Input.GetButtonDown("Jump"))
            velocityY += jumpSpeed * Time.deltaTime * 75;

        charCntrl.Move(dir * speed * Time.deltaTime);
        charCntrl.Move(transform.up * velocityY /*+ new Vector3(0, velocityY, 0)*/ *Time.deltaTime);
    }

}