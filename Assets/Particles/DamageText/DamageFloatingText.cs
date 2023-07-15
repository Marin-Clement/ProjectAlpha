using UnityEngine;
using TMPro;

namespace Particles.DamageText
{
    public class DamageFloatingText : MonoBehaviour
    {
        public float speed = 2f; // Floating speed
        public float duration = 1f; // Duration of the popup
        public AnimationCurve curve; // Animation curve for the floating motion
        public Vector2 sizeRange = new Vector2(1f, 1.5f); // Range for random size
        public Vector2 directionRange = new Vector2(-1f, 1f); // Range for random direction
        public AnimationCurve sizeCurve; // Animation curve for size variation over time
        public float damage;
        public GameObject criticalHitSprite;
        public bool isCritical;
        
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
                SetText(damage.ToString(), new Color(1f, 0.1746475f, 0.009134057f, 1f));
                criticalHitSprite.SetActive(true);
            }
            else
            {
                SetText(damage.ToString(), Color.white);
            }
        }

        private void Update()
        {
            timer += Time.deltaTime;

            // Floating animation using sine function and animation curve
            float xOffset = curve.Evaluate(timer / duration);
            Vector3 floatingOffset = new Vector3(xOffset, 0f, 0f) * (speed * Time.deltaTime);
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

        private void SetText(string text, Color color)
        {
            textMeshPro.text = text;
            textMeshPro.color = color;
        }
    }
}
