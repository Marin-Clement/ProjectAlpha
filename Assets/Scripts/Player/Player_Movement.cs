using System.Collections;
using UnityEngine;

public class Player_Movement :  MonoBehaviour
{
    // Properties
    public bool isMoving { get; private set; }

    // Player reference
    private Player_Behaviour _playerBehaviour;

    private SpriteRenderer _spriteRenderer;

    // Variables
    private Rigidbody2D _rigidbody;

    // Basic Movement
    [Header("Basic Movement")]
    [SerializeField] private int speed;
    private Vector2 _movement;

    
    // Dash Movement
    [Header("Dash Movement")]
    [SerializeField] private int dashForce;
    [SerializeField] private float dashCd;
    [SerializeField] private GameObject dashClone;
    private bool _isDashing;
    private int _dashCount = 2;
    private bool _dashTimer;
    private float _dashTimerCount;
    
    
    void Start()
    {
        _playerBehaviour = GetComponent<Player_Behaviour>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }
    
    void Update()
    {
        _movement.x = Input.GetAxisRaw("Horizontal");
        _movement.y = Input.GetAxisRaw("Vertical");

        isMoving = _movement != Vector2.zero;

        _spriteRenderer.flipX = _movement.x < 0;

        if (Input.GetKeyDown(KeyCode.Space) && !_isDashing && _dashCount > 0 && isMoving)
        {
            Dash();
            _playerBehaviour.playerUi.animateDashIcon();
        }

        if (!_dashTimer && _dashCount < 2)
        {
            StartCoroutine(DashTimer());
            _dashTimer = true;
        }

        if (_dashTimer)
        {
            _dashTimerCount += Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        if (!_isDashing)
        {
            Move();
        }
    }
    
    private void Move()
    {
        _rigidbody.velocity = _movement.normalized * speed;
    }
    
    private void Dash()
    {
        _isDashing = true;
        _dashCount--;
        _rigidbody.AddForce(_movement.normalized * (dashForce * 2), ForceMode2D.Impulse);
        StartCoroutine(DashClone());
        StartCoroutine(DashCooldown());
    }
    
    // knockback in opoosite direction of enemy
    public void Knockback(Vector3 enemyPosition)
    {
        _isDashing = true;
        _rigidbody.AddForce((transform.position - enemyPosition).normalized * (dashForce * 5), ForceMode2D.Impulse);
        StartCoroutine(KnockbackCooldown());
    }
    
    
    IEnumerator DashCooldown()
    {
        yield return new WaitForSeconds(0.2f);
        _isDashing = false;
    }
    
    IEnumerator KnockbackCooldown()
    {
        yield return new WaitForSeconds(0.1f);
        _isDashing = false;
    }
    
    IEnumerator DashTimer()
    {
        yield return new WaitForSeconds(dashCd);
        _dashCount += 1;
        _dashTimerCount = 0;
        _dashTimer = false;
    }

    IEnumerator DashClone()
    {
        for (int i = 0; i < 5; i++)
        {
            yield return new WaitForSeconds(0.05f);
            GameObject clone = Instantiate(dashClone, transform.position, Quaternion.identity);
            SpriteRenderer cloneSpriteRenderer = clone.GetComponent<SpriteRenderer>();
            cloneSpriteRenderer.flipX = _spriteRenderer.flipX;
            cloneSpriteRenderer.sprite = _spriteRenderer.sprite;
        }
    }

    public int GetDashCount()
    {
        return _dashCount;
    }
    
    public float GetDashTimerCount()
    {
        return _dashTimerCount;
    }
    
    // Getter and Setter
    public float DashCd
    {
        get => dashCd;
        set => dashCd = value;
    }
}
