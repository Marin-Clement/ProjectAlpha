using UnityEngine;

public class DoorSpawner : MonoBehaviour
{
    [SerializeField] private bool isTopDoor;
    [SerializeField] private bool isBottomDoor;
    [SerializeField] private bool isLeftDoor;
    [SerializeField] private bool isRightDoor;


    public void SpawnDoor(GameObject door)
    {
        door = Instantiate(door, transform.position, Quaternion.identity);
        InteractableDoor doorScript = door.GetComponent<InteractableDoor>();
        if (isTopDoor)
        {
            doorScript.SetDirection(Vector2.up);
        }
        else if (isBottomDoor)
        {
            doorScript.SetDirection(Vector2.down);
        }
        else if (isLeftDoor)
        {
            doorScript.SetDirection(Vector2.left);
        }
        else if (isRightDoor)
        {
            doorScript.SetDirection(Vector2.right);
        }

        doorScript.SetLocked(true);
        Destroy(gameObject);
    }
}