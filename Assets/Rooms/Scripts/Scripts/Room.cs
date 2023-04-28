using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{   
    [SerializeField] private RoomData roomData;
    
    [Header("Door Data")]
    private bool _hasNorthDoor;
    private bool _hasSouthDoor;
    private bool _hasEastDoor;
    private bool _hasWestDoor;
    private bool _isStartRoom;
    

    [Header("Room Prefab")]
    private GameObject _roomPrefab;
    
    [Header("Room Data")]
    private List<GameObject> _enemies;
    private List<GameObject> _traps;
    
    [Header("Room Loot")]
    private List<GameObject> _loot;
    
    [Header("Room Type")]
    private bool _isCombatRoom;
    private bool _isTrapRoom;
    private bool _isLootRoom;
    private bool _isBossRoom;
    

    private void Start()
    {
        SetRoomVariables();
        SpawnEnemies();
        if (_isStartRoom)
        {
            var position = transform.position;
            var player = Instantiate(GameManager.Instance.player, position, Quaternion.identity);
            var playerCamera = Instantiate(GameManager.Instance.playerCamera, position, Quaternion.identity);
            playerCamera.GetComponent<Player_Camera>().player = player.GetComponent<Player_Movement>();
        }
    }
    
    // Spawn enemies on enemy spawner 
    public void SpawnEnemies()
    {
        foreach (Transform child in transform)
        {
            if (child.CompareTag("EnemySpawner"))
            {
                foreach (var enemy in _enemies)
                {
                    child.GetComponent<EnemySpawner>().SpawnEnemy(enemy);
                }
            }
        }
    }
    
    

    private void SetRoomVariables()
    {
        _hasNorthDoor = roomData.hasNorthDoor;
        _hasSouthDoor = roomData.hasSouthDoor;
        _hasEastDoor = roomData.hasEastDoor;
        _hasWestDoor = roomData.hasWestDoor;
        _isStartRoom = roomData.isStartRoom;
        _enemies = roomData.enemies;
        _traps = roomData.traps;
        _loot = roomData.loot;
        _isCombatRoom = roomData.isCombatRoom;
        _isTrapRoom = roomData.isTrapRoom;
        _isLootRoom = roomData.isLootRoom;
        _isBossRoom = roomData.isBossRoom;
    }
    
    public bool GetHasNorthDoor()
    {
        return _hasNorthDoor;
    }
    
    public bool GetHasSouthDoor()
    {
        return _hasSouthDoor;
    }
    
    public bool GetHasEastDoor()
    {
        return _hasEastDoor;
    }
    
    public bool GetHasWestDoor()
    {
        return _hasWestDoor;
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
}
