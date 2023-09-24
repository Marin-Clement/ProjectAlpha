using UnityEngine;

public class InteractableFloorDoor : InteractableObject
{
    protected override void Interact()
    {
        DungeonManager.Instance.ChangeFloor();
        Debug.Log("Change Floor");
    }

    protected override void SetDetail()
    {
        throw new System.NotImplementedException();
    }
}