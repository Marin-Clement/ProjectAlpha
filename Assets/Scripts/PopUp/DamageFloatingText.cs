using UnityEngine;
using TMPro;

public class DamageFloatingText : MonoBehaviour
{

    [SerializeField] private TMP_Text text; // Text component
    [SerializeField] private Color damageColor; // Color of the text
    [SerializeField] private float speed = 2f; // Floating speed
    [SerializeField] private float duration = 1f; // Duration of the popup
    [SerializeField] private Vector2 sizeRange = new Vector2(1f, 1.5f); // Range for random size
    [SerializeField] private Vector2 directionRange = new Vector2(-1f, 1f); // Range for random direction
    [SerializeField] private AnimationCurve sizeCurve; // Animation curve for size variation over time
    [SerializeField] private GameObject criticalHitSprite;
    private bool isCritical;
    private float damage;

    private TextMeshPro textMeshPro;
    private float timer;
    private float size;
    private Vector3 direction;

    private void Awake()
    {
        textMeshPro = GetComponent<TextMeshPro>();
        textMeshPro.sortingOrder = 999;
    }

    private void Start()
    {
        // Generate random size within the specified range
        size = Random.Range(sizeRange.x, sizeRange.y);

        // Generate random direction within the specified range
        float directionX = Random.Range(directionRange.x, directionRange.y);
        float directionY = Random.Range(directionRange.x, directionRange.y);
        direction = new Vector3(directionX, directionY, 0f).normalized;
        if (isCritical)
        {
            SetText(damage, new Color(1f,0.5529412f,0.2039216f));
            criticalHitSprite.SetActive(true);
        }
        else
        {
            SetText(damage, damageColor);
        }
    }

    private void Update()
    {
        timer += Time.deltaTime;

        // Floating animation using sine function and animation curve
        Vector3 floatingOffset = new Vector3(1f, 0f, 0f) * (speed * Time.deltaTime);
        Vector3 positionOffset = direction * floatingOffset.magnitude;
        transform.position += positionOffset;

        // Scale the text based on the random size and time-based size variation
        float sizeMultiplier = sizeCurve.Evaluate(timer / duration);
        transform.localScale = new Vector3(size * sizeMultiplier, size * sizeMultiplier, size * sizeMultiplier);

        // Fade out the text over time
        // float alpha = 1f - (timer / duration);
        // textMeshPro.color = new Color(textMeshPro.color.r, textMeshPro.color.g, textMeshPro.color.b, alpha);

        // Destroy the popup when the duration is over
        if (timer >= duration)
        {
            Destroy(gameObject);
        }
    }

    private void SetText(float text, Color color)
    {
        textMeshPro.text = text.ToString("0");
        textMeshPro.color = color;
    }

    public bool IsCritical{
        get => isCritical;
        set => isCritical = value;
    }

    public float Damage{
        get => damage;
        set => damage = value;
    }

    public Color Color{
        get => damageColor;
        set => damageColor = value;
    }
}
