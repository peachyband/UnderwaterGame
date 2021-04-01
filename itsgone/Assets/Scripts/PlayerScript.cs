using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]
public class PlayerScript : MonoBehaviour
{
    public enum bodyType
    {
        muscle = 1,
        thick = 2,
        thin = 3
    }
    [Header("Player characterisic")]
    public bodyType PlayerBuild;
    public float speed = 6.0f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public int lives = 5;

    private Vector3 moveDirection = Vector3.zero;
    private float initialLives = 5;
    public bool isGround = false;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        initialLives = lives;
    }

}
