using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player_UI : MonoBehaviour
{
    [Header("Dash UI")]

    [SerializeField] private Image DashIcon;
    [SerializeField] private Image dashBar1;
    [SerializeField] private Image dashBar2;
    [SerializeField] private Slider dashTimer;
    [SerializeField] private TextMeshProUGUI indevText;
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
                dashBar1.enabled = false;
                dashBar2.enabled = false;
                break;
            case 1:
                dashBar1.enabled = true;
                dashBar2.enabled = false;
                break;
            case 2:
                dashBar1.enabled = true;
                dashBar2.enabled = true;
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

    public void animateDashIcon()
    {
        StartCoroutine(AnimateIconCoroutine(DashIcon));
    }
    
    private IEnumerator AnimateIconCoroutine(Image Icon)
    {
        Color originalColor = Icon.color;
        // change alpha to 0
        Icon.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
        for (float i = 0; i < 1f; i += 0.1f)
        {
            Icon.color = new Color(originalColor.r, originalColor.g, originalColor.b, i);
            yield return new WaitForSeconds(0.05f);
        }
    }
}
