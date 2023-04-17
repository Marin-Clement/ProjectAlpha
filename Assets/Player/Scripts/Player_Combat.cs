using UnityEditor.UI;
using UnityEngine;

public class Player_Combat : MonoBehaviour
{
    private Player_Behaviour _playerBehaviour;
    
    [SerializeField]
    private GameObject _arrowPrefab;

    private void Start()
    {
        _playerBehaviour = GetComponent<Player_Behaviour>();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Attack();
        }
    }
    
    // ReSharper disable Unity.PerformanceAnalysis
    public void Attack()
    {
        var playerTranform = this.transform;
        var arrow = Instantiate(_arrowPrefab, playerTranform.position, playerTranform.rotation);
        var projectileBehaviour = arrow.GetComponent<Projectile_Behaviour>();
        projectileBehaviour.SetDirection(playerTranform.up);
    }
}
