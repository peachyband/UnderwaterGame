using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    private float[] clipSize = new float[numWeapons];
    public const int numWeapons = 2;
    [Header("Weapon objects:")] public GameObject[] weapons = new GameObject[numWeapons];
    int currentWeapon = 0;
    bool ableToShoot = true;
    bool reloaded = true;

    IEnumerator reload()
    {
        yield return new WaitForSeconds(weapons[currentWeapon].GetComponent<RangeProp>().reloadTime);
        reloaded = true;
        weapons[currentWeapon].GetComponent<RangeProp>().holder = clipSize[currentWeapon];
    }
    IEnumerator delay()
    {
        yield return new WaitForSeconds(weapons[currentWeapon].GetComponent<RangeProp>().shootDelay);
        ableToShoot = true;
    }
    private void WeaponChoose()
    {
        if (Input.GetAxis("MouseScrollWheel") < 0)
        {
            if (currentWeapon + 1 <= numWeapons)
            {
                currentWeapon++;
            }
            else
            {
                currentWeapon = 0;
            }
        }
        else if (Input.GetAxis("MouseScrollWheel") > 0)
        {
            if (currentWeapon - 1 >= 0)
            {
                currentWeapon--;
            }
            else
            {
                currentWeapon = numWeapons;
            }
        }
        SelectWeapon(currentWeapon);

    }
    private void SelectWeapon(int curwp)
    {
        for(int i = 0; i < numWeapons; i++)
        {
            if (i == curwp) { weapons[i].SetActive(true); } 
            else weapons[i].SetActive(false);
        }
    }
    public void MakeAFire(float currX, float currY)
    {
        if (weapons[currentWeapon].GetComponent<RangeProp>().damageType.HasFlag(RangeProp.DamageType.Range) && ableToShoot && reloaded)
        {
            if (weapons[currentWeapon].GetComponent<RangeProp>().holder <= clipSize[currentWeapon] && weapons[currentWeapon].GetComponent<RangeProp>().holder > 0) {
                GameObject bullet = Instantiate(weapons[currentWeapon].GetComponent<RangeProp>().ammo, weapons[currentWeapon].GetComponent<RangeProp>().muzzle.position, Quaternion.identity, null);
                bullet.transform.rotation = Quaternion.Euler(currY, currX, 0);
                bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * weapons[currentWeapon].GetComponent<RangeProp>().weaponForce,ForceMode.Impulse);
                weapons[currentWeapon].GetComponent<RangeProp>().holder--;
            } else {
                reloaded = false;
                StartCoroutine(reload());
            }
        }   else if (weapons[currentWeapon].GetComponent<RangeProp>().damageType.HasFlag(RangeProp.DamageType.Close)){ 
        }
    }

    private void Start()
    {
        for(int i = 0; i < numWeapons; i++)
        {
            clipSize[i] = weapons[i].GetComponent<RangeProp>().holder;
        }
    }

    private void Update()
    {
        WeaponChoose();

    }
}
