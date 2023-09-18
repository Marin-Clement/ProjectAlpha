using UnityEngine;

public class InteractableDoor : InteractableObject
{
    private Vector2 _direction;
    protected override void Interact()
    {
        DungeonManager.Instance.ChangeRoom(_direction);
    }

    public void SetDirection(Vector2 direction)
    {
        _direction = direction;
    }

    public Vector2 GetDirection()
    {
        return _direction;
    }
}