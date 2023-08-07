using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player_UI : MonoBehaviour
{
    // Player reference
    private Player_Behaviour _playerBehaviour;

    [Header("Dash UI")]
    [SerializeField] private GameObject dashContainer;
    [SerializeField] private Image dashIcon;
    [SerializeField] private Image dashBar1;
    [SerializeField] private Image dashBar2;
    [SerializeField] private Slider dashTimer;

    [Header("Spell1 UI")]
    [SerializeField] private GameObject spell1Container;
    [SerializeField] private Image spell1Icon;
    [SerializeField] private Slider spell1Timer;
    [SerializeField] private Slider spell1ArrowTimer;

    [Header("Spell2 UI")]
    [SerializeField] private GameObject spell2Container;
    [SerializeField] private Image spell2Icon;
    [SerializeField] private Slider spell2Timer;

    [Header("Health UI")]
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private Slider healthBar;
    [SerializeField] private Slider healthTempBar;

    void Start()
    {
        _playerBehaviour = GetComponent<Player_Behaviour>();

    }
    private void LateUpdate() 
    {
        UpdateDashUI();
        UpdateMainSpellUI();
        UpdateHealthUI();
    }

    public void UpdateDashUI()
    {
        dashTimer.maxValue = _playerBehaviour.playerMovement.DashCd;
        if (_playerBehaviour.playerMovement.GetDashTimerCount() == 0)
        {
            dashTimer.value = dashTimer.maxValue;
        }
        else
        {
            dashTimer.value = _playerBehaviour.playerMovement.GetDashTimerCount();
        }
        switch (_playerBehaviour.playerMovement.GetDashCount())
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

    public void UpdateMainSpellUI()
    {
        spell1Timer.maxValue = _playerBehaviour.playerCombat.MaxCooldown;
        spell1ArrowTimer.maxValue = _playerBehaviour.playerCombat.HoldTime;
        if (_playerBehaviour.playerCombat.CurrentCooldown <= 0)
        {
            spell1Timer.value = spell1Timer.maxValue;
        }
        else
        {
            spell1Timer.value = _playerBehaviour.playerCombat.CurrentCooldown;
        }
        spell1ArrowTimer.value = _playerBehaviour.playerCombat.HeldTime;
    }

    public void UpdateHealthUI()
    {
        healthBar.maxValue = _playerBehaviour.playerHealth.MaxHealth;
        healthTempBar.maxValue = _playerBehaviour.playerHealth.MaxHealth;
        healthText.text = _playerBehaviour.playerHealth.HealthValue.ToString() + "/" + _playerBehaviour.playerHealth.MaxHealth.ToString();
        healthBar.value = Mathf.Lerp(healthBar.value, _playerBehaviour.playerHealth.HealthValue, 0.1f);
        healthTempBar.value = Mathf.Lerp(healthTempBar.value, healthBar.value, 0.01f);
    }
    public void animateDashIcon()
    {
        StartCoroutine(AnimateContainerCoroutine(dashContainer.transform));
        StartCoroutine(AnimateIconCoroutine(dashIcon));
    }

    public void animateMainSpellIcon()
    {
        StartCoroutine(AnimateContainerCoroutine(spell1Container.transform));
        StartCoroutine(AnimateIconCoroutine(spell1Icon));
    }
    
    private IEnumerator AnimateIconCoroutine(Image Icon)
    {
        Color originalColor = Icon.color;
        Icon.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
        for (float i = 0; i <= 1f; i += 0.2f)
        {
            
            Icon.color = new Color(originalColor.r, originalColor.g, originalColor.b, i);
            yield return new WaitForSeconds(0.05f);
        }
    }

    private IEnumerator AnimateContainerCoroutine(Transform Container)
    {
        float baseScale = 0.5f;
        Container.localScale = new Vector3(baseScale, baseScale, 1f);
        for (float i = baseScale; i <= 0.8f; i += 0.02f)
        {
            Container.localScale = new Vector3(i,i,1f);
            yield return new WaitForSeconds(0.01f);
        }
    }
}
