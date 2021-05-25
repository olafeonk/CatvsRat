using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

public class RoomInfo
{
    public string name;

    public int X;

    public int Y;
}

public class RoomController : MonoBehaviour
{
    public static RoomController instance;

    private string currentWorldName = "basement";

    private RoomInfo currentLoadRoomData;

    private Room currentRoom;

    private Queue<RoomInfo> loadRoomQueue = new Queue<RoomInfo>();
    
    public List<Room> LoadedRooms = new List<Room>();

    private bool isLoadingRoom;
    private bool spawnedBossRoom;
    private bool updatedRooms;

    void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        UpdateRoomQueue();
    }

    void UpdateRoomQueue()
    {
        if (isLoadingRoom)
        {
            return;
        }

        if (loadRoomQueue.Count == 0)
        {
            if (!spawnedBossRoom)
            {
                StartCoroutine(SpawnBossRoom());
            }
            else if (spawnedBossRoom && !updatedRooms)
            {
                foreach (var room in LoadedRooms)
                {
                    room.RemoveUnconnectedDoors();
                }
                UpdateRooms();
                updatedRooms = true;
            }
            return;
        }

        currentLoadRoomData = loadRoomQueue.Dequeue();
        isLoadingRoom = true;

        StartCoroutine(LoadRoomRoutine(currentLoadRoomData));
    }

    IEnumerator SpawnBossRoom()
    {
        spawnedBossRoom = true;
        yield return new WaitForSeconds(0.5f);
        if (loadRoomQueue.Count == 0)
        {
            var bossRoom = LoadedRooms[LoadedRooms.Count - 1];
            var temporaryRoom = gameObject.AddComponent<Room>();
            temporaryRoom.X = bossRoom.X;
            temporaryRoom.Y = bossRoom.Y;
            Destroy(bossRoom.gameObject);
            var roomToRemove = LoadedRooms.Single(r => r.X == temporaryRoom.X && r.Y == temporaryRoom.Y);
            LoadedRooms.Remove(roomToRemove);
            LoadRoom("End", temporaryRoom.X, temporaryRoom.Y);
        }
    }
    
    public void LoadRoom(string name, int x, int y)
    {
        if (DoesRoomExist(x, y))
        {
            return;
        }

        var newRoomData = new RoomInfo {name = name, X = x, Y = y};

        loadRoomQueue.Enqueue(newRoomData);
    }

    IEnumerator LoadRoomRoutine(RoomInfo info)
    {
        var roomName = currentWorldName + info.name;

        var loadRoom = SceneManager.LoadSceneAsync(roomName, LoadSceneMode.Additive);

        while (loadRoom.isDone == false)
        {
            yield return null;
        }
    }

    public void RegisterRoom(Room room)
    {
        if (!DoesRoomExist(currentLoadRoomData.X, currentLoadRoomData.Y))
        {
            var transform1 = room.transform;
            transform1.position = new Vector3(
                currentLoadRoomData.X * room.Width,
                currentLoadRoomData.Y * room.Height,
                0);

            room.X = currentLoadRoomData.X;
            room.Y = currentLoadRoomData.Y;
            room.name = currentWorldName + "-" + currentLoadRoomData.name + " " + room.X + ", " + room.Y;
            transform1.parent = transform;

            isLoadingRoom = false;

            if (LoadedRooms.Count == 0)
            {
                CameraController.instance.currentRoom = room;
            }

            LoadedRooms.Add(room);
        }
        else
        {
            Destroy(room.gameObject);
            isLoadingRoom = false;
        }
    }
    
    public bool DoesRoomExist(int x, int y)
    {
        return LoadedRooms.Find(item => item.X == x && item.Y == y) != null;
    }
    
    public Room FindRoom(int x, int y)
    {
        return LoadedRooms.Find(item => item.X == x && item.Y == y);
    }

    public string GetRandomRoomName()
    {
        var possibleRooms = new [] {
            "Empty",
            "Basic1",
            "Basic2"
        };

        return possibleRooms[Random.Range(0, possibleRooms.Length)];
    }
    public void OnPlayerEnterRoom(Room room)
    {
        CameraController.instance.currentRoom = room;
        currentRoom = room;
        
        StartCoroutine(RoomCoroutine());
    }
    
    
    public IEnumerator RoomCoroutine()
    {
        yield return new WaitForSeconds(0.2f);
        UpdateRooms();
    }

    public void UpdateRooms()
    {
        foreach(var room in LoadedRooms)
        {
            if (currentRoom == room)
            {
            
                var enemies = room.GetComponentsInChildren<EnemyControler>();
                Debug.Log(enemies.Length);
                if(enemies.Length > 0)
                {
                    foreach(var enemy in enemies)
                    {
                        enemy.notInRoom = false;
                        Debug.Log("In room");
                    }
                    
                    foreach(var door in room.GetComponentsInChildren<Door>())
                    {
                        door.doorCollider.SetActive(true);
                    }
                }
                else
                {
                    foreach(var door in room.GetComponentsInChildren<Door>())
                    {
                        door.doorCollider.SetActive(false);
                    }
                }  
            }
        }
    }
}
