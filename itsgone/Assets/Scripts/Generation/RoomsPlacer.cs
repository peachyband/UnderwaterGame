using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomsPlacer : MonoBehaviour
{
    public Room[] RoomPrefabs;
    public List<string> spawnedBridges;
    public Room StartingRoom;
    public GameObject noll;
    public GameObject deskPref;
    public GameObject _container;
    public int RoomCount;
    public Vector3Int Length;
    public Vector3Int Center;
    private Room[,,] spawnedRooms;

    private void Start()
    {
        spawnedRooms = new Room[Length.x, Length.y, Length.z];
        spawnedRooms[Center.x, Center.y, Center.z] = StartingRoom;
        for (int i = 0; i < RoomCount - 1 ; i++)
        {
            PlaceOneRoom();
        }
        foreach(Transform room in _container.transform)
        {
            for(int y = 1; y < spawnedRooms.GetLength(1) - 1; y++)
            {
                for (int x = 1; x < spawnedRooms.GetLength(0) - 1; x++)
                {
                    for (int z = 1; z < spawnedRooms.GetLength(2) - 1; z++)
                    {
                        if (spawnedRooms[x, y, z] != null)
                        {
                            if (spawnedRooms[x, y, z + 1] != null &&
                               !spawnedBridges.Contains("between " + spawnedRooms[x, y, z].name + " and " + spawnedRooms[x, y, z + 1].name) &&
                               !spawnedBridges.Contains("between " + spawnedRooms[x, y, z + 1].name + " and " + spawnedRooms[x, y, z].name) && 
                               spawnedRooms[x, y, z].Neighboors.Contains(spawnedRooms[x, y, z + 1])) 
                            {
                                MakeABridge(spawnedRooms[x, y, z], spawnedRooms[x, y, z + 1], new Vector3(0, 0, 1));
                            }
                            if (spawnedRooms[x, y, z - 1] != null &&
                               !spawnedBridges.Contains("between " + spawnedRooms[x, y, z].name + " and " + spawnedRooms[x, y, z - 1].name) &&
                               !spawnedBridges.Contains("between " + spawnedRooms[x, y, z - 1].name + " and " + spawnedRooms[x, y, z].name) &&
                               spawnedRooms[x, y, z].Neighboors.Contains(spawnedRooms[x, y, z - 1]))
                            {
                                MakeABridge(spawnedRooms[x, y, z], spawnedRooms[x, y, z - 1], new Vector3(0, 0, -1));
                            }
                            if (spawnedRooms[x + 1, y, z] != null &&
                               !spawnedBridges.Contains("between " + spawnedRooms[x, y, z].name + " and " + spawnedRooms[x + 1, y, z].name) &&
                               !spawnedBridges.Contains("between " + spawnedRooms[x + 1, y, z].name + " and " + spawnedRooms[x, y, z].name) &&
                               spawnedRooms[x, y, z].Neighboors.Contains(spawnedRooms[x + 1, y, z]))
                            {
                                MakeABridge(spawnedRooms[x, y, z], spawnedRooms[x + 1, y, z], new Vector3(1, 0, 0));
                            }
                            if (spawnedRooms[x - 1, y, z] != null &&
                               !spawnedBridges.Contains("between " + spawnedRooms[x, y, z].name + " and " + spawnedRooms[x - 1, y, z].name) &&
                               !spawnedBridges.Contains("between " + spawnedRooms[x - 1, y, z].name + " and " + spawnedRooms[x, y, z].name) &&
                               spawnedRooms[x, y, z].Neighboors.Contains(spawnedRooms[x - 1, y, z]))
                            {
                                MakeABridge(spawnedRooms[x, y, z], spawnedRooms[x - 1, y, z], new Vector3(-1, 0, 0));
                            }
                        }
                    }
                }
            }
        }
        
    }

    private void PlaceOneRoom()
    {
        HashSet<Vector3Int> vacantPlaces = new HashSet<Vector3Int>();
        Room newRoom = Instantiate(RoomPrefabs[Random.Range(0, RoomPrefabs.Length)], _container.transform);
        
        for (int x = 0; x < spawnedRooms.GetLength(0); x++)
        {
            for (int y = 0; y < spawnedRooms.GetLength(1); y++)
            {
                for(int z = 0; z < spawnedRooms.GetLength(2); z++)
                {
                    if (spawnedRooms[x, y, z] == null) continue;

                    int maxX = spawnedRooms.GetLength(0) - 1;
                    int maxY = spawnedRooms.GetLength(1) - 1;
                    int maxZ = spawnedRooms.GetLength(2) - 1;
                    
                    if (x > 0 && spawnedRooms[x - 1, y, z] == null) vacantPlaces.Add(new Vector3Int(x - 1, y, z));
                    if (y > 0 && spawnedRooms[x, y - 1, z] == null) vacantPlaces.Add(new Vector3Int(x, y - 1, z));
                    if (z > 0 && spawnedRooms[x, y, z - 1] == null) vacantPlaces.Add(new Vector3Int(x, y, z - 1));
                    if (x < maxX && spawnedRooms[x + 1, y, z] == null) vacantPlaces.Add(new Vector3Int(x + 1, y, z));
                    if (y < maxY && spawnedRooms[x, y + 1, z] == null) vacantPlaces.Add(new Vector3Int(x, y + 1, z));
                    if (z < maxZ && spawnedRooms[x, y, z + 1] == null) vacantPlaces.Add(new Vector3Int(x, y, z + 1));
                }
            }
        }

        // Эту строчку можно заменить на выбор комнаты с учётом её вероятности, вроде как в ChunksPlacer.GetRandomChunk()

        int limit = 500;
        while (limit-- > 0)
        {
            // Эту строчку можно заменить на выбор положения комнаты с учётом того насколько он далеко/близко от центра,
            // или сколько у него соседей, чтобы генерировать более плотные, или наоборот, растянутые данжи
            Vector3Int position = vacantPlaces.ElementAt(Random.Range(0, vacantPlaces.Count));
            //newRoom.RotateRandomly();
            var bound = newRoom.transform.GetChild(0).GetComponent<MeshFilter>().sharedMesh.bounds;
            float priorityX = (float)(bound.size.x / 10);
            float priorityY = (float)(bound.size.y / 10);
            float priorityZ = (float)(bound.size.z / 10);
            if (ConnectToSomething(newRoom, position))
            {
                newRoom.transform.position = new Vector3((position.x - Center.x) * bound.size.z * priorityX
                                                , (position.y - Center.y) * bound.size.z * priorityY
                                                    , (position.z - Center.z) * bound.size.z * priorityZ) ;
                                                        spawnedRooms[position.x, position.y, position.z] = newRoom;
                                                            newRoom.name = position.x + " " + position.y + " " + position.z;
                return;
            }
        }
        Destroy(newRoom.gameObject);
    }

    public void MakeABridge(Room currentRoom, Room newRoom, Vector3 direction)
    {
        float dist = Vector3.Distance(currentRoom.transform.position, newRoom.transform.position);
        float size = deskPref.transform.GetChild(0).GetComponent<MeshCollider>().sharedMesh.bounds.size.z;
        float amount = dist / size;
        GameObject bridge = Instantiate(noll, currentRoom.transform.position, Quaternion.identity, currentRoom.transform);
        for(int i = 0; i < amount; i++)
        {
            GameObject desk = Instantiate(deskPref, currentRoom.transform.position + direction * size * i, Quaternion.Euler(new Vector3(0, Mathf.Max(direction.x * 90, direction.z * 90), 0)), bridge.transform);
            desk.name = "desk";
        }
        bridge.name = "between " + currentRoom.name + " and " + newRoom.name;
        spawnedBridges.Add(bridge.name);
        Debug.Log("distance is: " + dist + " so there are " + amount + " needed to build a desk");
    }

    private bool ConnectToSomething(Room room, Vector3Int p)
    {
        int maxX = spawnedRooms.GetLength(0) - 1;
        int maxY = spawnedRooms.GetLength(1) - 1;
        int maxZ = spawnedRooms.GetLength(2) - 1;
        List<Vector3Int> neighbours = new List<Vector3Int>();

        if (room.DoorR != null && p.x < maxX && spawnedRooms[p.x + 1, p.y, p.z]?.DoorL != null) neighbours.Add(Vector3Int.right);
        if (room.DoorL != null && p.x > 0 && spawnedRooms[p.x - 1, p.y, p.z]?.DoorR != null) neighbours.Add(Vector3Int.left);
        if (room.DoorU != null && p.y < maxY && spawnedRooms[p.x, p.y + 1, p.z]?.DoorD != null) neighbours.Add(Vector3Int.up);
        if (room.DoorD != null && p.y > 0 && spawnedRooms[p.x, p.y - 1, p.z]?.DoorU != null) neighbours.Add(Vector3Int.down);
        if (room.DoorF != null && p.z < maxZ && spawnedRooms[p.x, p.y, p.z + 1]?.DoorB != null) neighbours.Add(Vector3Int.forward);
        if (room.DoorB != null && p.z > 0 && spawnedRooms[p.x, p.y, p.z - 1]?.DoorF != null) neighbours.Add(Vector3Int.back);

        if (neighbours.Count == 0) return false;

        Vector3Int selectedDirection = neighbours[Random.Range(0, neighbours.Count)];
        Room selectedRoom = spawnedRooms[p.x + selectedDirection.x, p.y + selectedDirection.y, p.z + selectedDirection.z];
        
        if (selectedDirection == Vector3Int.up)
        {
            room.DoorU.SetActive(false);
            selectedRoom.DoorD.SetActive(false);
            if(!room.Neighboors.Contains(selectedRoom)) selectedRoom.Neighboors.Add(selectedRoom);
            if(!selectedRoom.Neighboors.Contains(room)) selectedRoom.Neighboors.Add(room);
        }
        else if (selectedDirection == Vector3Int.down)
        {
            room.DoorD.SetActive(false);
            selectedRoom.DoorU.SetActive(false);
            if (!room.Neighboors.Contains(selectedRoom)) selectedRoom.Neighboors.Add(selectedRoom);
            if (!selectedRoom.Neighboors.Contains(room)) selectedRoom.Neighboors.Add(room);
        }
        else if (selectedDirection == Vector3Int.right)
        {
            room.DoorR.SetActive(false);
            selectedRoom.DoorL.SetActive(false);
            if (!room.Neighboors.Contains(selectedRoom)) selectedRoom.Neighboors.Add(selectedRoom);
            if (!selectedRoom.Neighboors.Contains(room)) selectedRoom.Neighboors.Add(room);
        }
        else if (selectedDirection == Vector3Int.left)
        {
            room.DoorL.SetActive(false);
            selectedRoom.DoorR.SetActive(false);
            if (!room.Neighboors.Contains(selectedRoom)) selectedRoom.Neighboors.Add(selectedRoom);
            if (!selectedRoom.Neighboors.Contains(room)) selectedRoom.Neighboors.Add(room);
        }
        else if (selectedDirection == Vector3Int.forward)
        {
            room.DoorF.SetActive(false);
            selectedRoom.DoorB.SetActive(false);
            if (!room.Neighboors.Contains(selectedRoom)) selectedRoom.Neighboors.Add(selectedRoom);
            if (!selectedRoom.Neighboors.Contains(room)) selectedRoom.Neighboors.Add(room);
        }
        else if (selectedDirection == Vector3Int.back)
        {
            room.DoorB.SetActive(false);
            selectedRoom.DoorF.SetActive(false);
            if (!room.Neighboors.Contains(selectedRoom)) selectedRoom.Neighboors.Add(selectedRoom);
            if (!selectedRoom.Neighboors.Contains(room)) selectedRoom.Neighboors.Add(room);
        }
        return true;
    }
   
}