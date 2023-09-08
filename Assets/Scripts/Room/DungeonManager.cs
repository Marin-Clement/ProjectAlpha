using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
   //! Singleton
   public static DungeonManager Instance { get; private set; }
     private void Awake()
   {
      if (Instance == null)
      {
         Instance = this;
         DontDestroyOnLoad(gameObject);
      }
      else
      {
         Destroy(gameObject);
      }
   }
   //! Singleton

   // Dungeon generation variables
   [Header("Dungeon Generation")]

   [SerializeField] private int rooms = 10;
   [SerializeField] private RoomData[] roomsData;
   [SerializeField] private GameObject startRoom;



   private Room[,] _roomsLayout;
   private Vector2 _currentRoomPosition;

   // Live variables


   private void Start()
   {
      GenerateDungeon();
   }
   
   // Generate the dungeon
   private void GenerateDungeon()
   {
      // instantiate as child of dungeon manager

   }

   private void GenerateDungeonLayout()
   {
      // Create the start room
      _roomsLayout[0, 0] = startRoom.GetComponent<Room>();

      // Create the rest of the rooms
      for (int i = 0; i < rooms; i++)
      {

      }
   }


   private void CreateRoom(Room room)
   {
      Instantiate(room.GetRoomData().roomPrefab, this.transform);
   }

   public void ChangeRoom(Vector2 direction)
   {
      // Change the current room position
      _currentRoomPosition += direction;
   }
}
