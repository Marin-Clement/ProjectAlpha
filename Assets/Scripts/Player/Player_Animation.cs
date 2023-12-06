using UnityEngine;

public class Player_Animation : MonoBehaviour
{
    private Player_Behaviour _playerBehaviour;
    private bool _isMoving;
    private Animator _animator;

    void Start()
    {
        _playerBehaviour = GetComponent<Player_Behaviour>();
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        _isMoving = _playerBehaviour.playerMovement.isMoving;
        _animator.SetBool("isMoving", _isMoving && !_playerBehaviour.playerCombat.isAttacking);
        _animator.SetBool("isAttacking", _playerBehaviour.playerCombat.isAttacking);
    }
}