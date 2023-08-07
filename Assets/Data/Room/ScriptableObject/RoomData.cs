using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New RoomData", menuName = "Room/RoomData")]
public class RoomData : ScriptableObject
{
    // Player Prefab
    [Header("Player Prefab (Start Room Only)")]
    public GameObject playerPrefab;
    public GameObject playerCameraPrefab;


    [Header("Door Data")]
    public bool hasNorthDoor;
    public bool hasSouthDoor;
    public bool hasEastDoor;
    public bool hasWestDoor;
    public bool isStartRoom;
    
    [Header("Room Data")]
    public List<GameObject> enemies;
    public List<GameObject> traps;
    public List<GameObject> loot;

    [Header("Room Type")]
    public bool isCombatRoom;
    public bool isTrapRoom;
    public bool isLootRoom;
    public bool isBossRoom;

    [Header("Room Prefab")]
    public GameObject roomPrefab;
}