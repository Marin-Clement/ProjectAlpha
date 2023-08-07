using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class DungeonManager : MonoBehaviour
{
   // Dungeon generation variables
   [Header("Dungeon Generation")]

   [SerializeField] private int rooms = 10;

   [SerializeField] private GameObject[] roomsPrefabs;
   
   [SerializeField] private GameObject startRoom;
   
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
