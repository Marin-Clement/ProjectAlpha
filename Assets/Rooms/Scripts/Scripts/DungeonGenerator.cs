using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoungeonGenerator : MonoBehaviour
{
   // Dungeon generation variables
   [Header("Dungeon Generation")]
   [Tooltip("The minimum number of rooms in the dungeon"),SerializeField] private int minRooms = 5;
   [Tooltip("The maximum number of rooms in the dungeon"), SerializeField] private int maxRooms = 10;
   [Tooltip("Rooms prefabs to be used in the dungeon generation"), SerializeField] private GameObject[] roomsPrefabs;
   [Tooltip("The room where the player will start"), SerializeField] private GameObject startRoom;
   
   // Dungeon provivate variables
   private List<Room> _rooms;
}
