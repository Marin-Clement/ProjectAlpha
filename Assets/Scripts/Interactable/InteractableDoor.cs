using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class InteractableDoor : InteractableObject
{
    private Vector2 _direction;
    private bool _isLocked;
    [SerializeField] GameObject floatingText;

    private Material _material;
    private ParticleSystem _particleSystem;
    private readonly Color _lockedColor = new(4f, 0f, 0f, 1f);
    private readonly Color _unlockedColor = new(4f, 0f, 1.83f, 1f);

    private new void Start()
    {
        base.Start();
        _material = GetComponentInChildren<SpriteRenderer>().material;
        _particleSystem = GetComponent<ParticleSystem>();
    }

    protected override void SetDetail()
    {
        var particleSystemMain = _particleSystem.main;
        if (_isLocked)
        {
            _material.SetColor("_Color", _lockedColor * 2);
            particleSystemMain.startColor = _lockedColor;
        }
        else
        {
            _material.SetColor("_Color", _unlockedColor * 2);
            particleSystemMain.startColor = _unlockedColor;
        }
    }

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