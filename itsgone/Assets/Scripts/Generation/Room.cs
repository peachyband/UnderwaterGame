using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Room : MonoBehaviour
{
    public GameObject DoorU;
    public GameObject DoorR;
    public GameObject DoorD;
    public GameObject DoorL;
    public GameObject DoorF;
    public GameObject DoorB;
    public List<Room> Neighboors;
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