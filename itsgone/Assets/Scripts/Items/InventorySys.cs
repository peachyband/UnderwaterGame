using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class InventorySys : MonoBehaviour
{
    public int passspace = 7;

    public GameObject ActCam;
    public GameObject InvCam;

    [Space]
    public bool OpenedInv = false;
    private Inventory _invsys;



    private void Start()
    {
        _invsys = GameObject.FindGameObjectWithTag("InvSys").GetComponent<Inventory>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Consumble" || other.tag == "Equipment")
        { 
            if (Input.GetKeyDown("e") && other.GetComponent<WearScript>() != null)
            {
                if (_invsys.PassItems.Count < passspace)
                {
                    WearScript wear = other.GetComponent<WearScript>();
                    Rigidbody rb = other.GetComponent<Rigidbody>();
                    rb.freezeRotation = true;
                    rb.detectCollisions = true;
                    if (!wear.isEquiped)
                    {
                        Debug.Log("Picked " + other.transform.name + "!");
                        other.transform.rotation = transform.rotation;
                        other.transform.SetParent(gameObject.transform);
                        other.gameObject.SetActive(false);
                        if (other.transform.GetChild(0) != null && other.transform.GetChild(0).name != "default")
                        {
                            wear.ItemCell._ref = other.transform.GetChild(0).gameObject;
                            wear.ItemCell._wear = other.transform.GetChild(0).gameObject.GetComponent<WearScript>().clothType.ToString();
                        }
                        else 
                        {
                            wear.ItemCell._ref = other.gameObject;
                            wear.ItemCell._wear = wear.clothType.ToString();
                        }
                        _invsys.PassItems.Add(wear.ItemCell);
                        _invsys.ActivateRender = true;
                    }
                }
            }
        }
    }
    void Update()
    {
        if (Input.GetKeyDown("i"))
        {
            if (!OpenedInv)
            {
                OpenedInv = true;
                Cursor.lockState = CursorLockMode.None;
            } 
            else if (OpenedInv)
            {
                OpenedInv = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            ActCam.GetComponent<Camera>().enabled = !OpenedInv;
            InvCam.GetComponent<Camera>().enabled = OpenedInv;
        }
    }
}
