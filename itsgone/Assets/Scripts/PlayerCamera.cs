using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public float senstivityX = 5;
    public float senstivityY = 1;
    private float currentX = 0;
    private float currentY = 1;
    public float minY = 30;
    public float maxY = 50;
    void Start()
    {

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
        GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>().rotation = Quaternion.Euler(0, currentX, GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().anglz);
    }
    void LateUpdate()
    {
        Quaternion rotation = Quaternion.Euler(currentY, currentX, GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().anglz);
        transform.rotation = rotation;
        UpdateRotation();
        //if (GameObject.Find("Pockets").GetComponent<InventorySys>().OpenedInv == true && GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().isGround == false || GameObject.Find("Pockets").GetComponent<InventorySys>().OpenedInv == false)
        //{
        //}
        //else if (GameObject.Find("Pockets").GetComponent<InventorySys>().OpenedInv == true && GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().isGround == true)
        //{
        //    transform.position = new Vector3(transform.position.x, 2.5f, transform.position.z);
        //    transform.rotation = rotation;
        //}
    }
}