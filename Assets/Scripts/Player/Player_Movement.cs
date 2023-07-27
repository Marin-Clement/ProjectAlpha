using System.Collections;
using UnityEngine;

public class Player_Movement :  MonoBehaviour
{
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
    private bool _isDashing;
    private int _dashCount = 2;
    private bool _dashTimer;
    private float _dashTimerCount;

    // playerUI

    private Player_UI _playerUi;



    void Start()
    {
        _playerUi = GetComponentInParent<Player_UI>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }
    
    void Update()
    {
        // player look at mouse
        Vector3 mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        var transform1 = transform;
        var position = transform1.position;
        Vector2 direction = new Vector2(mousePos.x - position.x, mousePos.y - position.y);
        transform1.up = direction;



        _movement.x = Input.GetAxisRaw("Horizontal");
        _movement.y = Input.GetAxisRaw("Vertical");
        
        
        if (Input.GetKeyDown(KeyCode.Space) && !_isDashing && _dashCount > 0 && _movement != Vector2.zero)
        {
            Dash();
            _playerUi.animateDashIcon();
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
        StartCoroutine(DashCooldown());
    }
    
    // knockback in opoosite direction of enemy
    public void Knockback(Vector3 enemyPosition)
    {
        _isDashing = true;
        _rigidbody.AddForce((transform.position - enemyPosition).normalized * (dashForce * 3), ForceMode2D.Impulse);
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
