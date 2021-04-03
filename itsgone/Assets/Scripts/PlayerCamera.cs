using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    private Transform player;
    public float senstivityX = 5;
    public float senstivityY = 1;
    private float currentX = 0;
    private float currentY = 1;
    public float minY = 30;
    public float maxY = 50;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    void Update()
    {
        currentX += Input.GetAxis("Mouse X");
        currentY -= Input.GetAxis("Mouse Y");
        currentX = Mathf.Repeat(currentX, 360);
        currentY = Mathf.Clamp(currentY, minY, maxY);
        //transform.position = GameObject.Find("mask").GetComponent<Transform>().position;
    }
    void UpdateRotation()
    {
        player.rotation = Quaternion.Euler(0, currentX, 0);
    }
    void LateUpdate()
    {
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        transform.rotation = rotation;
        UpdateRotation();
 
    }
}