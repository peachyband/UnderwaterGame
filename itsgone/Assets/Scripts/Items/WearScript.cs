using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WearScript : MonoBehaviour
{
    public enum ClothType{
        mask = 1, 
        hat = 2,
        vest = 3,
        left_arm = 4,
        right_arm = 5,
        legs = 6,
        boots = 7,
        consumble = 8,
        allie = 9
    }
    public ClothType clothType;
    public float defenseForce;
    public float speedchange;
    public float damageForce;
    public float dexterity;
    public bool isEquiped = false;
    public bool unchangable = false;
    public AssetItem ItemCell;
}
