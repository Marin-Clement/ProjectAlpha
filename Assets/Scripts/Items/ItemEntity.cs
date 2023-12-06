using System;
using UnityEngine;

public abstract class ItemEntity : MonoBehaviour
{
    private GameObject _target;
    [SerializeField] private int _range;
    [SerializeField] private float _speed;
    private Rigidbody2D _rigidbody2D;

    private bool _isDestroyed = false;

    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _target = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    private void Update()
    {
        Vector2 position = transform.position;
        // if player is in range
        if (Vector2.Distance(position, _target.transform.position) < _range)
        {
            // move towards player
            Vector2 targetPosition = _target.transform.position;
            Vector2 desiredDirection = (targetPosition - position).normalized;

            Vector2 newDirection =
                Vector2.Lerp(_rigidbody2D.velocity.normalized, desiredDirection, _speed * Time.deltaTime);
            _rigidbody2D.velocity = newDirection * _speed;
            Debug.DrawLine(position, desiredDirection, Color.red);
        }

        if (_isDestroyed)
        {
            DestroyAnimation();
        }
    }

    private void DestroyAnimation()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        // change opacity
        Color color = spriteRenderer.color;
        color.a -= 1f * Time.deltaTime * 4;
        spriteRenderer.color = color;
        transform.localScale -= new Vector3(0.5f, 0.5f, 0.5f) * Time.deltaTime * 4;
    }

    protected abstract void Action(Collider2D other);

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _range);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Action(other);
            _isDestroyed = true;
            Destroy(gameObject, 0.2f);
        }

        if (other.CompareTag("Wall"))
        {
            _rigidbody2D.velocity = Vector2.zero;
        }
    }
}