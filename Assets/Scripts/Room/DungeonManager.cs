using System;
using UnityEngine;
using Random = UnityEngine.Random;

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

   [SerializeField] private int rooms;
   [SerializeField] private RoomData[] roomsData;
   [SerializeField] private RoomData startRoom;



   private RoomData[,] _roomsLayout;
   private bool[,] _roomsVisited;
   private Vector2 _currentRoomPosition;

   // Live variables


   private void Start()
   {
      _roomsLayout = new RoomData[rooms * 2, rooms * 2];
      GenerateDungeonLayout();
      DebugLayout();
   }

   // Generate the dungeon
   private void GenerateDungeon()
   {
      // instantiate as child of dungeon manager
   }

   private RoomData GetRandomRoomData()
   {
      int randomRoomIndex = Random.Range(0, roomsData.Length);
      if (!roomsData[randomRoomIndex].isStartRoom)
      {
         return roomsData[randomRoomIndex];
      }
      else
      {
         return GetRandomRoomData();
      }
   }

   private void GenerateDungeonLayout()
   {
      float startTime = Time.realtimeSinceStartup;
      Vector2 currentRoomGenerationPosition = new Vector2(rooms / 2, rooms / 2);
      bool asGeneratedaARoom = false;
      // Create the start room
      _roomsLayout[rooms / 2, rooms / 2] = startRoom;
      Debug.Log("Start room generated");

      // Create the rest of the rooms
      for (int i = 0; i < rooms; i++)
      {
         // check if room are already created around the current room
         if (_roomsLayout[(int)currentRoomGenerationPosition.x + 1, (int)currentRoomGenerationPosition.y] == null)
         {
            // Create a room to the right
            _roomsLayout[(int)currentRoomGenerationPosition.x + 1, (int)currentRoomGenerationPosition.y] =
               GetRandomRoomData();
            asGeneratedaARoom = true;
         }
         else if (_roomsLayout[(int)currentRoomGenerationPosition.x - 1, (int)currentRoomGenerationPosition.y] == null)
         {
            // Create a room to the left
            _roomsLayout[(int)currentRoomGenerationPosition.x - 1, (int)currentRoomGenerationPosition.y] =
               GetRandomRoomData();
            asGeneratedaARoom = true;
         }
         else if (_roomsLayout[(int)currentRoomGenerationPosition.x, (int)currentRoomGenerationPosition.y + 1] == null)
         {
            // Create a room to the top
            _roomsLayout[(int)currentRoomGenerationPosition.x, (int)currentRoomGenerationPosition.y + 1] =
               GetRandomRoomData();
            asGeneratedaARoom = true;
         }
         else if (_roomsLayout[(int)currentRoomGenerationPosition.x, (int)currentRoomGenerationPosition.y - 1] == null)
         {
            // Create a room to the bottom
            _roomsLayout[(int)currentRoomGenerationPosition.x, (int)currentRoomGenerationPosition.y - 1] =
               GetRandomRoomData();
            asGeneratedaARoom = true;
         }
         // have a one to four chance to change the direction of the room generation
         if (Random.Range(0, 4) == 0)
         {
            // change the direction of the room generation by one unit
            currentRoomGenerationPosition += new Vector2(Random.Range(-1, 1), Random.Range(-1, 1));
            asGeneratedaARoom = false;
         }
         if (!asGeneratedaARoom)
         {
            i--;
         }
         Debug.Log("Room " + i + " generated");
      }
      Debug.Log("Dungeon layout generated" + " in " + (Time.realtimeSinceStartup - startTime) + " seconds");
   }
   void DebugLayout()
   {
      for (int i = 0; i < rooms * 2; i++)
      {
         for (int j = 0; j < rooms * 2; j++)
         {
            if (_roomsLayout[i, j] != null)
            {
               Debug.Log("Room at " + i + " " + j + " is " + _roomsLayout[i, j].name);
            }
         }
      }
   }

   private void CreateRoom(RoomData roomData)
   {
      // Create the room
      GameObject room = Instantiate(roomData.roomPrefab, transform);
   }


   public void ChangeRoom(Vector2 direction)
   {
      // Change the current room position
      _currentRoomPosition += direction;
   }
}
