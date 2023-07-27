using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class DungeonGenerator : MonoBehaviour
{
   // Dungeon generation variables
   [FormerlySerializedAs("Rooms")]
   [Header("Dungeon Generation")]
   [Tooltip("Number of rooms to be generated"), SerializeField]
   private int rooms = 10;
   [Tooltip("Rooms prefabs to be used in the dungeon generation"), SerializeField] 
   private GameObject[] roomsPrefabs;
   [Tooltip("The room where the player will start"), SerializeField] 
   private GameObject startRoom;
   
   // Dungeon private variables
   private List<Room> _rooms;
   private Room[,] _roomsGrid; 
   private int _numberOfRooms;
   
   
   // Start is called before the first frame update
   private void Start()
   {
      _rooms = new List<Room>();
      _numberOfRooms = rooms;
      GenerateDungeon();
   }
   
   // Generate the dungeon
   private void GenerateDungeon()
   {
      
   }
}
