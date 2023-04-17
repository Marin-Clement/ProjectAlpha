using UnityEditor.UI;
using UnityEngine;

public class Player_Combat : MonoBehaviour
{
    private Player_Behaviour _playerBehaviour;
    
    [SerializeField]
    private GameObject arrowPrefab;
    [SerializeField]
    private SpriteRenderer rangeIndicator;
    
    // Bow Stats
    private float _heldTime;
    

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
            if (_heldTime > 0.7f)
            {
                _heldTime = 0.7f;
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
        var arrow = Instantiate(arrowPrefab, playerTranform.position, playerTranform.rotation);
        var projectileBehaviour = arrow.GetComponent<Projectile_Behaviour>();
        projectileBehaviour.Duration = (_heldTime + 0.15f) * 1.2f;
        projectileBehaviour.SetDirection(playerTranform.up);
    }
    
    public void SetRangeIndicator(float range)
    {
        var transform1 = rangeIndicator.transform;
        transform1.localScale = new Vector3(0.5f, (range + 0.25f) * 20.5f, 1);
        transform1.localPosition = new Vector3(0, (range + 0.25f) * 10.25f, 0);
    }
}
