using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeProp : MonoBehaviour
{
    [Header("Weapon properties:")]
    [Range(0, 100f)] public float holder;
    [Range(0, 100f)] public float weaponForce;
    [Range(0, 100f)] public float shootDelay;
    [Range(0, 100f)] public float reloadTime;
    public enum DamageType{
        Range = 1,
        Close = 2
    }
    public enum WeaponType
    {
        LaserGun = 1,
        Throwable = 2,
        Point = 3,
        ArrowType = 4,
        Physical = 5
    }
    public DamageType damageType;
    public WeaponType weaponType;
    public GameObject ammo;
    public Transform muzzle;
    private WeaponController wpctrl;
    private InventorySys invsys;
    private float currentX = 0;
    private float currentY = 1;
    public float minY = 30;
    public float maxY = 50;


    private void Start()
    {
        wpctrl = GameObject.FindGameObjectWithTag("Player").GetComponent<WeaponController>();
        invsys = GameObject.Find("Pockets").GetComponent<InventorySys>();
    }
    private void Update()
    {
        currentX += Input.GetAxis("Mouse X");
        currentY -= Input.GetAxis("Mouse Y");

        currentX = Mathf.Repeat(currentX, 360);
        currentY = Mathf.Clamp(currentY, minY, maxY);

        if (invsys.OpenedInv == false) gameObject.transform.rotation = Quaternion.Euler(currentY, currentX, 0);

        if (Input.GetButtonDown("Fire1"))
        {
            wpctrl.MakeAFire(currentX,currentY);
        }
    }

}
