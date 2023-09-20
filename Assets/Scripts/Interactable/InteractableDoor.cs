using UnityEngine;

public class InteractableDoor : InteractableObject
{
    private Vector2 _direction;
    private bool _isLocked;
    [SerializeField] GameObject floatingText;

    protected override void Interact()
    {
        if (!_isLocked)
        {
            DungeonManager.Instance.ChangeRoom(_direction);
        }
        else
        {
            GameObject text = Instantiate(floatingText, transform.position, Quaternion.identity);
            text.GetComponent<FloatingText>().SetText("Locked", Color.red);
        }
    }

    public void SetDirection(Vector2 direction)
    {
        _direction = direction;
    }

    public Vector2 GetDirection()
    {
        return _direction;
    }

    public void SetLocked(bool isLocked)
    {
        _isLocked = isLocked;
    }

    public bool GetLocked()
    {
        return _isLocked;
    }
}