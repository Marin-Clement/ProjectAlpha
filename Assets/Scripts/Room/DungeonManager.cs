using System.Collections.Generic;
using UnityEditor;
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
   [SerializeField] private RoomData[] bossRoomsData;
   [SerializeField] private RoomData startRoom;
   private readonly Vector2[] _directions = {Vector2.up, Vector2.down, Vector2.left, Vector2.right};


   private RoomData[,] _roomsLayout;
   private Vector2 _currentRoomPosition;

   // Live variables
   private List<bool> _roomsCleared;
   private int _floor;

   private void Start()
   {
      _roomsLayout = new RoomData[rooms * 2, rooms * 2];
      _currentRoomPosition = new Vector2(rooms, rooms);
      _floor = 1;
      GenerateDungeonLayout();
      GenerateRoom();
   }

   // Generate the Room
   private void GenerateRoom()
   {
      RoomData roomData = _roomsLayout[(int)_currentRoomPosition.x, (int)_currentRoomPosition.y];
      if (roomData != null)
      {
         // instantiate the room as a child of the dungeon manager
         Instantiate(roomData.roomPrefab, transform);
      }
      else
      {
         Debug.LogError("Room data is null");
      }
   }

   public void ChangeRoom(Vector2 direction)
   {
      _currentRoomPosition += direction;
   }

   private RoomData GetRandomRoomData()
   {
      int randomRoomIndex = Random.Range(0, roomsData.Length);
      if (!roomsData[randomRoomIndex].isStartRoom)
      {
         return roomsData[randomRoomIndex];
      }
      return GetRandomRoomData();
   }

   private RoomData GetBossRoomData()
   {
      return bossRoomsData[_floor - 1];
   }

   public void ResetDungeon()
   {
      _roomsLayout = new RoomData[rooms * 2, rooms * 2];
      _currentRoomPosition = new Vector2(rooms, rooms);
      _floor = 1;
      GenerateDungeonLayout();
   }

   private void GenerateDungeonLayout()
   {
      //! Debug Time
      float startTime = Time.realtimeSinceStartup;

      Vector2 currentRoomGenerationPosition = new Vector2(rooms, rooms );
      _roomsLayout[rooms, rooms] = startRoom;

      for (int i = 1; i < rooms; i++)
      {
         bool roomGenerated = TryGenerateRoomInAllDirections(currentRoomGenerationPosition, i);
         if (Random.Range(0, 2) == 0 || !roomGenerated)
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
   private bool TryGenerateRoomInAllDirections(Vector2 generationPosition, int index)
   {
      foreach (Vector2 direction in _directions)
      {
         if (IsRoomInDirectionEmpty(generationPosition, direction))
         {
            if (index == rooms - 1)
            {
               GenerateBossRoom(generationPosition, direction);
            }
            else
            {
               GenerateRoomInDirection(generationPosition, direction);
            }
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

   private void GenerateBossRoom(Vector2 generationPosition, Vector2 direction)
   {
      _roomsLayout[(int)generationPosition.x + (int)direction.x, (int)generationPosition.y + (int)direction.y] =
         GetBossRoomData();
   }

   //! DEBUG // TODO: Remove
   private void DebugPrintDungeonLayout()
   {
      for (int i = 0; i < _roomsLayout.GetLength(0); i++)
      {
         for (int j = 0; j < _roomsLayout.GetLength(1); j++)
         {
            if (_roomsLayout[i, j] != null)
            {
               if (_roomsLayout[i, j].isStartRoom)
               {
                  Debug.DrawLine(new Vector3(i, j, 0), new Vector3(i, j + 1, 0), Color.blue, 100f);
               }
               else if(_roomsLayout[i, j].isBossRoom)
               {
                  Debug.DrawLine(new Vector3(i, j, 0), new Vector3(i, j + 1, 0), Color.red, 100f);
               }
               else
               {
                  Debug.DrawLine(new Vector3(i, j, 0), new Vector3(i, j + 1, 0), Color.green, 100f);
               }
            }
            else
            {
               Debug.DrawLine(new Vector3(i, j, 0), new Vector3(i, j + 1, 0), Color.white, 100f);
            }
         }
      }
   }
}

[CustomEditor(typeof(DungeonManager))]
public class DungeonManagerEditor : Editor
{
   public override void OnInspectorGUI()
   {
      base.OnInspectorGUI();

      DungeonManager dungeonManager = (DungeonManager)target;

      GUILayout.Space(10);

      if (GUILayout.Button("Regenerate Dungeon Layout"))
      {
         dungeonManager.ResetDungeon();
      }
   }
}
