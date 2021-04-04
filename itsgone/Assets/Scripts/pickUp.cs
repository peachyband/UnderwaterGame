using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pickUp : MonoBehaviour
{
    public Image scroll;
    public Text hint;
    private int dir = 1;
   // private bool bbool = false;

    public GameObject[] weapons;
    public weapon[] weaponScript;
    public PlayerCamera cam;
    // Start is called before the first frame update
    void Start()
    {
        weapons = GameObject.FindGameObjectsWithTag("Weapon");
        Debug.Log(weapons.Length);
        weaponScript = new weapon[weapons.Length];
        for (int i = 0; i < weapons.Length; i++)
            weaponScript[i] = weapons[i].GetComponent<weapon>();
        cam = Camera.main.GetComponent<PlayerCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        if ((dir < 0) && (Input.GetButton("Lift")))
        {
            changeTime();
            scroll.gameObject.SetActive(false);
        }
    }

    void changeTime() 
    {
        Time.timeScale -= dir;
        for (int i = 0; i < weapons.Length; i++)
            weaponScript[i].isAtt = !weaponScript[i].isAtt;
        cam.freeze = !cam.freeze;
        dir = -dir;
        Debug.Log(dir);
    }


    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Scroll")
        {
            hint.text = "������� � ��� ���������";
            hint.gameObject.SetActive(true);
        }
    }

    void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == "Scroll")
        {
            hint.text = "������� � ��� ���������";
            hint.gameObject.SetActive(true);
            Debug.Log("Text");
            if (Input.GetButton("Use"))
            {
                scroll.gameObject.SetActive(true);
                changeTime();
            }
        }
    }

    void OnTriggerExit(Collider col)
    {
        hint.text = "";
        hint.gameObject.SetActive(false);
    }
}
