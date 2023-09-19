using TMPro;
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
      _roomsLayout = new RoomData[rooms + 2, rooms + 2];
      _visitedRooms = new bool[rooms + 2, rooms + 2];
      _currentRoomPosition = new Vector2(rooms/2, rooms/2);
      _floor = 1;
      GenerateDungeonLayout();
      GenerateRoom(Vector2.zero);
   }
   //! Singleton

   // Dungeon UI
   [Header("Dungeon UI")]
   [SerializeField] private TMP_Text roomText;
   [SerializeField] private TMP_Text roomNameText;

   // Dungeon generation variables
   [Space(20),Header("Dungeon Generation")]

   [SerializeField] private int rooms;
   [SerializeField] private RoomData[] roomsData;
   [SerializeField] private RoomData[] bossRoomsData;
   [SerializeField] private RoomData startRoom;
   private readonly Vector2[] _directions = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };


   private RoomData[,] _roomsLayout;
   private Vector2 _currentRoomPosition;

   // Live variables
   private GameObject _currentRoom;
   private bool[,] _visitedRooms;
   private int _floor;

   // DEBUG
   [SerializeField, Space(20), Header("Debug")]
   private bool debug;

   // Generate the Room
   private void GenerateRoom(Vector2 direction)
   {
      RoomData roomData = _roomsLayout[(int)_currentRoomPosition.x, (int)_currentRoomPosition.y];
      if (roomData)
      {
         _currentRoom = Instantiate(roomData.roomPrefab, transform);
         Room room = _currentRoom.GetComponent<Room>();
         room.playerSpawnPosition = direction;
         if (_visitedRooms[(int)_currentRoomPosition.x, (int)_currentRoomPosition.y])
         {
            room.visited = true;
         }
         else
         {
            _visitedRooms[(int)_currentRoomPosition.x, (int)_currentRoomPosition.y] = true;
         }
      }
      else
      {
         Debug.LogError("Room data is null");
      }
      //? Update UI
      int x = (int)_currentRoomPosition.x - rooms / 2;
      int y = (int)_currentRoomPosition.y - rooms / 2;
      roomText.text = "(" + x + "," + y + ")";
      roomNameText.text = roomData.roomPrefab.name;
   }

   // ReSharper disable Unity.PerformanceAnalysis
   public void ChangeRoom(Vector2 direction)
   {
      _currentRoomPosition += direction;
      Room room = _currentRoom.GetComponent<Room>();
      room.DestroyDoors();
      room.DestroyEnemies();
      Destroy(_currentRoom);
      GenerateRoom(direction);
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

   // ReSharper disable Unity.PerformanceAnalysis
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

      Vector2 currentRoomGenerationPosition = new Vector2(rooms/2, rooms/2);
      _roomsLayout[(int)currentRoomGenerationPosition.x, (int)currentRoomGenerationPosition.y] = startRoom;

      for (int i = 1; i < rooms; i++)
      {
         bool roomGenerated = TryGenerateRoomInAllDirections(currentRoomGenerationPosition, i);
         if (!roomGenerated)
         {
            Vector2 direction = _directions[Random.Range(0, _directions.Length)];
            foreach(Vector2 dir in _directions)
            {
               if (IsRoomInDirectionEmpty(currentRoomGenerationPosition, dir))
               {
                  direction = dir;
                  break;
               }
            }
            currentRoomGenerationPosition += direction;
         }
         if (!roomGenerated)
         {
            i--;
         }
      }
      if (debug)
      {
         Debug.Log("<color=green>Generated dungeon layout in " + ((Time.realtimeSinceStartup - startTime)* 1000) + "ms</color>");
         DebugPrintDungeonLayout();
      }
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

   // left door = 0, right door = 1, top door = 2, bottom door = 3
   public bool[] GetRoomDoors()
   {
      bool[] doors = new bool[4];
      foreach (Vector2 direction in _directions)
      {
         if (!IsRoomInDirectionEmpty(_currentRoomPosition, direction))
         {
            doors[ArrayUtility.IndexOf(_directions, direction)] = true;
         }
      }
      return doors;
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

   public RoomData[,] GetRoomsLayout()
   {
      return _roomsLayout;
   }

   public Vector2 GetCurrentRoomPosition()
   {
      return _currentRoomPosition;
   }
}

[CustomEditor(typeof(DungeonManager))]
public class DungeonManagerEditor : Editor
{
   public override void OnInspectorGUI()
   {
      base.OnInspectorGUI();

      DungeonManager dungeonManager = (DungeonManager)target;

      GUILayout.Space(30);

      if (GUILayout.Button("Regenerate Dungeon Layout"))
      {
         dungeonManager.ResetDungeon();
      }
   }
}
