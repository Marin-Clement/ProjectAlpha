using UnityEditor.UI;
using UnityEngine;

public class Player_Combat : MonoBehaviour
{
    private Player_Behaviour _playerBehaviour;
    
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private SpriteRenderer rangeIndicator;
    
    // Bow Stats
    private float _heldTime;
    private const float MinHoldTime = 0.2f;
    [SerializeField] private Projectile_Data arrowData;

    private void Start()
    {
        _playerBehaviour = GetComponent<Player_Behaviour>();
        rangeIndicator.gameObject.SetActive(false);
    }

    public void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            rangeIndicator.gameObject.SetActive(true);
            _heldTime += Time.deltaTime;
            if (_heldTime > arrowData.lifeTime)
            {
                _heldTime = arrowData.lifeTime;
            }
            SetRangeIndicator(_heldTime);
        }
        if (!Input.GetKeyUp(KeyCode.Mouse0)) return;
        if (_heldTime > 0)
        {
            rangeIndicator.gameObject.SetActive(false);
            Debug.Log("Held Time: " + _heldTime);
            Attack();
        }
        _heldTime = 0;
    }
    
    public void Attack()
    {
        var playerTranform = transform;
        var arrow = Instantiate(arrowPrefab, playerTranform.position + (playerTranform.up), Quaternion.identity);
        var projectileBehaviour = arrow.GetComponent<Projectile_Behaviour>();
        projectileBehaviour.Duration = (_heldTime + MinHoldTime) * 1.2f;
        projectileBehaviour.SetDirection(playerTranform.up);
    }
    
    public void SetRangeIndicator(float range)
    {
        var transform1 = rangeIndicator.transform;
        transform1.localScale = new Vector3(0.5f, (arrowData.speed * (range + MinHoldTime) * 1.2f), 1);
        transform1.localPosition = new Vector3(0, (transform1.localScale.y / 2), 0);
    }
}
