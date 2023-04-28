using UnityEngine;
using UnityEngine.UI;

public class Player_UI : MonoBehaviour
{
    [SerializeField]
    private Image dashIcon1;
    [SerializeField]
    private Image dashIcon2;
    [SerializeField]
    private Slider dashTimer;
    private GameObject _dashTimerObject;
    
    private Player_Movement _playerMovement;
    void Start()
    {
        _playerMovement = GetComponent<Player_Movement>();
        _dashTimerObject = dashTimer.gameObject;
    }

    private void Update()
    {
        dashTimer.maxValue = _playerMovement.DashCd;
        dashTimer.value = _playerMovement.GetDashTimerCount();
        _dashTimerObject.SetActive(_playerMovement.GetDashTimerCount() != 0);
        switch (_playerMovement.GetDashCount())
        {
            case 0:
                dashIcon1.enabled = false;
                dashIcon2.enabled = false;
                break;
            case 1:
                dashIcon1.enabled = true;
                dashIcon2.enabled = false;
                break;
            case 2:
                dashIcon1.enabled = true;
                dashIcon2.enabled = true;
                break;
        }
    }
}
