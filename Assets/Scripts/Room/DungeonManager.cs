using System.Collections.Generic;
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
   private readonly Vector2[] _directions = {Vector2.up, Vector2.down, Vector2.left, Vector2.right};


   private RoomData[,] _roomsLayout;
   private Vector2 _currentRoomPosition;

   // Live variables
   private List<bool> _roomsCleared;

   private void Start()
   {
      _roomsLayout = new RoomData[rooms * 2, rooms * 2];
      GenerateDungeonLayout();
   }

   // Generate the Room
   private void GenerateRoom()
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
      //! Debug Time
      float startTime = Time.realtimeSinceStartup;

      Vector2 currentRoomGenerationPosition = new Vector2(rooms / 2, rooms / 2);
      _roomsLayout[rooms / 2, rooms / 2] = startRoom;
      // Create the rest of the rooms
      for (int i = 1; i < rooms; i++)
      {
         bool roomGenerated = TryGenerateRoomInAllDirections(currentRoomGenerationPosition);
         if (Random.Range(0, 4) == 0 || !roomGenerated)
         {
            currentRoomGenerationPosition += _directions[Random.Range(0, _directions.Length)];
         }
         if (!roomGenerated)
         {
            i--;
         }
      }
      Debug.Log("<color=green>Generated dungeon layout in " + (Time.realtimeSinceStartup - startTime) + " seconds</color>");
      DebugPrintDungeonLayout();
   }
   private bool TryGenerateRoomInAllDirections(Vector2 generationPosition)
   {
      foreach (Vector2 direction in _directions)
      {
         if (IsRoomInDirectionEmpty(generationPosition, direction))
         {
            GenerateRoomInDirection(generationPosition, direction);
            return true;
         }
      }
      return false;
   }

   private bool IsRoomInDirectionEmpty(Vector2 generationPosition, Vector2 direction)
   {
      return _roomsLayout[(int)generationPosition.x + (int)direction.x, (int)generationPosition.y + (int)direction.y] == null;
   }

   private void GenerateRoomInDirection(Vector2 generationPosition, Vector2 direction)
   {
      _roomsLayout[(int)generationPosition.x + (int)direction.x, (int)generationPosition.y + (int)direction.y] =
         GetRandomRoomData();
   }

   private void DebugPrintDungeonLayout()
   {
      for (int i = 0; i < _roomsLayout.GetLength(0); i++)
      {
         for (int j = 0; j < _roomsLayout.GetLength(1); j++)
         {
            if (_roomsLayout[i, j] != null)
            {
               Debug.Log("<color=yellow>Room at </color>" + i + ", " + j + " is " + "<color=red>" + _roomsLayout[i, j].name + "</color>");
            }
         }
      }
   }

   public void ChangeRoom(Vector2 direction)
   {
      // Change the current room position
      _currentRoomPosition += direction;
   }
}
