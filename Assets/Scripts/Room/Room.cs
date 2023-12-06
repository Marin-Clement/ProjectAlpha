using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Room : MonoBehaviour
{
    [SerializeField] private RoomData roomData;

    [Header("Door Data")] private bool _isStartRoom;
    private bool _isNewFloor;

    [Header("Room Prefab")] private GameObject _roomPrefab;
    [SerializeField] private GameObject doorPrefab;

    [Header("Room Data")] private List<GameObject> _enemies;
    private List<GameObject> _traps;

    [Header("Room Loot")] private List<GameObject> _loot;

    [Header("Room Type")] private bool _isCombatRoom;
    private bool _isTrapRoom;
    private bool _isLootRoom;
    private bool _isBossRoom;

    [Header("Room Status")] private bool _isCleared;

    // Live variables
    private bool _enemiesSpawned;
    public bool visited;
    public Vector2 playerSpawnPosition;
    private List<GameObject> _doorsAlive;
    private List<GameObject> _enemiesAlive;

    private void Start()
    {
        SetRoomVariables();
        SpawnDoors();
        SpawnPlayer();

        //! Todo: Remove this if statement
        if (_isStartRoom && !visited && !_isNewFloor)
        {
            SpawnEnemies();
        }

        if (!visited)
        {
            if (_isStartRoom && !_isNewFloor)
            {
                var position = transform.position;
                var player = Instantiate(roomData.playerPrefab, position, Quaternion.identity);
                var playerCamera = Instantiate(roomData.playerCameraPrefab, position, Quaternion.identity);
                player.GetComponent<Player_Behaviour>().SetPlayerCamera(playerCamera.GetComponent<PlayerCamera>());
                playerCamera.GetComponent<PlayerCamera>().player = player.GetComponent<Player_Movement>();
            }
            else
            {
                SpawnEnemies();
            }
        }
        else
        {
            foreach (var door in _doorsAlive)
            {
                door.GetComponent<InteractableDoor>().SetLocked(false);
            }
        }

        if (!_isBossRoom && !_isStartRoom) return;

        foreach (var door in _doorsAlive)
        {
            door.GetComponent<InteractableDoor>().SetLocked(false);
        }
    }

    private void Update()
    {
        if (visited) return;
        if (!_isCleared) return;

        if (_enemiesSpawned)
        {
            CheckIfClear();
        }

        GameObject[] doors = GameObject.FindGameObjectsWithTag("Door");
        foreach (var door in doors)
        {
            door.GetComponent<InteractableDoor>().SetLocked(false);
        }
    }

    private void CheckIfClear()
    {
        if (_enemiesAlive == null) Debug.Log("EnemiesAlive Didn't Spawn Yet");
        foreach (var enemy in _enemiesAlive)
        {
            if (enemy != null) return;
        }

        _isCleared = true;
    }


    private void SpawnPlayer()
    {
        if (playerSpawnPosition == Vector2.up)
        {
            foreach (var door in _doorsAlive.Where(door =>
                         door.GetComponent<InteractableDoor>().GetDirection() == Vector2.down))
            {
                GameManager.Instance.player.transform.position = door.transform.position;
            }
        }
        else if (playerSpawnPosition == Vector2.down)
        {
            foreach (var door in _doorsAlive.Where(door =>
                         door.GetComponent<InteractableDoor>().GetDirection() == Vector2.up))
            {
                GameManager.Instance.player.transform.position = door.transform.position;
            }
        }
        else if (playerSpawnPosition == Vector2.right)
        {
            foreach (var door in _doorsAlive.Where(door =>
                         door.GetComponent<InteractableDoor>().GetDirection() == Vector2.left))
            {
                GameManager.Instance.player.transform.position = door.transform.position;
            }
        }
        else if (playerSpawnPosition == Vector2.left)
        {
            foreach (var door in _doorsAlive.Where(door =>
                         door.GetComponent<InteractableDoor>().GetDirection() == Vector2.right))
            {
                GameManager.Instance.player.transform.position = door.transform.position;
            }
        }
    }


    // Spawn enemies on enemy spawner 
    private void SpawnEnemies()
    {
        StartCoroutine(SpawnEnemyTimer());
    }

    private IEnumerator SpawnEnemyTimer()
    {
        foreach (Transform child in transform)
        {
            if (!child.CompareTag("EnemySpawner")) continue;
            EnemySpawner enemySpawner = child.GetComponent<EnemySpawner>();
            enemySpawner.SetCanSpawn(true);
        }

        yield return new WaitForSeconds(1f);
        foreach (Transform child in transform)
        {
            if (!child.CompareTag("EnemySpawner")) continue;
            EnemySpawner enemySpawner = child.GetComponent<EnemySpawner>();
            enemySpawner.SpawnEnemy(_enemies[Random.Range(0, _enemies.Count)]);
        }

        _enemiesAlive = GameObject.FindGameObjectsWithTag("Enemy").ToList();
        _enemiesSpawned = true;
    }

    // left door = 0, right door = 1, top door = 2, bottom door = 3
    private void SpawnDoors()
    {
        var doorsToSpawn = DungeonManager.Instance.GetRoomDoors();
        var doorSpawners =
            (from Transform child in transform where child.CompareTag("DoorSpawner") select child.gameObject).ToList();

        for (var i = 0; i < doorsToSpawn.Length; i++)
        {
            if (doorsToSpawn[i])
            {
                doorSpawners[i].GetComponent<DoorSpawner>().SpawnDoor(doorPrefab);
            }
        }

        _doorsAlive = GameObject.FindGameObjectsWithTag("Door").ToList();
    }

    public void DestroyDoors()
    {
        foreach (var door in _doorsAlive)
        {
            Destroy(door);
        }
    }

    public void DestroyEnemies()
    {
        if (_enemiesAlive == null) return;
        foreach (var enemy in _enemiesAlive)
        {
            Destroy(enemy);
        }
    }

    private void SetRoomVariables()
    {
        _isStartRoom = roomData.isStartRoom;
        _enemies = roomData.enemies;
        _traps = roomData.traps;
        _loot = roomData.loot;
        _isCombatRoom = roomData.isCombatRoom;
        _isTrapRoom = roomData.isTrapRoom;
        _isLootRoom = roomData.isLootRoom;
        _isBossRoom = roomData.isBossRoom;
    }

    public RoomData GetRoomData()
    {
        return roomData;
    }

    public bool GetIsStartRoom()
    {
        return _isStartRoom;
    }

    public GameObject GetRoomPrefab()
    {
        return _roomPrefab;
    }

    public List<GameObject> GetEnemies()
    {
        return _enemies;
    }

    public List<GameObject> GetTraps()
    {
        return _traps;
    }

    public List<GameObject> GetLoot()
    {
        return _loot;
    }

    public bool GetIsCombatRoom()
    {
        return _isCombatRoom;
    }

    public bool GetIsTrapRoom()
    {
        return _isTrapRoom;
    }

    public bool GetIsLootRoom()
    {
        return _isLootRoom;
    }

    public bool GetIsBossRoom()
    {
        return _isBossRoom;
    }

    public void SetIsNewFloor()
    {
        _isNewFloor = true;
    }
}