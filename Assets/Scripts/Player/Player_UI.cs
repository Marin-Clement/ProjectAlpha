using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player_UI : MonoBehaviour
{
    [SerializeField]
    private Image dashIcon1;
    [SerializeField]
    private Image dashIcon2;
    [SerializeField]
    private Slider dashTimer;
    [SerializeField]
    private TextMeshProUGUI indevText;
    private RectTransform indevTextRectTransform;
    private GameObject _dashTimerObject;
    private Player_Movement _playerMovement;

    // Debug
    // TODO: Remove this when the game is finished
    private RectTransform canvasRectTransform;

    void Start()
    {
        _playerMovement = GetComponent<Player_Movement>();
        indevTextRectTransform = indevText.GetComponent<RectTransform>();
        canvasRectTransform = indevText.transform.parent.GetComponent<RectTransform>();
        _dashTimerObject = dashTimer.gameObject;
    }

    private void Update()
    {
        KeepIndevTextAtTopRightCorner();
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


    private void KeepIndevTextAtTopRightCorner()
    {
        float canvasWidth = canvasRectTransform.rect.width;
        float canvasHeight = canvasRectTransform.rect.height;
        float xOffset = canvasWidth / 2f - indevTextRectTransform.rect.width / 2f;
        float yOffset = canvasHeight / 2f + indevTextRectTransform.rect.height / 2f;

        indevTextRectTransform.localPosition = new Vector3(xOffset + 30f, (yOffset - 30f) + Mathf.Sin(Time.time * 2f) * 10f, 0f);
    }
}
