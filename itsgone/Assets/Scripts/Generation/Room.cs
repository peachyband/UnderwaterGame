using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Room : MonoBehaviour
{
    public bool scrolled = false;
    public Transform[] ScrollsPos;
    public GameObject DoorU;
    public GameObject DoorR;
    public GameObject DoorD;
    public GameObject DoorL;
    public GameObject DoorF;
    public GameObject DoorB;
    public List<Room> Neighboors;
    public enum Type
    {
        mobs = 1,
        tres = 2,
        mobs_n_tres = 3,
        tres_n_artfs = 4,
        mobs_n_artfs = 5,
        keys = 6
    }
    public Type roomType;
    public void RotateRandomly()
    {
        int count = Random.Range(0, 4);
        for (int i = 0; i < count; i++)
        {
            transform.Rotate(0, 90, 0);

            GameObject tmp = DoorL;
            DoorL = DoorB;
            DoorB = DoorR;
            DoorR = DoorF;
            DoorF = tmp;
        }
    }
}