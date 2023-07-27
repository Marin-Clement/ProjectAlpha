using UnityEngine;

public class Player_Combat : MonoBehaviour
{
    private Player_Behaviour _playerBehaviour;

    // Arrow Variables
    [Header("Arrow Variables")]
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Projectile_Data arrowData;
    private LineRenderer _lineRenderer;
    
    // Bow Stats
    private float _heldTime;
    private const float MinHoldTime = 0.2f;
    

    private void Start()
    {
        _playerBehaviour = GetComponent<Player_Behaviour>();
        _lineRenderer = GetComponentInChildren<LineRenderer>();
        _lineRenderer.enabled = false;
    }

    public void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            _heldTime += Time.deltaTime;
            if (_heldTime > arrowData.lifeTime)
            {
                _heldTime = arrowData.lifeTime;
            }
        }
        if (!Input.GetKeyUp(KeyCode.Mouse0)) return;
        if (_heldTime > 0)
        {
            Attack();
        }
        _heldTime = 0;
    }

    private void Attack()
    {
        var playerTranform = transform;
        var up = playerTranform.up;
        var arrow = Instantiate(arrowPrefab, playerTranform.position + (up), Quaternion.identity);
        var projectileBehaviour = arrow.GetComponent<Projectile_Behaviour>();
        projectileBehaviour.Duration = (_heldTime + MinHoldTime) * 1.2f;
        projectileBehaviour.SetDirection(up);
    }
}
