using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]
public class PlayerScript : MonoBehaviour
{
    private CharacterController charCntrl;
    private float velocityY = 0f;

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
        Cursor.lockState = CursorLockMode.Locked;
        initialLives = lives;
    }

    void Update()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        Vector3 dir = transform.right * moveX + transform.forward * moveZ;
        if (charCntrl.isGrounded && velocityY < 0) velocityY = 0;
        velocityY += -9.81f * Time.deltaTime;

        if (charCntrl.isGrounded && Input.GetButtonDown("Jump"))
            velocityY += jumpSpeed * Time.deltaTime * 100;

        charCntrl.Move(dir * speed * Time.deltaTime);
        charCntrl.Move(new Vector3 (0, velocityY, 0) * Time.deltaTime);
    }

}
