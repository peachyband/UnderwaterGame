using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomsPlacer : MonoBehaviour
{
    public Room[] KeyRoomPrefabs;
    public Room[] TresRoomPrefabs;
    public Room[] ArtfRoomPrefabs;
    public GameObject[] ScrollPrefabs;
    public List<string> spawnedBridges;
    public Room StartingRoom;
    public GameObject noll;
    public GameObject deskPref;
    public GameObject _container;
    public int RoomCount;
    public bool[] entranceY;
    public Vector3Int Length;
    public Vector3Int Center;
    private Room[,,] spawnedRooms;
    private int ArtfRooms;
    private int KeyRooms;
    private int MobRooms;
    private int TresRooms;
    private int MobNTres;
    private int ArtfsNMobs;
    private int ArtfsNTres;

    private void Start()
    {
        spawnedRooms = new Room[Length.x, Length.y, Length.z];
        spawnedRooms[Center.x, Center.y, Center.z] = StartingRoom;
        entranceY = new bool[Length.y];
        KeyRooms = 3;
        ArtfRooms = 4;
        MobRooms = TresRooms = MobNTres = (RoomCount - (KeyRooms + ArtfRooms)) / 3;
        ArtfsNTres = Random.Range(1, RoomCount - (KeyRooms + MobRooms + TresRooms + MobNTres) - 1);
        ArtfsNMobs = ArtfRooms - ArtfsNTres;
        Debug.Log("KeyRooms count is: " + KeyRooms +"; TresRooms count is: " + TresRooms + ";\n" +
            "MobRooms count is: " + MobRooms + "; MobNTres count is: " + MobNTres) ;
        Debug.Log("ArtfsNTres count is: " + ArtfsNTres + "; ArtfsNMobs count is: " + ArtfsNMobs);


        for (int i = 0; i < RoomCount - 1 ; i++)
        {
            PlaceOneRoom();
        }
        foreach (Transform room in _container.transform)
        {
            int maxX = spawnedRooms.GetLength(0) - 1;
            int maxZ = spawnedRooms.GetLength(2) - 1;

            for (int y = 0; y < spawnedRooms.GetLength(1); y++)
            {
                for (int x = 0; x < spawnedRooms.GetLength(0); x++)
                {
                    for (int z = 0; z < spawnedRooms.GetLength(2); z++)
                    {
                        if (spawnedRooms[x, y, z] != null)
                        {
                            if (z < maxZ && spawnedRooms[x, y, z + 1] != null &&
                               !spawnedBridges.Contains("between " + spawnedRooms[x, y, z].name + " and " + spawnedRooms[x, y, z + 1].name) &&
                               !spawnedBridges.Contains("between " + spawnedRooms[x, y, z + 1].name + " and " + spawnedRooms[x, y, z].name) &&
                               spawnedRooms[x, y, z].Neighboors.Contains(spawnedRooms[x, y, z + 1]))
                            {
                                MakeABridge(spawnedRooms[x, y, z], spawnedRooms[x, y, z + 1], new Vector3(0, 0, 1));
                                //Debug.Log("Founded space for bridge!");
                            }
                            if (z > 0 && spawnedRooms[x, y, z - 1] != null &&
                               !spawnedBridges.Contains("between " + spawnedRooms[x, y, z].name + " and " + spawnedRooms[x, y, z - 1].name) &&
                               !spawnedBridges.Contains("between " + spawnedRooms[x, y, z - 1].name + " and " + spawnedRooms[x, y, z].name) &&
                               spawnedRooms[x, y, z].Neighboors.Contains(spawnedRooms[x, y, z - 1]))
                            {
                                MakeABridge(spawnedRooms[x, y, z], spawnedRooms[x, y, z - 1], new Vector3(0, 0, -1));
                                //Debug.Log("Founded space for bridge!");
                            }
                            if (x < maxX && spawnedRooms[x + 1, y, z] != null &&
                               !spawnedBridges.Contains("between " + spawnedRooms[x, y, z].name + " and " + spawnedRooms[x + 1, y, z].name) &&
                               !spawnedBridges.Contains("between " + spawnedRooms[x + 1, y, z].name + " and " + spawnedRooms[x, y, z].name) &&
                               spawnedRooms[x, y, z].Neighboors.Contains(spawnedRooms[x + 1, y, z]))
                            {
                                MakeABridge(spawnedRooms[x, y, z], spawnedRooms[x + 1, y, z], new Vector3(1, 0, 0));
                                //Debug.Log("Founded space for bridge!");
                            }
                            if (x > 0 && spawnedRooms[x - 1, y, z] != null &&
                               !spawnedBridges.Contains("between " + spawnedRooms[x, y, z].name + " and " + spawnedRooms[x - 1, y, z].name) &&
                               !spawnedBridges.Contains("between " + spawnedRooms[x - 1, y, z].name + " and " + spawnedRooms[x, y, z].name) && 
                               spawnedRooms[x, y, z].Neighboors.Contains(spawnedRooms[x - 1, y, z]))
                            {
                                MakeABridge(spawnedRooms[x, y, z], spawnedRooms[x - 1, y, z], new Vector3(-1, 0, 0));
                                //Debug.Log("Founded space for bridge!");
                            }
                        }
                    }
                }
            }
        }
        foreach (GameObject scroll in ScrollPrefabs)
        {
            Transform pickedroom = _container.transform.GetChild(Random.Range(0, _container.transform.childCount));
            Room room = pickedroom.GetComponent<Room>();
            GameObject Scroll = Instantiate(ScrollPrefabs[Random.Range(0, ScrollPrefabs.Length)], room.transform);
            Scroll.transform.position = room.ScrollsPos[Random.Range(0, room.ScrollsPos.Length)].position;
            room.scrolled = true;
        }
    }

    private Room CatchType()
    {
        if (KeyRooms > 0)
        {
            Room newRoom = Instantiate(KeyRoomPrefabs[Random.Range(0, KeyRoomPrefabs.Length)], _container.transform);
            newRoom.roomType = Room.Type.keys;
            KeyRooms -= 1;
            return newRoom;
        }
        else if (MobRooms > 0)
        {
            Room newRoom = Instantiate(TresRoomPrefabs[Random.Range(0, TresRoomPrefabs.Length)], _container.transform);
            newRoom.roomType = Room.Type.mobs;
            MobRooms -= 1;
            return newRoom;
        }
        else if (TresRooms > 0)
        {
            Room newRoom = Instantiate(TresRoomPrefabs[Random.Range(0, TresRoomPrefabs.Length)], _container.transform);
            newRoom.roomType = Room.Type.tres;
            TresRooms -= 1;
            return newRoom;
        }
        else if (MobNTres > 0)
        {
            Room newRoom = Instantiate(TresRoomPrefabs[Random.Range(0, TresRoomPrefabs.Length)], _container.transform);
            newRoom.roomType = Room.Type.mobs_n_tres;
            MobNTres -= 1;
            return newRoom;
        }
        else if (ArtfsNMobs > 0)
        {
            Room newRoom = Instantiate(ArtfRoomPrefabs[Random.Range(0, ArtfRoomPrefabs.Length)], _container.transform);
            newRoom.roomType = Room.Type.mobs_n_artfs;
            ArtfsNMobs -= 1;
            return newRoom;
        }
        else if (ArtfsNTres > 0)
        {
            Room newRoom = Instantiate(ArtfRoomPrefabs[Random.Range(0, ArtfRoomPrefabs.Length)], _container.transform);
            newRoom.roomType = Room.Type.tres_n_artfs;
            ArtfsNTres -= 1;
            return newRoom;
        }
        else return StartingRoom;
    }

    private void PlaceOneRoom()
    {
        HashSet<Vector3Int> vacantPlaces = new HashSet<Vector3Int>();
        Room newRoom = CatchType();
        
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
                    if (y > 0 && spawnedRooms[x, y - 1, z] == null && entranceY[y - 1] == false) vacantPlaces.Add(new Vector3Int(x, y - 1, z));
                    if (z > 0 && spawnedRooms[x, y, z - 1] == null) vacantPlaces.Add(new Vector3Int(x, y, z - 1));
                    if (x < maxX && spawnedRooms[x + 1, y, z] == null) vacantPlaces.Add(new Vector3Int(x + 1, y, z));
                    if (y < maxY && spawnedRooms[x, y + 1, z] == null && entranceY[y] == false) vacantPlaces.Add(new Vector3Int(x, y + 1, z)); 
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
            var boxbound = newRoom.transform.GetChild(0).GetComponent<BoxCollider>().size;
            int priorityX = (int)(boxbound.x / 5);
            int priorityY = (int)(boxbound.y / 5);
            int priorityZ = (int)(boxbound.z / 5);
            Debug.Log("priorities: " + priorityX + "; " + priorityY + "; " + priorityZ);
            if (ConnectToSomething(newRoom, position))
            {
                newRoom.transform.position = new Vector3((position.x - Center.x) * boxbound.x * priorityX
                                                , (position.y - Center.y) * boxbound.y * priorityY
                                                    , (position.z - Center.z) * boxbound.z * priorityZ) ;
                                                        spawnedRooms[position.x, position.y, position.z] = newRoom;
                                                            newRoom.name = position.x + " " + position.y + " " + position.z;
                return;
            }
        }
        Destroy(newRoom.gameObject);
    }

    public void MakeABridge(Room currentRoom, Room newRoom, Vector3 direction)
    {
        var boxbound1 = currentRoom.transform.GetChild(0).GetComponent<BoxCollider>().size;
        var boxbound2 = newRoom.transform.GetChild(0).GetComponent<BoxCollider>().size;
        float distant = 0;
        if (direction.z == 1)
        {
            distant = Vector3.Distance(currentRoom.transform.position + new Vector3(0, 0, boxbound1.z / 2), 
                newRoom.transform.position - new Vector3(0, 0, boxbound2.z / 2));
        }
        else if (direction.z == -1)
        {
            distant = Vector3.Distance(currentRoom.transform.position - new Vector3(0, 0, boxbound1.z / 2),
                newRoom.transform.position + new Vector3(0, 0, boxbound2.z / 2));
        }
        else if (direction.x == 1)
        {
            distant = Vector3.Distance(currentRoom.transform.position + new Vector3(boxbound1.x / 2, 0, 0),
                newRoom.transform.position - new Vector3(boxbound2.x / 2, 0, 0));
        }
        else if (direction.x == -1)
        {
            distant = Vector3.Distance(currentRoom.transform.position - new Vector3(boxbound1.x / 2, 0, 0),
                newRoom.transform.position + new Vector3(boxbound2.x / 2, 0, 0));
        }
        float size = deskPref.transform.GetChild(0).GetComponent<MeshCollider>().sharedMesh.bounds.size.z;
        float amount = distant / size;
        GameObject bridge = Instantiate(noll, currentRoom.transform.position, Quaternion.identity, currentRoom.transform);
        for(int i = 0; i < amount; i++)
        {
            GameObject desk;
            if (direction.z == 1)
            {
                desk = Instantiate(deskPref, currentRoom.transform.position + new Vector3(0, 0, boxbound1.z / 2) + 
                    direction * size * i, Quaternion.Euler(new Vector3(0, -90, 0)), bridge.transform);
                desk.transform.name = "desk";
            }
            else if(direction.z == -1)
            {
                desk = Instantiate(deskPref, currentRoom.transform.position - new Vector3(0, 0, boxbound1.z / 2) + 
                    direction * size * i, Quaternion.Euler(new Vector3(0, 90, 0)), bridge.transform);
                desk.transform.name = "desk";
            }
            else if(direction.x == 1)
            {
                desk = Instantiate(deskPref, currentRoom.transform.position + new Vector3(boxbound1.x / 2, 0, 0) + 
                    direction * size * i, Quaternion.Euler(new Vector3(0, 0, 0)), bridge.transform);
                desk.transform.name = "desk";
            }
            else if(direction.x == -1)
            {
                desk = Instantiate(deskPref, currentRoom.transform.position - new Vector3(boxbound1.x / 2, 0, 0) + 
                    direction * size * i, Quaternion.Euler(new Vector3(0, -180, 0)), bridge.transform);
                desk.transform.name = "desk";
            }
        }
        bridge.name = "between " + currentRoom.name + " and " + newRoom.name;
        spawnedBridges.Add(bridge.name);
        Debug.Log("BUILDING: distance is: " + distant + " so there are " + amount + " needed to build a desk");
    }

    private bool ConnectToSomething(Room room, Vector3Int p)
    {
        int maxX = spawnedRooms.GetLength(0) - 1;
        int maxY = spawnedRooms.GetLength(1) - 1;
        int maxZ = spawnedRooms.GetLength(2) - 1;
        List<Vector3Int> neighbours = new List<Vector3Int>();

        if (room.DoorR != null && p.x < maxX && spawnedRooms[p.x + 1, p.y, p.z]?.DoorL != null) neighbours.Add(Vector3Int.right);
        if (room.DoorL != null && p.x > 0 && spawnedRooms[p.x - 1, p.y, p.z]?.DoorR != null) neighbours.Add(Vector3Int.left);
        if (room.DoorU != null && p.y < maxY && spawnedRooms[p.x, p.y + 1, p.z]?.DoorD != null && p.y != Center.y + 1 &&
            entranceY[p.y] == false && spawnedRooms[p.x, p.y + 1, p.z].Neighboors.Count != 0) neighbours.Add(Vector3Int.up);
        if (room.DoorD != null && p.y > 0 && spawnedRooms[p.x, p.y - 1, p.z]?.DoorU != null && p.y != Center.y - 1 &&
            entranceY[p.y - 1] == false && spawnedRooms[p.x, p.y - 1, p.z].Neighboors.Count != 0) neighbours.Add(Vector3Int.down);
        if (room.DoorF != null && p.z < maxZ && spawnedRooms[p.x, p.y, p.z + 1]?.DoorB != null) neighbours.Add(Vector3Int.forward);
        if (room.DoorB != null && p.z > 0 && spawnedRooms[p.x, p.y, p.z - 1]?.DoorF != null) neighbours.Add(Vector3Int.back);

        if (neighbours.Count == 0) return false;

        Vector3Int selectedDirection = neighbours[Random.Range(0, neighbours.Count)];
        Room selectedRoom = spawnedRooms[p.x + selectedDirection.x, p.y + selectedDirection.y, p.z + selectedDirection.z];
        
        if (selectedDirection == Vector3Int.up && p.y != Center.y + 1)
        {
            entranceY[p.y] = true;
            room.DoorU.SetActive(false);
            selectedRoom.DoorD.SetActive(false);
            if(!room.Neighboors.Contains(selectedRoom)) room.Neighboors.Add(selectedRoom);
            if(!selectedRoom.Neighboors.Contains(room)) selectedRoom.Neighboors.Add(room);
        }
        else if (selectedDirection == Vector3Int.down && p.y != Center.y - 1)
        {
            Debug.Log("Building y-pass with coordinates: " + new Vector3(p.x, p.y, p.z) + " and " + Center);
            entranceY[p.y - 1] = true;
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
            //MakeABridge(room, selectedRoom, new Vector3(1, 0, 0));
        }
        else if (selectedDirection == Vector3Int.left)
        {
            room.DoorL.SetActive(false);
            selectedRoom.DoorR.SetActive(false);
            if (!room.Neighboors.Contains(selectedRoom)) selectedRoom.Neighboors.Add(selectedRoom);
            if (!selectedRoom.Neighboors.Contains(room)) selectedRoom.Neighboors.Add(room);
            //MakeABridge(room, selectedRoom, new Vector3(-1, 0, 0));
        }
        else if (selectedDirection == Vector3Int.forward)
        {
            room.DoorF.SetActive(false);
            selectedRoom.DoorB.SetActive(false);
            if (!room.Neighboors.Contains(selectedRoom)) selectedRoom.Neighboors.Add(selectedRoom);
            if (!selectedRoom.Neighboors.Contains(room)) selectedRoom.Neighboors.Add(room);
            //MakeABridge(room, selectedRoom, new Vector3(0, 0, 1));
        }
        else if (selectedDirection == Vector3Int.back)
        {
            room.DoorB.SetActive(false);
            selectedRoom.DoorF.SetActive(false);
            if (!room.Neighboors.Contains(selectedRoom)) selectedRoom.Neighboors.Add(selectedRoom);
            if (!selectedRoom.Neighboors.Contains(room)) selectedRoom.Neighboors.Add(room);
            //MakeABridge(room, selectedRoom, new Vector3(0, 0, -1));
        }
        return true;
    }
   
}